using Quoridor.Console.App.PvP.Input;
using Quoridor.Console.App.PvP.Output;
using Quoridor.Controllers;
using Quoridor.Models;

namespace Quoridor.Console.App.PvP
{
    class Program
    {
        static void Main(string[] args)
        {
            // PvP
            QuoridorControllerWithEvents quoridorPvPController;
            {
                Field field = new Field();
                Player[] players = new Player[]
                {
                    new Player("Player A",'A'),
                    new Player("Player B",'B'),
                    //new Player("Player C",'C'),
                    //new Player("Player D",'D')
                };
                QuoridorModel quoridorModel = new QuoridorModel(players, field);
                quoridorPvPController = new QuoridorControllerWithEvents(quoridorModel);
            }
            PlayerControllerWithEvents playerPvPController = new PlayerControllerWithEvents(quoridorPvPController);
            ConsoleOutput outputPvP = new ConsoleOutput();
            outputPvP.ListenTo(quoridorPvPController);
            outputPvP.ListenTo(playerPvPController);

            // PvAI
            QuoridorControllerWithEvents quoridorPvAIController;
            {
                Field field = new Field();
                Player[] players = new Player[]
                {
                    new Player("Player A",'A'),
                    new Player("Juliett AI",'J')
                };
                QuoridorModel quoridorModel = new QuoridorModel(players, field);
                quoridorPvAIController = new QuoridorControllerWithEvents(quoridorModel);
            }
            PlayerControllerWithEvents playerPvAIController = new PlayerControllerWithEvents(quoridorPvAIController);
            WeakAIControllerWithEvents weakAIController = new WeakAIControllerWithEvents(quoridorPvAIController);
            ConsoleOutput outputPvAI = new ConsoleOutput();
            outputPvAI.ListenTo(quoridorPvAIController);
            outputPvAI.ListenTo(playerPvAIController);
            outputPvAI.ListenTo(weakAIController);

            var input = new ConsoleInput();
            input.StartProcessing(quoridorPvPController, playerPvPController,
                quoridorPvAIController, playerPvAIController, weakAIController);
        }
    }
}
