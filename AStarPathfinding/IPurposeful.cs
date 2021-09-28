namespace AStarPathfinding
{
    public interface IPurposeful<T> where T : IPathNode
    {
        (AStar.IsGoal<T>, AStar.ComputeHScore<T>) Goal { get; set; }
    }
}
