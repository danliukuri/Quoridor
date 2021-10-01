using Quoridor.Models;
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

        public override void MakePlayerMove(Direction direction)
        {
            base.MakePlayerMove(direction);
            FieldUpdated?.Invoke(quoridorModel.Field);
            CheckIsThereWinner();
        }
        public override void MakePlayerMove(Direction direction, Direction directionFromAnotherPlayer)
        {
            base.MakePlayerMove(direction, directionFromAnotherPlayer);
            FieldUpdated?.Invoke(quoridorModel.Field);
            CheckIsThereWinner();
        }

        public override void PlaceWall(Direction direction, int widthCoordinate, int heightCoordinate)
        {
            base.PlaceWall(direction, widthCoordinate, heightCoordinate);
            FieldUpdated?.Invoke(quoridorModel.Field);
            WallPlaced?.Invoke(quoridorModel.PreviousPlayer);
        }
    }
}