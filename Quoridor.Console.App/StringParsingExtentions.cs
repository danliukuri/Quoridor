using Quoridor.ErrorHandling;
using Quoridor.Models;

namespace Quoridor.Console.App
{
    static class StringParsingExtentions
    {
        public static EitherLeftOrRight<ValidationError, Direction> ParseToDirection(this string str)
        {
            Direction direction;
            switch (str.ToLower())
            {
                case "up":
                    direction = Direction.Up;
                    break;
                case "down":
                    direction = Direction.Down;
                    break;
                case "left":
                    direction = Direction.Left;
                    break;
                case "right":
                    direction = Direction.Right;
                    break;
                default:
                    return new ValidationError($"There is no such direction ({str})");
            };
            return direction;
        }
        public static EitherLeftOrRight<ValidationError, int> ParseToInt(this string str)
        {
            if (!int.TryParse(str, out int result))
                return new ValidationError($"Can't parse '{str}' to int value");
            return result;
        }
    }
}
