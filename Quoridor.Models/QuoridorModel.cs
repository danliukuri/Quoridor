using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Models
{
    public class QuoridorModel
    {
        public IPlayer CurrentPlayer 
        { 
            get => Players[currentPlayerIndex];
            private set => Players[currentPlayerIndex] = value; 
        }
        public IPlayer PreviousPlayer
        {
            get
            {
                int previousPlayerIndex = currentPlayerIndex - 1;
                if (previousPlayerIndex < 0)
                    previousPlayerIndex = Players.Length - 1;
                return Players[previousPlayerIndex];
            }
        }
        public IPlayer Winner { get; set; }
        public bool IsGameEnded { get; set; }
        int currentPlayerIndex;

        public IField Field { get; private set; }
        public IPlayer[] Players { get; private set; }

        public QuoridorModel(IPlayer[] players, IField field)
        {
            if (players is null)
            {
                throw new ArgumentNullException(nameof(players));
            }
            if (!(players.Length == 2 || players.Length == 4))
            {
                throw new ArgumentException($"Must be 2 or 4 players (Length in '{nameof(players)}' array)");
            }
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                    throw new ArgumentNullException($"Player with index '{i}' is null in {nameof(players)} array");
            }
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            Field = field;
            Players = players;
        }

        public void Reset()
        {
            Winner = null;
            IsGameEnded = false;
            Field.Reset();
            PlacePlayers();
        }
        public void PlacePlayers()
        {
            if (Players.Length == 2)
            {
                Players[0].Position = Field.GetCell(Field.Width / 2, Field.Height - 1);
                Players[0].Goal = ((current) => current.Y == 0, (current) => Math.Abs(0 - current.Y));

                Players[1].Position = Field.GetCell(Field.Width / 2, 0);
                Players[1].Goal = ((current) => current.Y == Field.Height - 1,
                    (current) => Math.Abs(Field.Height - 1 - current.Y));
            }
            if (Players.Length == 4)
            {
                Players[0].Position = Field.GetCell(Field.Width / 2, Field.Height - 1);
                Players[0].Goal = ((current) => current.Y == 0, (current) => Math.Abs(0 - current.Y));

                Players[1].Position = Field.GetCell(0, Field.Height / 2);
                Players[1].Goal = ((current) => current.X == Field.Width - 1,
                    (current) => Math.Abs(Field.Width - 1 - current.X));

                Players[2].Position = Field.GetCell(Field.Width / 2, 0);
                Players[2].Goal = ((current) => current.Y == Field.Height - 1,
                    (current) => Math.Abs(Field.Height - 1 - current.Y));

                Players[3].Position = Field.GetCell(Field.Width - 1, Field.Height / 2);
                Players[3].Goal = ((current) => current.X == 0, (current) => Math.Abs(0 - current.X));
            }
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].NumberOfWalls = (Field.Height + Field.Width) / Players.Length + 1;
            }
            currentPlayerIndex = 0;
        }
        public void SwitchPlayer()
        {
            currentPlayerIndex++;
            if (currentPlayerIndex >= Players.Length)
                currentPlayerIndex = 0;
            CurrentPlayer = Players[currentPlayerIndex];
        }
    }
}