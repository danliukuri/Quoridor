using AStarPathfinding;
using Quoridor.Models;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using Quoridor.Models.Interfaces;
using System;


namespace Quoridor.Controllers
{
    public class PlayerController
    {
        protected QuoridorModel quoridorModel;
        protected QuoridorController quoridorController;
        public PlayerController(QuoridorController quoridorController)
        {
            if (quoridorController is null)
            {
                throw new ArgumentNullException(nameof(quoridorController));
            }

            this.quoridorController = quoridorController;
            quoridorModel = quoridorController.QuoridorModel;
        }

        public virtual EitherLeftOrVoid<ValidationError> TryToMakePlayerMove(Direction direction)
        {
            IFieldNode playerCell = quoridorModel.CurrentPlayer.Position;
            IFieldNode adjacentCell = playerCell.Neighbors.Get(direction);
            if (adjacentCell is null)
            {
                return new ValidationError($"Can't move player {direction.ToString().ToLower()} - " +
                    $"cell is null");
            }
            if (playerCell.Walls.Get(direction) != null)
            {
                return new ValidationError($"Can't move player {direction.ToString().ToLower()} - " +
                    $"there is a wall in the way");
            }

            bool isPlayerMoved = false;
            IPlayer[] players = quoridorModel.Players;
            // Check if there is a player on the cell in selected direction from player
            for (int i = 0; i < players.Length; i++)
            {
                IFieldNode adjacentPlayerCell = players[i].Position;
                if (adjacentCell == adjacentPlayerCell)
                {
                    EitherLeftOrVoid<ValidationError> result =
                        TryToMakePlayerMoveThroughTheAdjacentPlayer(adjacentPlayerCell, direction);
                    if (result.IsLeft)
                        return result;
                    isPlayerMoved = true;
                }
            }

            IPlayer currentPlayer = quoridorModel.CurrentPlayer;
            if (!isPlayerMoved)
                currentPlayer.Position = adjacentCell;
            quoridorController.CheckIsGameEnd(currentPlayer);
            quoridorModel.SwitchPlayer();
            return new EitherLeftOrVoid<ValidationError>();
        }
        public virtual EitherLeftOrVoid<ValidationError> TryToMakePlayerMove(
            Direction direction, Direction directionFromAnotherPlayer)
        {
            IFieldNode playerCell = quoridorModel.CurrentPlayer.Position;
            IFieldNode adjacentCell = playerCell.Neighbors.Get(direction);
            if (adjacentCell is null)
            {
                return new ValidationError($"Can't move player {direction.ToString().ToLower()} - " +
                    $"cell is null");
            }
            if (playerCell.Walls.Get(direction) != null)
            {
                return new ValidationError($"Can't move player {direction.ToString().ToLower()} - " +
                    $"there is a wall in the way");
            }

            bool isPlayerMoved = false;
            IPlayer[] players = quoridorModel.Players;
            // Check if there is a player on the cell in selected direction from player
            for (int i = 0; i < players.Length; i++)
            {
                IFieldNode adjacentPlayerCell = players[i].Position;
                if (adjacentCell == adjacentPlayerCell)
                {
                    EitherLeftOrVoid<ValidationError> result = TryToMakePlayerMoveToTheSideThroughTheAdjacentPlayer(
                        adjacentPlayerCell, direction, directionFromAnotherPlayer);
                    if (result.IsLeft)
                        return result;
                    isPlayerMoved = true;
                }
            }

            if (!isPlayerMoved)
            {
                return new ValidationError($"Can't move to the side through the player - " +
                    $"there is no other player to the {direction.ToString().ToLower()} of the player's " +
                    $"current position ({playerCell.X + 1},{playerCell.Y + 1})");
            }
            quoridorController.CheckIsGameEnd(quoridorModel.CurrentPlayer);
            quoridorModel.SwitchPlayer();
            return new EitherLeftOrVoid<ValidationError>();
        }
        EitherLeftOrVoid<ValidationError> TryToMakePlayerMoveThroughTheAdjacentPlayer(
            IFieldNode adjacentPlayerCell, Direction direction)
        {
            if (adjacentPlayerCell is null)
            {
                throw new ArgumentNullException(nameof(adjacentPlayerCell));
            }
            // Move through the player
            IFieldNode targetCell = adjacentPlayerCell.Neighbors.Get(direction);

            if (targetCell is null)
            {
                return new ValidationError($"Can't go through the player - the cell" +
                    $" to the {direction.ToString().ToLower()} of the player's cell" +
                    $"({adjacentPlayerCell.X + 1},{adjacentPlayerCell.Y + 1}) is null");
            }
            if (adjacentPlayerCell.Walls.Get(direction) != null)
            {
                return new ValidationError($"Can't go through the player - the cell" +
                    $"({adjacentPlayerCell.X + 1},{adjacentPlayerCell.Y + 1}) has a wall in the " +
                    $"{direction.ToString().ToLower()} direction");
            }

            quoridorModel.CurrentPlayer.Position = targetCell;
            return new EitherLeftOrVoid<ValidationError>();
        }
        EitherLeftOrVoid<ValidationError> TryToMakePlayerMoveToTheSideThroughTheAdjacentPlayer(
            IFieldNode adjacentPlayerCell, Direction direction, Direction directionFromAdjacentPlayer)
        {
            if (adjacentPlayerCell is null)
            {
                throw new ArgumentNullException(nameof(adjacentPlayerCell));
            }
            // Move to the side through the player
            IFieldNode targetCell = adjacentPlayerCell.Neighbors.Get(directionFromAdjacentPlayer);

            if (targetCell is null)
            {
                return new ValidationError($"Can't move to the side through the player - the cell" +
                    $" to the {directionFromAdjacentPlayer.ToString().ToLower()} of the player's cell" +
                    $"({adjacentPlayerCell.X + 1},{adjacentPlayerCell.Y + 1}) is null");
            }
            if (adjacentPlayerCell.Walls.Get(direction) is null && adjacentPlayerCell.Neighbors.Get(direction) != null)
            {
                return new ValidationError($"Can't move to the side through the player - the cell" +
                    $"({adjacentPlayerCell.X + 1},{adjacentPlayerCell.Y + 1}) has no wall in the " +
                    $"{direction.ToString().ToLower()} direction and you need to go through it");
            }
            if (adjacentPlayerCell.Walls.Get(directionFromAdjacentPlayer) != null)
            {
                return new ValidationError($"Can't move to the side through the player - the cell" +
                    $"({adjacentPlayerCell.X + 1},{adjacentPlayerCell.Y + 1}) has a wall in the " +
                    $"{directionFromAdjacentPlayer.ToString().ToLower()} direction");
            }

            quoridorModel.CurrentPlayer.Position = targetCell;
            return new EitherLeftOrVoid<ValidationError>();
        }

