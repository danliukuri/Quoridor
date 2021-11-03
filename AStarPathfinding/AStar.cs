using System;
using System.Collections.Generic;
using System.Linq;

namespace AStarPathfinding
{
    public static class AStar
    {
        public delegate bool IsGoal<T>(T value) where T : IPathNode;
        public delegate int ComputeHScore<T>(T value) where T : IPathNode;

        public static T FindTheShortestPath<T>(IGraph<T> graph, T start,
            ComputeHScore<T> computeHScore , IsGoal<T> goal) where T : IPathNode
        {
            if (graph is null)
            {
                throw new ArgumentNullException(nameof(graph));
            }
            if (start is null)
            {
                throw new ArgumentNullException(nameof(start));
            }
            if (computeHScore is null)
            {
                throw new ArgumentNullException(nameof(computeHScore));
            }
            if (goal is null)
            {
                throw new ArgumentNullException(nameof(goal));
            }
            graph.ResetPreviousPathNodes();

            T current = default;
            List<T> openList = new List<T>();
            List<T> closedList = new List<T>();
            int g = 0;

            // start by adding the original position to the open list
            openList.Add(start);

            while (openList.Count > 0)
            {
                // Get the square with the lowest F score
                var lowest = openList.Min(l => l.F);
                current = openList.First(l => l.F == lowest);

                // Add the current square to the closed list
                closedList.Add(current);

                // Remove it from the open list
                openList.Remove(current);

                // If we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(node => goal.Invoke(node)) != null)
                    break;

                T[] adjacentNodes = graph.GetWalkableAdjacentNodes(current);
                g = current.G + 1;

                for (int i = 0; i < adjacentNodes.Length; i++)
                {
                    // If this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.X == adjacentNodes[i].X && l.Y == adjacentNodes[i].Y) != null)
                        continue;

                    // If it's not in the open list...
                    if (openList.FirstOrDefault(l => l.X == adjacentNodes[i].X
                            && l.Y == adjacentNodes[i].Y) == null)
                    {
                        // Compute its score, set the parent
                        adjacentNodes[i].G = g;
                        adjacentNodes[i].H = computeHScore.Invoke(adjacentNodes[i]);
                        adjacentNodes[i].F = adjacentNodes[i].G + adjacentNodes[i].H;
                        adjacentNodes[i].PreviousPathNode = current;

                        // And add it to the open list
                        openList.Insert(0, adjacentNodes[i]);
                    }
                    else
                    {
                        // Test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (g + adjacentNodes[i].H < adjacentNodes[i].F)
                        {
                            adjacentNodes[i].G = g;
                            adjacentNodes[i].F = adjacentNodes[i].G + adjacentNodes[i].H;
                            adjacentNodes[i].PreviousPathNode = current;
                        }
                    }
                }
                // There are no more tiles in the open list to process, which would indicate that there is no path between A and B
                if (openList.Count == 0)
                    current = default;
            }

            return current;
        }
    }
}
