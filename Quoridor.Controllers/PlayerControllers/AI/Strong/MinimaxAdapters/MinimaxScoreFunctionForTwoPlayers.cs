using AStarPathfinding;
using Quoridor.Models;
using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI.Strong.MinimaxAdapters
{
    public static class MinimaxScoreFunction
    {
        public static double CalculateForTwoPlayers(MinimaxStateAdapter minimaxAdapter, IPlayer player)
        {
            if (minimaxAdapter is null)
            {
                throw new ArgumentNullException(nameof(minimaxAdapter));
            }
            if (player is null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            QuoridorModel quoridorModel = minimaxAdapter.QuoridorModel;
            if (quoridorModel.Players.Length != 2)
            {
                throw new InvalidOperationException($"There must be two players in the game." +
                    " Parameter name: " + nameof(quoridorModel.Players));
            }

            IPlayer firstPlayer = quoridorModel.Players[0];
            IPlayer secondPlayer = quoridorModel.Players[1];
            IPlayer opponent = player == firstPlayer ? secondPlayer : firstPlayer;

            return CalculateForPlayer(player, opponent, quoridorModel);
        }
        static double CalculateForPlayer(IPlayer player, IPlayer opponent, QuoridorModel quoridorModel)
        {
            // Maximum number of steps to win in a straight line
            int maxStepsToVictory = quoridorModel.Field.Height - 1;

            // A number that shows how close the player is to victory
            double score = maxStepsToVictory / CalculateStepsToVictoryForPlayer(player, quoridorModel);

            double opponentStepsToVictory = CalculateStepsToVictoryForPlayer(opponent, quoridorModel);

            if (opponentStepsToVictory < 2)
                score -= maxStepsToVictory / opponentStepsToVictory;
            else if (opponentStepsToVictory < 4)
                score -= maxStepsToVictory / opponentStepsToVictory / 2;

            return score;
        }
        static double CalculateStepsToVictoryForPlayer(IPlayer player, QuoridorModel quoridorModel)
        {
            IFieldNode finded = AStar.FindTheShortestPath(quoridorModel.Field, player.Position,
                           player.Goal.Item2, player.Goal.Item1);
            int stepsToVictory = 0;
            if (finded != null)
            {
                IPathNode fieldNode = finded;
                while (fieldNode.PreviousPathNode != null)
                {
                    stepsToVictory++;
                    fieldNode = fieldNode.PreviousPathNode;
                }
            }
            return stepsToVictory;
        }
    }
}