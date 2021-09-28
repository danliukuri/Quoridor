namespace AStarPathfinding
{
    public interface IPathNode
    {
        int X { get; }
        int Y { get; }
        /// <summary>
        /// The F score is just G + H
        /// </summary>
        int F { get; set; }
        /// <summary>
        /// The G score is the distance from the starting point
        /// </summary>
        int G { get; set; }
        /// <summary>
        /// The H score is the estimated distance from the destination (calculated as the city block distance)
        /// </summary>
        int H { get; set; }
        IPathNode PreviousPathNode { get; set; }
    }
}