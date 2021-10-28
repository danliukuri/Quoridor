using Quoridor.Models.General;
using System;

namespace Quoridor.Controllers.UtilityModels
{
    public static class DirectionExtentions
    {
        public static Direction GetTheOppositeDirection(Direction direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException($"The value \"{direction}\" must be one of the Direction enumeration")
        };

        public static Direction GetRandomDirection() => (Direction)new Random().Next(0, 4);
    }
}