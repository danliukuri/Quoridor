using AStarPathfinding;

namespace Quoridor.Models.Interfaces
{
    public interface IField : IGraph<IFieldNode>
    {
        int Height { get; }
        int Width { get; }

        IFieldNode GetCell(int w, int h);
        void Reset();
    }
}