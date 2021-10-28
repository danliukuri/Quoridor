﻿using MinimaxAlgorithm;
using Quoridor.Controllers.UtilityModels;
using Quoridor.Models;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;
using Quoridor.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Quoridor.Controllers.PlayerControllers.AI.Strong.MinimaxAdapters
{
    public class MinimaxStateAdapter : IState<Player, Move>
    {
        public QuoridorModel QuoridorModel => quoridorModel;
        
        protected QuoridorModel quoridorModel;
        protected PlayerController playerController;
        public MinimaxStateAdapter(QuoridorController quoridorController)
        {
            if (quoridorController is null)
            {
                throw new ArgumentNullException(nameof(quoridorController));
            }

            quoridorModel = quoridorController.QuoridorModel;
            playerController = new PlayerController(quoridorController);
        }

        public bool IsTerminal => quoridorModel.IsGameEnded;
        public Player ActivePlayer => (Player)quoridorModel.CurrentPlayer;

        public IList<Move> GetPossibleMoves()
        {
            List<Move> moves = new List<Move>();

            IPlayer[] players = quoridorModel.Players;
            IFieldNode currentPLayerPosition = quoridorModel.CurrentPlayer.Position;
            IFieldNode[] currentPlayerNeighbors = currentPLayerPosition.Neighbors.GetAll();
            
            moves.AddPossibleWalks(players, currentPLayerPosition, currentPlayerNeighbors);

            return moves;
        }

        public void MakeMove(Move move)
        {
            if (move.IsWalk)
                playerController.TryToMakePlayerMove(move.GetWalkParameters());
            else if (move.IsJump)
            {
                EitherLeftOrRight<Direction, (Direction, Direction)> jump = move.GetJumpParameters();
                if (jump.IsLeft)
                {
                    playerController.TryToMakePlayerMove(jump.LeftOrDefault());
                }
                else
                {
                    (Direction directionFromThisPlayer, Direction directionFromAnotherPlayer) = jump.RightOrDefault();
                    playerController.TryToMakePlayerMove(directionFromThisPlayer, directionFromAnotherPlayer);
                }
            }
            else if (move.IsWallPlace)
                playerController.TryToPlaceWall(move.GetWallPlaceParameters().Item1,
                    move.GetWallPlaceParameters().Item2.X, move.GetWallPlaceParameters().Item2.Y);
        }
        public void UndoMove(Move move)
        {
            if (quoridorModel.IsGameEnded)
                quoridorModel.IsGameEnded = false;

            // Make previous player turn
            for (int i = 0; i < quoridorModel.Players.Length - 1; i++)
                quoridorModel.SwitchPlayer();

            // Undo is to reverse the previous turn
            if (move.IsWalk)
            {
                playerController.TryToMakePlayerMove(
                    DirectionExtentions.GetTheOppositeDirection(move.GetWalkParameters()));
            }
            else if (move.IsJump)
            {
                EitherLeftOrRight<Direction, (Direction, Direction)> jump = move.GetJumpParameters();
                if (jump.IsLeft)
                {
                    playerController.TryToMakePlayerMove(
                        DirectionExtentions.GetTheOppositeDirection(jump.LeftOrDefault()));
                }
                else
                {
                    (Direction directionFromThisPlayer, Direction directionFromAnotherPlayer) = jump.RightOrDefault();
                    playerController.TryToMakePlayerMove(
                        DirectionExtentions.GetTheOppositeDirection(directionFromAnotherPlayer),
                        DirectionExtentions.GetTheOppositeDirection(directionFromThisPlayer));
                }
            }
            else if (move.IsWallPlace)
            {
                playerController.TryToRemoveWall(move.GetWallPlaceParameters().Item2,
                    move.GetWallPlaceParameters().Item1);
            }
            // Make previous player turn
            for (int i = 0; i < quoridorModel.Players.Length - 1; i++)
                quoridorModel.SwitchPlayer();
        }
    }
}