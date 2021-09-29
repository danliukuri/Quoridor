using Quoridor.Models;
using System;
using Quoridor.Models.Interfaces;

namespace Quoridor.Controllers
{
    public class QuoridorController
    {
        public QuoridorModel QuoridorModel { get; private set; }
        public QuoridorController(QuoridorModel quoridorModel)
        {
            if (quoridorModel is null)
            {
                throw new ArgumentNullException(nameof(quoridorModel));
            }

            QuoridorModel = quoridorModel;
        }
        public virtual void StartGame()
        {
            QuoridorModel.PlacePlayers();
        }

        public virtual void CheckIsGameEnd(IPlayer currentPlayer)
        {
            if (currentPlayer is null)
            {
                throw new ArgumentNullException(nameof(currentPlayer));
            }

            if (currentPlayer.Goal.Item1(currentPlayer.Position))
                EndGame(currentPlayer);
        }
        protected virtual void EndGame(IPlayer winner)
        {
            if (winner is null)
            {
                throw new ArgumentNullException(nameof(winner));
            }

            QuoridorModel.IsGameEnded = true;
            QuoridorModel.Winner = winner;
        }
    }
}