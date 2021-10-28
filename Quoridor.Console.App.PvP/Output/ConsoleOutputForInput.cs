using Quoridor.Models.Interfaces;

namespace Quoridor.Console.App.PvP.Output
{
    class ConsoleOutputForInput
    {
        public static void OutputGameTitle()
        {
            System.Console.WriteLine("Welcome to greatest Quoridor game!");
            System.Console.WriteLine("\nSelect type of game:");
            System.Console.WriteLine("1. Player versus player (pvp)");
            System.Console.WriteLine("2. Player versus artificial intelligence (pvai)");
            System.Console.WriteLine("\nType 'pvp' or 'pvai' to select\n");
        }
        public static void OutputPvPModeTitle()
        {
            System.Console.Clear();
            System.Console.WriteLine("Quoridor PvP mode\n");
        }
        public static void OutputPvAIModeTitle()
        {
            System.Console.Clear();
            System.Console.WriteLine("Quoridor PvAI mode\n");
        }

        public static void InputView()
        {
            System.Console.Write("> ");
        }
        public static void PlayerInputView(IPlayer player)
        {
            System.Console.Write(player.Name + " > ");
        }
        public static void AITurn(IPlayer player)
        {
            System.Console.WriteLine($"{player.Name} thinks how to move...");
            System.Threading.Thread.Sleep(1000);
        }
        public static void AIMistake(IPlayer player)
        {
            System.Console.WriteLine($"{player.Name} made a mistake and right now she will try to make a move again...");
        }

        public static void UnknownCommand()
        {
            System.Console.WriteLine($"\nUnknown command. Please try again...\n");
        }
        public static void Exception(string exceptionMassage)
        {
            System.Console.WriteLine($"\n{exceptionMassage}\nPlease try again...\n");
        }
        public static void QuitTheGame(IPlayer player)
        {
            System.Console.WriteLine($"\nPlayer {player.Mark} is out of the game. The game is over...\n");
        }
    }
}
