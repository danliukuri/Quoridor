using Quoridor.Controllers;
using Quoridor.Models.Interfaces;

namespace Quoridor.Console.App.PvP.Output
{
    class ConsoleOutput
    {
        QuoridorControllerWithEvents quoridorController;
        GameFieldConsoleView gameFieldConsoleView;

        public void ListenTo(QuoridorControllerWithEvents quoridorController)
        {
            gameFieldConsoleView = new GameFieldConsoleView(quoridorController);

            this.quoridorController = quoridorController;
            quoridorController.GameStarted += FieldUpdated;
            quoridorController.GameStarted += GameStarted;
        }
        public void ListenTo(PlayerControllerWithEvents playerController)
        {
            playerController.FieldUpdated += FieldUpdated;
            playerController.WallPlaced += WallPlaced;
            playerController.PlayerWon += PlayerWon;
        }
        public void ListenTo(WeakAIControllerWithEvents weakAIController)
        {
            weakAIController.FieldUpdated += FieldUpdated;
            weakAIController.WallPlaced += WallPlaced;
            weakAIController.AIWon += PlayerWon;
        }

        void PlayerWon(INamed player)
        {
            System.Console.WriteLine($"Game is over!\n{player.Name} won! Congratulations!\n");
        }
        void GameStarted(IField field)
        {
            System.Console.WriteLine("Game is started!\nMake your first move. You can choose one of two");
            System.Console.WriteLine("1. Move your player up, left, right or down.\n   Example: 'move up'");
            System.Console.WriteLine("2. You can place the wall in some direction from the cell.\n   Example: " +
                "'place wall up 0 4' where 0 is horizontal and 4 vertical coordinate\n" +
                $"Each player has {quoridorController.QuoridorModel.CurrentPlayer.NumberOfWalls} " +
                $"walls, spend them wisely!\n");
        }
        void FieldUpdated(IField field)
        {
            gameFieldConsoleView.Output(field);
        }
        void WallPlaced(IPlayer player)
        {
            if (player.NumberOfWalls > 0)
                System.Console.WriteLine($"{player.Name}, you have {player.NumberOfWalls} walls left\n");
            else
                System.Console.WriteLine($"{player.Name}, you have no walls left, now all you need to do is reach the finish line\n");
        }
    }
}
