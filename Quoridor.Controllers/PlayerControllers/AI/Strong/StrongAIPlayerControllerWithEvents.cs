using MinimaxAlgorithm;
using Quoridor.Controllers.UtilityModels;
using Quoridor.Models;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI.Strong
{
    public class StrongAIPlayerControllerWithEvents : StrongAIPlayerController
    {
        public event Action<Move> BestMoveFound;

        public StrongAIPlayerControllerWithEvents(QuoridorController quoridorController,
            Func<IState<Player, Move>, Player, double> scoreFunction) : base(quoridorController, scoreFunction) { }

        protected override Move GetBestMove()
        {
            Move move = base.GetBestMove();
            BestMoveFound?.Invoke(move);
            return move;
        }
    }
}