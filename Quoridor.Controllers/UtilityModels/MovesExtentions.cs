using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using Quoridor.Models.Interfaces;
using System.Collections.Generic;

namespace Quoridor.Controllers.UtilityModels
{
    static class MovesExtentions
    {
        /// <summary>
        /// Search and add possible walks
        /// </summary>
        public static void AddPossibleWalks(this List<Move> moves, IPlayer[] players,
            IFieldNode currentPLayerPosition, IFieldNode[] currentPlayerNeighbors)
        {
            for (int i = 0; i < currentPlayerNeighbors.Length; i++)
                if (currentPLayerPosition.IsItPossibleMoveTo(currentPlayerNeighbors[i]))
                {
                    bool isWalkableAdjacentNode = true;
                    for (int j = 0; j < players.Length; j++)
                        if (currentPlayerNeighbors[i] == players[j].Position)
                            isWalkableAdjacentNode = false;

                    if (isWalkableAdjacentNode)
                        moves.Add(new Move((Direction)i)); // Add directions to walkable adjacent nodes
                }
        }

        /// <summary>
        /// Search and add possible jumps
        /// </summary>
        public static void AddPossibleJumps(this List<Move> moves, IPlayer[] players,
           IFieldNode currentPLayerPosition, IFieldNode[] currentPlayerNeighbors)
        {
            for (int i = 0; i < players.Length; i++)
            {
                IFieldNode adjacentPlayerCell = players[i].Position;
                for (int j = 0; j < currentPlayerNeighbors.Length; j++)
                {
                    Direction directionFromThisPlayer = (Direction)j;
                    // Check if there is a player another player on adjacent cells from the current player 
                    // and check if is there a wall between them
                    if (currentPlayerNeighbors[j] == adjacentPlayerCell &&
                        currentPLayerPosition.Walls.Get(directionFromThisPlayer) == null)
                    {
                        if (adjacentPlayerCell.Neighbors.Get(directionFromThisPlayer) != null &&
                        adjacentPlayerCell.Walls.Get(directionFromThisPlayer) == null)
                        {
                            // Add directions to jump through the adjacent player
                            moves.Add(new Move(new EitherLeftOrRight<Direction,
                                (Direction directionFromThisPlayer, Direction directionFromAnotherPlayer)>
                                (directionFromThisPlayer)));
                        }
                        else if (adjacentPlayerCell.Walls.Get(directionFromThisPlayer) != null ||
                            adjacentPlayerCell.Neighbors.Get(directionFromThisPlayer) == null)
                        {

                            IFieldNode[] adjacentPlayerCellNeighbors = adjacentPlayerCell.Neighbors.GetAll();
                            // Add directions to jump to the side through the adjacent player
                            for (int k = 0; k < adjacentPlayerCellNeighbors.Length; k++)
                            {
                                Direction directionFromAnotherPlayer = (Direction)k;
                                int halfOfDirectionCount = adjacentPlayerCellNeighbors.Length / 2;
                                if ((j < halfOfDirectionCount && k >= halfOfDirectionCount) ||
                                    (j >= halfOfDirectionCount &&  k < halfOfDirectionCount))
                                    if (adjacentPlayerCell.Neighbors.Get(directionFromAnotherPlayer) != null &&
                                        adjacentPlayerCell.Walls.Get(directionFromAnotherPlayer) == null)
                                    {
                                        moves.Add(new Move(
                                            new EitherLeftOrRight<Direction, (Direction directionFromThisPlayer,
                                                Direction directionFromAnotherPlayer)>(
                                                (directionFromThisPlayer, directionFromAnotherPlayer)
                                                )));
                                    }
                            }
                        }
                    }
                }
            }
        }
    }
}