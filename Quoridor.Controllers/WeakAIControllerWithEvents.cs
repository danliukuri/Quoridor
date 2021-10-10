using Quoridor.ErrorHandling;
using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Controllers
{
    public class WeakAIControllerWithEvents : WeakAIController
    {
        public event Action<IField> FieldUpdated;

        public event Action<IPlayer> WallPlaced;

        public event Action<IPlayer> AIWon;
        public WeakAIControllerWithEvents(QuoridorController quoridorController) : base(quoridorController) { }
        void CheckIsThereWinner()
        {
            IPlayer winner = quoridorModel.Winner;
            if (winner != null)
                AIWon?.Invoke(winner);
        }

        protected override EitherLeftOrVoid<ValidationError> TryToMakePlayerMove()
        {
            EitherLeftOrVoid<ValidationError> result = base.TryToMakePlayerMove();
            FieldUpdated?.Invoke(quoridorModel.Field);
            CheckIsThereWinner();
            return result;
        }

        protected override EitherLeftOrVoid<ValidationError> TryToPlaceWall()
        {
            EitherLeftOrVoid<ValidationError> result = base.TryToPlaceWall();
            FieldUpdated?.Invoke(quoridorModel.Field);
            WallPlaced?.Invoke(quoridorModel.PreviousPlayer);
            return result;
        }
    }
}
