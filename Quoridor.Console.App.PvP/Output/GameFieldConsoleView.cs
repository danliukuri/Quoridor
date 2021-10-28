using Quoridor.Controllers;
using Quoridor.Models;
using Quoridor.Models.General;
using Quoridor.Models.Interfaces;
using System;

namespace Quoridor.Console.App.PvP.Output
{
    class GameFieldConsoleView
    {
        const int wallDimension = 2;

        const char horizontalBorder = '═';
        const char verticalBorder = '║';
        const char centerBorder = '╬';
        const char cell = ' ';

        const char horizontalWall = '■';
        const char verticalWall = '█';

        char bordersPlaceholder;
        char centerPlaceholder;

        int[] verticalWallСonnectionСounters;
        int horizontalWallСonnectionСounter;

        QuoridorControllerWithEvents quoridorControllerWithEvents;

        public GameFieldConsoleView(QuoridorControllerWithEvents quoridorControllerWithEvents)
        {
            this.quoridorControllerWithEvents = quoridorControllerWithEvents;
        }

        public void Output(IField field)
        {
            OutputHorizontalCellNumbers(field);

            verticalWallСonnectionСounters = new int[field.Width];
            for (int i = 0; i < verticalWallСonnectionСounters.Length; i++)
                verticalWallСonnectionСounters[i] = 0;

            for (int i = 0; i < field.Height * 2 + 1; i++)
            {
                OutputVerticalCellNumbers(i);
                ConfigureForFieldOutput(i);

                horizontalWallСonnectionСounter = 0;

                System.Console.Write(bordersPlaceholder);
                // Inside each cell
                for (int j = 0; j < field.Width; j++)
                {
                    ConfigureForWallOutput(i, j, field);

                    // The cell inside consists of three characters
                    for (int k = 0; k < 3; k++)
                    {   // Center of cell
                        if (i % 2 != 0 && k == 1)
                        {
                            INamed player = TryToGetPlayerOnPosition(field.GetCell(j, i / 2));
                            System.Console.Write(player?.Mark ?? centerPlaceholder);
                        }
                        else
                            System.Console.Write(centerPlaceholder);
                    }
                    System.Console.Write(bordersPlaceholder);
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }

        /// <summary>
        /// Outputs the first line - horizontal cell numbers
        /// </summary>
        /// <param name="field"></param>
        void OutputHorizontalCellNumbers(IField field)
        {
            System.Console.Write(" # ");
            for (int j = 0; j < field.Width; j++)
            {
                System.Console.Write(verticalBorder);
                System.Console.Write((j + 1).ToString("000"));
            }
            System.Console.WriteLine(verticalBorder);
        }
        /// <summary>
        /// Outputs the first column - vertical cell numbers 
        /// </summary>
        /// <param name="i">Index by height</param>
        void OutputVerticalCellNumbers(int i)
        {
            if (i % 2 == 0)
                System.Console.Write($"{horizontalBorder}{horizontalBorder}{horizontalBorder}");
            else
                System.Console.Write((i / 2 + 1).ToString("000"));
        }

        void ConfigureForFieldOutput(int i)
        {
            if (i % 2 == 0)
            {
                bordersPlaceholder = centerBorder;
                centerPlaceholder = horizontalBorder;
            }
            else
            {
                bordersPlaceholder = verticalBorder;
                centerPlaceholder = cell;
            }
        }

        void ConfigureForWallOutput(int i, int j, IField field)
        {
            ConfigureForVerticalWallOutput(i, j, field);
            ConfigureForHorizontalWallOutput(i, j, field);
        }
        void ConfigureForVerticalWallOutput(int i, int j, IField field)
        {
            // If the vertical wall exists, we need configure to draw it
            if (i % 2 != 0 && field.GetCell(j, i / 2).Walls.Get(Direction.Right) != null)
            {
                bordersPlaceholder = verticalWall;
                verticalWallСonnectionСounters[j]++;
            }
            else if (i % 2 == 0 && i / 2 - 1 >= 0 && i / 2 < field.Height &&
                field.GetCell(j, i / 2).Walls.Get(Direction.Right) != null)
            {
                if (verticalWallСonnectionСounters[j] % wallDimension != 0)
                {
                    bordersPlaceholder = verticalWall;
                }
                else
                {
                    bordersPlaceholder = centerBorder;
                }
            }
            else if (i % 2 == 0)
            {
                bordersPlaceholder = centerBorder;
            }
            else if (i % 2 != 0)
            {
                bordersPlaceholder = verticalBorder;
            }
        }
        void ConfigureForHorizontalWallOutput(int i, int j, IField field)
        {
            // If the horizontal wall exists, we need configure to draw it
            if (i % 2 == 0 && i / 2 - 1 >= 0 && i / 2 < field.Height &&
                field.GetCell(j, i / 2).Walls.Get(Direction.Up) != null)
            {
                centerPlaceholder = horizontalWall;
                if (horizontalWallСonnectionСounter % wallDimension == 0)
                {
                    bordersPlaceholder = horizontalWall;
                }
                else
                {
                    if (verticalWallСonnectionСounters[j] % wallDimension == 0)
                        bordersPlaceholder = centerBorder;
                }
                horizontalWallСonnectionСounter++;
            }
            else if (i % 2 == 0)
            {
                centerPlaceholder = horizontalBorder;
            }
        }

        INamed TryToGetPlayerOnPosition(IFieldNode position)
        {
            if (position is null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            Player player = null;
            IPlayer[] players = quoridorControllerWithEvents.QuoridorModel.Players;
            for (int i = 0; i < players.Length; i++)
                if (players[i].Position == position)
                    player = (Player)players[i];
            return player;
        }
    }
}
