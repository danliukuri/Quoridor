using AStarPathfinding;

namespace Quoridor.Models.Interfaces
{
    public interface IPlayer : IPurposeful<IFieldNode>, INamed
    {
        IFieldNode Position { get; set; }

        int NumberOfWalls { get; set; }
    }
}
