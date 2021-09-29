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
        bool GetRandomBool() => new Random().Next(0, 2) > 0 ? true : false; 
        
        public virtual void MakeMove()
        {   
            if(GetRandomBool())
                MakePlayerMove();
            else
                PlaceWall();
        }
        protected virtual void MakePlayerMove()
        {
            Direction direction = GetRandomDirection();
            playerController.MakePlayerMove(direction);
        }
        protected virtual void PlaceWall()
        {
            Direction direction = GetRandomDirection();
            int w = new Random().Next(0, quoridorModel.Field.Width);
            int h = new Random().Next(0, quoridorModel.Field.Height);
            playerController.PlaceWall(direction, w, h);
        }
    }
}
