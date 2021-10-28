using AStarPathfinding;
using Quoridor.Models.General;
using Quoridor.Models.Interfaces;

namespace Quoridor.Models
{
    public class FieldNode : IFieldNode
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        /// <summary>
        /// The F score is just G + H
        /// </summary>
        public int F { get; set; }
        /// <summary>
        /// The G score is the distance from the starting point
        /// </summary>
        public int G { get; set; }
        /// <summary>
        /// The H score is the estimated distance from the destination (calculated as the city block distance)
        /// </summary>
        public int H { get; set; }
        public IPathNode PreviousPathNode { get; set; }

        public Neighbors<IFieldNode> Neighbors { get; set; } = new Neighbors<IFieldNode>();
        public Neighbors<IWall> Walls { get; set; } = new Neighbors<IWall>();
        public FieldNode(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool IsItPossibleMoveTo(IFieldNode fieldNode)
        {
            bool isItPossibleMove = false;
            if (fieldNode != null)
            {
                IFieldNode[] neighbors = Neighbors.GetAll();
                for (int i = 0; i < neighbors.Length; i++)
                    if (fieldNode == neighbors[i])
                        if(Walls.GetAll()[i] == null)
                            isItPossibleMove = true;
            }
            return isItPossibleMove;
        }
    }
}
