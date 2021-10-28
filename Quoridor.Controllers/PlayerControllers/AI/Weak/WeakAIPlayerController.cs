using Quoridor.Controllers.UtilityModels;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI.Weak
{
    public class WeakAIPlayerController : AIPlayerController
    {
        public WeakAIPlayerController(QuoridorController quoridorController) : base(quoridorController) { }

        bool GetRandomBool() => new Random().Next(0, 2) > 0;

        public override void MakeMove()
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
                Direction direction = DirectionExtentions.GetRandomDirection();
                result = playerController.TryToMakePlayerMove(direction);
            } while (result.IsLeft);
        }
        protected virtual void PlaceWall()
        {
            EitherLeftOrVoid<ValidationError> result;
            do
            {
                Direction direction = DirectionExtentions.GetRandomDirection();
                int w = new Random().Next(0, quoridorModel.Field.Width);
                int h = new Random().Next(0, quoridorModel.Field.Height);
                result = playerController.TryToPlaceWall(direction, w, h);
            } while (result.IsLeft);
        }
    }
}
