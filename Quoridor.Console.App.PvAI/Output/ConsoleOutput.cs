using Quoridor.Controllers.PlayerControllers.AI.Strong;
using Quoridor.Controllers.UtilityModels;

namespace Quoridor.Console.App.PvAI.Output
{
    class ConsoleOutput
    {
        ConsoleCommandAdapter consoleCommandAdapter;

        public void ListenTo(StrongAIPlayerControllerWithEvents minimaxAIController,
            ConsoleCommandAdapter consoleCommandAdapter)
        {
            this.consoleCommandAdapter = consoleCommandAdapter;
            minimaxAIController.BestMoveFound += BestMoveFound;
        }

        void BestMoveFound(Move move)
        {
            string[] splitCommand = consoleCommandAdapter.GetMoveCommand(move);
            System.Console.Write(splitCommand[0] + " ");
            for (int i = 1; i < splitCommand.Length; i++)
                System.Console.Write(splitCommand[i]);
            System.Console.WriteLine();
        }
    }
}