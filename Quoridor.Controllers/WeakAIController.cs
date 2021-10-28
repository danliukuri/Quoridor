using Quoridor.Models;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using System;

namespace Quoridor.Controllers
{
    public class WeakAIController
    {
        protected QuoridorModel quoridorModel;
        protected QuoridorController quoridorController;
        protected PlayerController playerController;
        public WeakAIController(QuoridorController quoridorController)
        {
            if (quoridorController is null)
            {
                throw new ArgumentNullException(nameof(quoridorController));
            }

            this.quoridorController = quoridorController;
            quoridorModel = quoridorController.QuoridorModel;
            playerController = new PlayerController(quoridorController);
        }

        Direction GetRandomDirection() => (Direction)new Random().Next(0, 4);
        bool GetRandomBool() => new Random().Next(0, 2) > 0;

        public virtual void MakeMove()
        {
            if (GetRandomBool() && quoridorModel.CurrentPlayer.NumberOfWalls > 0)
                PlaceWall();
            else
                MakePlayerMove();
        }

        protected virtual void MakePlayerMove()
        {
            EitherLeftOrVoid<ValidationError> result;
            do
            {
                Direction direction = GetRandomDirection();
                result = playerController.TryToMakePlayerMove(direction);
            } while (result.IsLeft);
        }
        protected virtual void PlaceWall()
        {
            EitherLeftOrVoid<ValidationError> result;
            do
            {
                Direction direction = GetRandomDirection();
                int w = new Random().Next(0, quoridorModel.Field.Width);
                int h = new Random().Next(0, quoridorModel.Field.Height);
                result = playerController.TryToPlaceWall(direction, w, h);
            } while (result.IsLeft);
        }
    }
}
