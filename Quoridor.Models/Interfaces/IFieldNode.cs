﻿using AStarPathfinding;
using Quoridor.Models.General;

namespace Quoridor.Models.Interfaces
{
    public interface IFieldNode : IPathNode
    {
        Neighbors<IFieldNode> Neighbors { get; set; } 
        Neighbors<IWall> Walls { get; set; }
        bool IsItPossibleMoveTo(IFieldNode fieldNode);
    }
}
