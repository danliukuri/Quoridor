namespace AStarPathfinding
{
    public interface IGraph<T> where T : IPathNode
    {
        T[] GetWalkableAdjacentNodes(T current);
    }
}