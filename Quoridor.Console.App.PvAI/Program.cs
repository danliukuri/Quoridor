using Quoridor.Console.App.PvAI.Input;
using Quoridor.Console.App.PvAI.Output;
using Quoridor.Controllers;
using Quoridor.Controllers.PlayerControllers;
using Quoridor.Controllers.PlayerControllers.AI.Strong;
using Quoridor.Controllers.PlayerControllers.AI.Strong.MinimaxAdapters;
using Quoridor.Models;

namespace Quoridor.Console.App.PvAI
{
    class Program
    {
        static void Main(string[] args)
        {
            // PvAI
            Field field = new Field();
            Player[] players = new Player[]
            {
                new Player("White Player",'W'),
                new Player("Black Player",'B')
            };
            QuoridorModel quoridorModel = new QuoridorModel(players, field);
            QuoridorController quoridorPvAIController = new QuoridorController(quoridorModel);

            PlayerController playerPvAIController = new PlayerControllerWithEvents(quoridorPvAIController);

            StrongAIPlayerControllerWithEvents minimaxPlayerController =
                new StrongAIPlayerControllerWithEvents(quoridorPvAIController, (minimaxStateAdapter, player) =>
                    MinimaxScoreFunction.CalculateForTwoPlayers((MinimaxStateAdapter)minimaxStateAdapter, player));
            ConsoleCommandAdapter consoleCommandAdapter = new ConsoleCommandAdapter(quoridorPvAIController);

            ConsoleOutput outputPvAI = new ConsoleOutput();
            outputPvAI.ListenTo(minimaxPlayerController, consoleCommandAdapter);

            var input = new ConsoleInput();
            input.StartProcessingPvAI(quoridorPvAIController, playerPvAIController,
                minimaxPlayerController, consoleCommandAdapter);
        }
    }
}
