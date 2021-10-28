using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Controllers
{
    public class PlayerControllerWithEvents : PlayerController
    {
        public event Action<IField> FieldUpdated;

        public event Action<IPlayer> WallPlaced;

        public event Action<IPlayer> PlayerWon;
        public PlayerControllerWithEvents(QuoridorController quoridorController) : base(quoridorController) { }

        void CheckIsThereWinner()
        {
            IPlayer winner = quoridorModel.Winner;
            if (winner != null)
                PlayerWon?.Invoke(winner);
        }

        public override EitherLeftOrVoid<ValidationError> TryToMakePlayerMove(Direction direction)
        {
            EitherLeftOrVoid<ValidationError> result = base.TryToMakePlayerMove(direction);
            FieldUpdated?.Invoke(quoridorModel.Field);
            CheckIsThereWinner();
            return result;
        }
        public override EitherLeftOrVoid<ValidationError> TryToMakePlayerMove(Direction direction, Direction directionFromAnotherPlayer)
        {
            EitherLeftOrVoid<ValidationError> result =
                base.TryToMakePlayerMove(direction, directionFromAnotherPlayer);
            FieldUpdated?.Invoke(quoridorModel.Field);
            CheckIsThereWinner();
            return result;
        }

        public override EitherLeftOrVoid<ValidationError> TryToPlaceWall(Direction direction,
            int widthCoordinate, int heightCoordinate)
        {
            EitherLeftOrVoid<ValidationError> result =
                base.TryToPlaceWall(direction, widthCoordinate, heightCoordinate);
            FieldUpdated?.Invoke(quoridorModel.Field);
            WallPlaced?.Invoke(quoridorModel.PreviousPlayer);
            return result;
        }
    }
}