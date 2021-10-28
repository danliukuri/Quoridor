using Quoridor.Models;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI
{
    public abstract class AIPlayerController
    {
        protected QuoridorModel quoridorModel;
        protected PlayerController playerController;
        public AIPlayerController(QuoridorController quoridorController)
        {
            if (quoridorController is null)
            {
                throw new ArgumentNullException(nameof(quoridorController));
            }

            quoridorModel = quoridorController.QuoridorModel;
            playerController = new PlayerController(quoridorController);
        }

        public abstract void MakeMove();
    }
}