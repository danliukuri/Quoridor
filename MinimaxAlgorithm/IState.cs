using System.Collections.Generic;

namespace MinimaxAlgorithm
{
    public interface IState<Player, Move>
        where Player : class
        where Move : class
    {
        IList<Move> GetPossibleMoves();
        void MakeMove(Move move);
        void UndoMove(Move move);

        bool IsTerminal { get; }
        Player ActivePlayer { get; }
    }
}