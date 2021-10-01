using Quoridor.Models;
using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Controllers
{
    public class QuoridorControllerWithEvents : QuoridorController
    {
        public event Action<IField> GameStarted;
        public QuoridorControllerWithEvents(QuoridorModel quoridorModel) : base(quoridorModel) { }

        public override void StartGame()
        {
            base.StartGame();
            GameStarted?.Invoke(QuoridorModel.Field);
        }
    }
}