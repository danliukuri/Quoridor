using Quoridor.Models.General;
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
    }
}