        public virtual EitherLeftOrVoid<ValidationError> TryToPlaceWall(Direction direction,
            int widthCoordinate, int heightCoordinate)
        {
            if (widthCoordinate < 0 || widthCoordinate >= quoridorModel.Field.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(widthCoordinate));
            }
            if (heightCoordinate < 0 || heightCoordinate >= quoridorModel.Field.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(heightCoordinate));
            }

            IPlayer currentPlayer = quoridorModel.CurrentPlayer;
            // Check if the current player has walls
            if (currentPlayer.NumberOfWalls <= 0)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward of the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - the current player " +
                    $"({currentPlayer.Name}) has no walls left to place");
            }

            IFieldNode cell = quoridorModel.Field.GetCell(widthCoordinate, heightCoordinate);

            if (cell is null)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward of the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - cell is null");
            }
            if (cell.Walls.Get(direction) != null)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward of the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - there is already a wall there");
            }

            Direction adjacentDirection = (direction == Direction.Up || direction == Direction.Down) ? Direction.Right : Direction.Down;
            IFieldNode adjacentCell = cell.Neighbors.Get(adjacentDirection);

            // Check neighbors in neighborDirection from selected cell
            if (adjacentCell is null)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward from the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - the cell on the " +
                    $"{adjacentDirection.ToString().ToLower()} is null.\n" +
                    $"The wall is placed in the direction from the selected cell and from the cell to the left of the " +
                    $"selected one or from the cell below the selected one, depending on the direction.");
            }
            else if (adjacentCell.Walls.Get(direction) != null)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward from the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - the cell to the " +
                    $"{adjacentDirection.ToString().ToLower()} of the selected already has a wall in the selected " +
                    $"direction ({direction.ToString().ToLower()}).\n" +
                    $"The wall is placed in the direction from the selected cell and from the cell to the left of the selected " +
                    $"one or from the cell below the selected one, depending on the direction.");
            }

            // Check neighbors in direction from selected cell
            else if (cell.Neighbors.Get(direction) is null)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                     $"ward of the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - cell to the " +
                     $"{direction.ToString().ToLower()} of the selected is null");
            }
            else if (adjacentCell.Neighbors.Get(direction) is null)
            {
                return new ValidationError($"Can't place the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward from the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - cell to the " +
                    $"{direction.ToString().ToLower()} of the cell on the {adjacentDirection.ToString().ToLower()} is null.\n" +
                    $"The wall is placed in the direction from the selected cell and from the cell to the left of the " +
                    $"selected one or from the cell below the selected one, depending on the direction.");
            }

            // Checking if another wall is in the way
            else if (cell.Walls.Get(adjacentDirection) != null &&
                cell.Neighbors.Get(direction)?.Walls.Get(adjacentDirection) != null)
            {
                // Checking maybe this is not a wall but two ends of different walls
                if (cell.Neighbors.Get(GetTheOppositeDirection(direction))?.Walls.Get(adjacentDirection) == null ||
                    cell.Neighbors.Get(direction)?.Neighbors.Get(direction)?.Walls.Get(adjacentDirection) == null)
                    return new ValidationError($"Can't place a wall to the {direction.ToString().ToLower()} of the cell " +
                        $"({widthCoordinate + 1},{heightCoordinate + 1}) - another wall interferes");
            }

            Wall wall = new Wall();
            cell.Neighbors.Get(direction).Walls.Set(GetTheOppositeDirection(direction), wall);
            cell.Walls.Set(direction, wall);
            adjacentCell.Neighbors.Get(direction).Walls.Set(GetTheOppositeDirection(direction), wall);
            adjacentCell.Walls.Set(direction, wall);

            // Check if the player can reach the goal
            IPlayer[] players = quoridorModel.Players;
            for (int i = 0; i < players.Length; i++)
            {
                IFieldNode finded = AStar.FindTheShortestPath(quoridorModel.Field, players[i].Position,
                    players[i].Goal.Item2, players[i].Goal.Item1);

                if (finded is null)
                {
                    EitherLeftOrVoid<ValidationError> result = TryToRemoveWall(cell, direction);
                    if (result.IsLeft)
                        return result;
                    return new ValidationError($"Can't place a wall to the {direction.ToString().ToLower()}" +
                        $" of the cell ({widthCoordinate + 1},{heightCoordinate + 1}) - the player {players[i].Mark}" +
                        $" cannot reach the finish line");
                }
            }

            currentPlayer.NumberOfWalls--;
            quoridorModel.SwitchPlayer();

            return new EitherLeftOrVoid<ValidationError>();
        }
        EitherLeftOrVoid<ValidationError> TryToRemoveWall(IFieldNode cell, Direction direction)
        {
            if (cell is null)
            {
                throw new NullReferenceException($"Can't remove the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward of the cell ({cell.X + 1},{cell.Y + 1}) - cell is null");
            }
            if (cell.Walls.Get(direction) is null)
            {
                return new ValidationError($"Can't remove the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward of the cell ({cell.X + 1},{cell.Y + 1}) - there is no wall there");
            }

            Direction adjacentDirection = (direction == Direction.Up || direction == Direction.Down) ? Direction.Right : Direction.Down;
            IFieldNode adjacentCell = cell.Neighbors.Get(adjacentDirection);

            if (adjacentCell is null)
            {
                return new ValidationError($"Can't remove the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward from the cell ({cell.X + 1},{cell.Y + 1}) - the cell on the " +
                    $"{adjacentDirection.ToString().ToLower()} is null.\n" +
                    $"The wall is placed in the direction from the selected cell and from the cell to the left of the selected " +
                    $"one or from the cell below the selected one, depending on the direction.");
            }
            if (adjacentCell.Walls.Get(direction) is null)
            {
                return new ValidationError($"Can't remove the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward from the cell ({cell.X + 1},{cell.Y + 1}) - the cell to the " +
                    $"{adjacentDirection.ToString().ToLower()} of the selected has not a wall in the selected " +
                    $"direction ({direction.ToString().ToLower()}).\n" +
                    $"The wall is placed in the direction from the selected cell and from the cell to the left of the selected " +
                    $"one or from the cell below the selected one, depending on the direction.");
            }

            // Check neighbors in direction from selected cell
            if (cell.Neighbors.Get(direction) is null)
            {
                return new ValidationError($"Can't remove the wall in the direction {direction.ToString().ToLower()}" +
                     $"ward of the cell ({cell.X + 1},{cell.Y + 1}) - cell to the " +
                     $"{direction.ToString().ToLower()} of the selected is null");
            }
            if (adjacentCell.Neighbors.Get(direction) is null)
            {
                return new ValidationError($"Can't remove the wall in the direction {direction.ToString().ToLower()}" +
                    $"ward from the cell ({cell.X + 1},{cell.Y + 1}) - cell to the " +
                    $"{direction.ToString().ToLower()} of the cell on the {adjacentDirection.ToString().ToLower()} is null.\n" +
                    $"The wall is placed in the direction from the selected cell and from the cell to the left of the " +
                    $"selected one or from the cell below the selected one, depending on the direction.");
            }

            cell.Neighbors.Get(direction).Walls.Set(GetTheOppositeDirection(direction), null);
            cell.Walls.Set(direction, null);
            adjacentCell.Neighbors.Get(direction).Walls.Set(GetTheOppositeDirection(direction), null);
            adjacentCell.Walls.Set(direction, null);

            return new EitherLeftOrVoid<ValidationError>();
        }

        Direction GetTheOppositeDirection(Direction direction) => direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentException($"The value \"{direction}\" must be one of the Direction enumeration")
        };
    }
}