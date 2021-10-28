using Quoridor.Controllers;
using Quoridor.Controllers.UtilityModels;
using Quoridor.Models;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using Quoridor.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Quoridor.Console.App.PvAI
{
    public class ConsoleCommandAdapter
    {
        readonly QuoridorController quoridorController;

        public ConsoleCommandAdapter(QuoridorController quoridorController)
        {
            this.quoridorController = quoridorController ?? throw new ArgumentNullException(nameof(quoridorController));
        }

        public Direction GetMoveParameters(string commandParameters)
        {
            int w = commandParameters.ToUpper()[0] - 'A';
            int h = int.Parse(commandParameters[1].ToString()) - 1;

            FieldNode fieldNode = (FieldNode)quoridorController.QuoridorModel.Field.GetCell(w, h);
            IFieldNode[] neighborFieldNodesOfCurrentPlayer =
                quoridorController.QuoridorModel.CurrentPlayer.Position.Neighbors.GetAll();

            Direction direction = default;

            for (int i = 0; i < neighborFieldNodesOfCurrentPlayer.Length; i++)
                if (fieldNode == neighborFieldNodesOfCurrentPlayer[i] &&
                    fieldNode != quoridorController.QuoridorModel.PreviousPlayer.Position) //playerS
                {
                    direction = (Direction)i;
                }

            return direction;
        }
        public EitherLeftOrRight<Direction, int> GetJumpParameters(string commandParameters)
        {
            int w = commandParameters.ToUpper()[0] - 'A';
            int h = int.Parse(commandParameters[1].ToString()) - 1;

            IFieldNode[] neighborFieldNodesOfCurrentPlayer =
                quoridorController.QuoridorModel.CurrentPlayer.Position.Neighbors.GetAll();
            IFieldNode[] ad = new IFieldNode[neighborFieldNodesOfCurrentPlayer.Length];
            for (int i = 0; i < neighborFieldNodesOfCurrentPlayer.Length; i++)
                ad[i] = neighborFieldNodesOfCurrentPlayer[i].Neighbors.Get((Direction)i);

            FieldNode fieldNode = (FieldNode)quoridorController.QuoridorModel.Field.GetCell(w, h);

            EitherLeftOrRight<Direction, int> result = default;

            bool isJumped = false;

            for (int i = 0; i < ad.Length; i++)
                if (fieldNode == ad[i])
                {
                    result = (Direction)i;
                    isJumped = true;
                }
            if (!isJumped)
            {
                List<IFieldNode> cornerFields = new List<IFieldNode>();
                IFieldNode node = quoridorController.QuoridorModel.CurrentPlayer.Position;

                if (node.X > 0 && node.Y > 0)
                    cornerFields.Add(quoridorController.QuoridorModel.Field.GetCell(node.X - 1, node.Y - 1));
                if (node.X < quoridorController.QuoridorModel.Field.Width - 1 && node.Y > 0)
                    cornerFields.Add(quoridorController.QuoridorModel.Field.GetCell(node.X + 1, node.Y - 1));
                if (node.X > 0 && node.Y < quoridorController.QuoridorModel.Field.Height - 1)
                    cornerFields.Add(quoridorController.QuoridorModel.Field.GetCell(node.X - 1, node.Y + 1));
                if (node.X < quoridorController.QuoridorModel.Field.Width - 1 &&
                    node.Y < quoridorController.QuoridorModel.Field.Height - 1)
                    cornerFields.Add(quoridorController.QuoridorModel.Field.GetCell(node.X + 1, node.Y + 1));

                for (int i = 0; i < cornerFields.Count; i++)
                    if (fieldNode == cornerFields[i])
                    {
                        result = i;
                    }
            }
            return result;
        }
        public (Direction, int w, int h) GetWallPlaceParameters(string commandParameters)
        {
            int w = commandParameters.ToUpper()[0] - 'S';
            int h = int.Parse(commandParameters[1].ToString()) - 1;
            bool isVertical = commandParameters.ToUpper()[2] == 'V';

            return (isVertical ? Direction.Right : Direction.Down, w, h);
        }


        public string[] GetWalkCommand(Direction direction)
        {
            IFieldNode playerPosition = quoridorController.QuoridorModel.CurrentPlayer.Position;
            IFieldNode fieldNode = playerPosition.Neighbors.Get(direction);

            char w = (char)(fieldNode.X + 'A');
            int h = fieldNode.Y + 1;
            return new string[] { "move", w.ToString(), h.ToString() };
        }
        public string[] GetJumpCommand(EitherLeftOrRight<Direction,
            (Direction directionFromThisPlayer, Direction directionFromAnotherPlayer)> jumpParameters)
        {
            IFieldNode playerPosition = quoridorController.QuoridorModel.CurrentPlayer.Position;

            IFieldNode filedNodeWhereWeJump;
            if (jumpParameters.IsLeft)
            {
                filedNodeWhereWeJump = playerPosition.Neighbors.Get(jumpParameters.LeftOrDefault());
            }
            else
            {
                (Direction directionFromThisPlayer, Direction directionFromAnotherPlayer) =
                    jumpParameters.RightOrDefault();

                IFieldNode fieldNode = playerPosition.Neighbors.Get(directionFromThisPlayer);
                filedNodeWhereWeJump = fieldNode.Neighbors.Get(directionFromAnotherPlayer);
            }
            char w = (char)(filedNodeWhereWeJump.X + 'A');
            int h = filedNodeWhereWeJump.Y + 1;
            return new string[] { "jump", w.ToString(), h.ToString() };
        }
        public string[] GetWallPlaceCommand(Direction direction, FieldNode fieldNode)
        {
            char w = (char)(fieldNode.X + 'S');
            int h = fieldNode.Y + 1;
            string wallDirection = direction == Direction.Right ? "v" : "h";
            return new string[] { "wall", w.ToString(), h.ToString(), wallDirection };
        }
        public string[] GetMoveCommand(Move move)
        {
            string[] moveCommand = default;
            if (move.IsWalk)
            {
                moveCommand = GetWalkCommand(move.GetWalkParameters());
            }
            else if (move.IsJump)
            {
                moveCommand = GetJumpCommand(move.GetJumpParameters());
            }
            else if (move.IsWallPlace)
            {
                (Direction direction, FieldNode fieldNode) = move.GetWallPlaceParameters();
                moveCommand = GetWallPlaceCommand(direction, fieldNode);
            }
            return moveCommand;
        }
    }
}