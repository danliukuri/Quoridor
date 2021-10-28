using MinimaxAlgorithm;
using Quoridor.Controllers.PlayerControllers.AI.Strong.MinimaxAdapters;
using Quoridor.Controllers.UtilityModels;
using Quoridor.Models;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI.Strong
{
    public class StrongAIPlayerController : AIPlayerController
    {
        readonly Minimax<MinimaxStateAdapter, Player, Move> minimax;
        readonly MinimaxStateAdapter minimaxAdapter;
        const int maxDepth = 1000;

        public StrongAIPlayerController(QuoridorController quoridorController,
            Func<IState<Player, Move>, Player, double> scoreFunction) : base(quoridorController)
        {
            minimax = new Minimax<MinimaxStateAdapter, Player, Move>(scoreFunction, maxDepth);
            minimaxAdapter = new MinimaxStateAdapter(quoridorController);
        }
        public override void MakeMove() => minimaxAdapter.MakeMove(GetBestMove());
        protected virtual Move GetBestMove() => minimax.FindBestMove(minimaxAdapter);
    }
}