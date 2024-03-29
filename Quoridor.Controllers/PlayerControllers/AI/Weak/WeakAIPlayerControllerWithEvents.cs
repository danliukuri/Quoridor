﻿using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Controllers.PlayerControllers.AI.Weak
{
    public class WeakAIPlayerControllerWithEvents : WeakAIPlayerController
    {
        public event Action<IField> FieldUpdated;

        public event Action<IPlayer> WallPlaced;

        public event Action<IPlayer> AIWon;
        public WeakAIPlayerControllerWithEvents(QuoridorController quoridorController) : base(quoridorController) { }
        void CheckIsThereWinner()
        {
            IPlayer winner = quoridorModel.Winner;
            if (winner != null)
                AIWon?.Invoke(winner);
        }

        protected override void MakePlayerMove()
        {
            base.MakePlayerMove();
            FieldUpdated?.Invoke(quoridorModel.Field);
            CheckIsThereWinner();
        }

        protected override void PlaceWall()
        {
            base.PlaceWall();
            FieldUpdated?.Invoke(quoridorModel.Field);
            WallPlaced?.Invoke(quoridorModel.PreviousPlayer);
        }
    }
}
