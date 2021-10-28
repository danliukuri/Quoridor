using System;
using System.Linq;

namespace MinimaxAlgorithm
{
    public class Minimax<State, Player, Move>
        where State : IState<Player, Move>
        where Player : class
        where Move : class
    {
        readonly Func<State, Player, double> scoreFunction;
        readonly int maxDepth;
        int depth;

        public Minimax(Func<State, Player, double> scoreFunction, int maxDepth)
        {
            if (scoreFunction is null)
                throw new ArgumentNullException(nameof(scoreFunction));
            if (maxDepth < 1)
                throw new ArgumentException("The value must be greater than one.", nameof(maxDepth));

            this.scoreFunction = scoreFunction;
            this.maxDepth = depth = maxDepth;
        }

        public Move FindBestMove(State state)
        {
            if (state.IsTerminal)
            {
                var exMessage = "There should be no moves for a terminal state.";
                throw new InvalidOperationException(exMessage);
            }

            Player activePlayer = state.ActivePlayer;
            if (activePlayer == null)
            {
                var exMessage = "There must be an active player for an intermediate state.";
                throw new InvalidOperationException(exMessage);
            }

            depth = maxDepth;
            var (_, nextMove) = Estimate(state, activePlayer);
            return nextMove;
        }
        private (double Score, Move NextMove) Estimate(State state, Player player)
        {
            if (state.IsTerminal)
            {
                return
                (
                    Score: scoreFunction.Invoke(state, player),
                    NextMove: null
                );
            }

            if (depth <= 0)
            {
                return
                (
                    Score: scoreFunction.Invoke(state, player),
                    NextMove: null
                );
            }

            var possibleMoves = state.GetPossibleMoves();
            if (possibleMoves.Count == 0)
            {
                var exMessage = "At least one move must exist for an intermediate state.";
                throw new InvalidOperationException(exMessage);
            }

            var estimations = possibleMoves.Select((move) =>
            {
                state.MakeMove(move);
                depth--;
                var (score, _) = Estimate(state, player);
                state.UndoMove(move);

                return (Score: score, Move: move);
            });

            var isOpponentTurn = player != state.ActivePlayer;
            return isOpponentTurn
                ? estimations.Aggregate((e1, e2) => e1.Score < e2.Score ? e1 : e2)
                : estimations.Aggregate((e1, e2) => e1.Score > e2.Score ? e1 : e2);
        }
    }
}