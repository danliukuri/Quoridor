using Quoridor.ErrorHandling;
using Quoridor.Models;
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
            EitherLeftOrVoid<ValidationError> result;
            do
            {
                result = GetRandomBool() ? TryToMakePlayerMove() : TryToPlaceWall();
            } while (result.IsLeft);
        }
        protected virtual EitherLeftOrVoid<ValidationError> TryToMakePlayerMove()
        {
            Direction direction = GetRandomDirection();
            return playerController.TryToMakePlayerMove(direction);
        }
        protected virtual EitherLeftOrVoid<ValidationError> TryToPlaceWall()
        {
            Direction direction = GetRandomDirection();
            int w = new Random().Next(0, quoridorModel.Field.Width);
            int h = new Random().Next(0, quoridorModel.Field.Height);
            return playerController.TryToPlaceWall(direction, w, h);
        }
    }
}
