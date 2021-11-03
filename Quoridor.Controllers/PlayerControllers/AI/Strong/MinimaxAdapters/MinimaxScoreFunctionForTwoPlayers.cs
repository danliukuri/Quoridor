using AStarPathfinding;
using Quoridor.Models;
using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI.Strong.MinimaxAdapters
{
    public static class MinimaxScoreFunction
    {
        public static double CalculateForTwoPlayers(MinimaxStateAdapter minimaxAdapter, Player player)
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

            Player firstPlayer = (Player)quoridorModel.Players[0];
            Player secondPlayer = (Player)quoridorModel.Players[1];
            double score; // A number that shows how close the player is to victory

            if (player == firstPlayer)
            {
                score = CalculateForPlayer(firstPlayer, quoridorModel);
            }
            else
            {
                score = CalculateForPlayer(secondPlayer, quoridorModel);
            }
            return score;
        }

        static double CalculateForPlayer(Player player, QuoridorModel quoridorModel)
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
            // Value inversely proportional to the number of steps to victory
            return (double)(quoridorModel.Field.Height - 1) / stepsToVictory;
        }
    }
}