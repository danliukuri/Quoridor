using Quoridor.Models;
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
                // Value inversely proportional to the number of steps to victory
                score = (double)(quoridorModel.Field.Height - 1) / firstPlayer.Goal.Item2(firstPlayer.Position);
            }
            else
            {
                score = (double)(quoridorModel.Field.Height - 1) / secondPlayer.Goal.Item2(secondPlayer.Position);
            }
            return score;
        }
    }
}