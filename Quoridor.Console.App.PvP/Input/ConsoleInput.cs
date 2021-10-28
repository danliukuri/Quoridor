using Quoridor.Console.App.PvP.Output;
using Quoridor.Controllers;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;

namespace Quoridor.Console.App.PvP.Input
{
    class ConsoleInput
    {
        public void StartProcessing(QuoridorController quoridorPvPController, PlayerController playerPvPController,
            QuoridorController quoridorPvAIController, PlayerController playerPvAIController,
            WeakAIController weakAIController)
        {
            ConsoleOutputForInput.OutputGameTitle();
            bool finishProcessing = false; ;
            while (!finishProcessing)
            {
                if (quoridorPvPController.QuoridorModel.IsGameEnded || quoridorPvAIController.QuoridorModel.IsGameEnded)
                {
                    ConsoleOutputForInput.OutputGameTitle();
                    quoridorPvPController.QuoridorModel.Reset();
                    quoridorPvAIController.QuoridorModel.Reset();
                }

                ConsoleOutputForInput.InputView();
                string command = System.Console.ReadLine();
                var splitCommand = command.Split(new char[0]);
                switch (splitCommand[0].ToLower())
                {
                    case "pvp":
                        StartProcessingPvP(quoridorPvPController, playerPvPController);
                        break;
                    case "pvai":
                        StartProcessingPvAI(quoridorPvAIController, playerPvAIController, weakAIController);
                        break;
                    case "quit":
                        finishProcessing = true;
                        break;
                    default:
                        ConsoleOutputForInput.UnknownCommand();
                        break;
                }
            }
        }

        public void StartProcessingPvP(QuoridorController quoridorController, PlayerController playerController)
        {
            ConsoleOutputForInput.OutputPvPModeTitle();
            quoridorController.StartGame();

            while (!quoridorController.QuoridorModel.IsGameEnded)
            {
                EitherLeftOrRight<ValidationError, bool> result =
                    HandlePlayerTurn(quoridorController, playerController);
                if (result.IsLeft)
                    ConsoleOutputForInput.Exception(result.LeftOrDefault().Message);
            }
        }

        public void StartProcessingPvAI(QuoridorController quoridorController, PlayerController playerController,
            WeakAIController weakAIController)
        {
            ConsoleOutputForInput.OutputPvAIModeTitle();
            quoridorController.StartGame();

            bool isPlayerTurn = true;
            while (!quoridorController.QuoridorModel.IsGameEnded)
            {
                if (isPlayerTurn)
                {
                    EitherLeftOrRight<ValidationError, bool> result =
                        HandlePlayerTurn(quoridorController, playerController);
                    if (result.IsLeft)
                        ConsoleOutputForInput.Exception(result.LeftOrDefault().Message);
                    else
                        isPlayerTurn = result.RightOrDefault();
                }
                else
                {
                    ConsoleOutputForInput.AITurn(quoridorController.QuoridorModel.CurrentPlayer);
                    weakAIController.MakeMove();
                    isPlayerTurn = true;
                }
            }
        }

        EitherLeftOrRight<ValidationError, bool> HandlePlayerTurn
            (QuoridorController quoridorController, PlayerController playerController)
        {
            bool isPlayerTurn = true;
            ConsoleOutputForInput.PlayerInputView(quoridorController.QuoridorModel.CurrentPlayer);
            string command = System.Console.ReadLine();
            string[] splitCommand = command.Split(new char[0]);

            switch (splitCommand[0].ToLower())
            {
                case "move":
                    {
                        Direction direction;
                        EitherLeftOrRight<ValidationError, Direction> directionResult =
                            splitCommand[1].ParseToDirection();
                        if (directionResult.IsLeft)
                            return directionResult.LeftOrDefault();
                        else
                            direction = directionResult.RightOrDefault();

                        EitherLeftOrVoid<ValidationError> result;
                        if (splitCommand.Length == 3)
                        {
                            Direction directionFromAnotherPlayer;
                            EitherLeftOrRight<ValidationError, Direction> directionFromAnotherPlayerResult =
                                splitCommand[2].ParseToDirection();
                            if (directionResult.IsLeft)
                                return directionResult.LeftOrDefault();
                            else
                                directionFromAnotherPlayer = directionResult.RightOrDefault();

                            result = playerController.TryToMakePlayerMove(direction, directionFromAnotherPlayer);
                        }
                        else
                            result = playerController.TryToMakePlayerMove(direction);
                        if (result.IsLeft)
                            ConsoleOutputForInput.Exception(result.LeftOrDefault().Message);
                        else
                            isPlayerTurn = false;
                    }
                    break;
                case "place":
                    {
                        if (splitCommand[1].ToLower() != "wall")
                        {
                            return new ValidationError($"\nUnknown command. Can't place '{splitCommand[1].ToLower()}'");
                        }
                        EitherLeftOrRight<ValidationError, Direction> directionResult =
                            splitCommand[2].ParseToDirection();
                        if (directionResult.IsLeft)
                            return directionResult.LeftOrDefault();
                        else
                        {
                            Direction direction = directionResult.RightOrDefault();

                            int w;
                            int h;
                            EitherLeftOrRight<ValidationError, int> intResult = splitCommand[3].ParseToInt();
                            if (intResult.IsLeft)
                                return intResult.LeftOrDefault();
                            else
                                w = intResult.RightOrDefault() - 1;

                            intResult = splitCommand[4].ParseToInt();
                            if (intResult.IsLeft)
                                return intResult.LeftOrDefault();
                            else
                                h = intResult.RightOrDefault() - 1;


                            EitherLeftOrVoid<ValidationError> result =
                                playerController.TryToPlaceWall(direction, w, h);
                            if (result.IsLeft)
                                ConsoleOutputForInput.Exception(result.LeftOrDefault().Message);
                            else
                                isPlayerTurn = false;
                        }
                    }
                    break;
                case "quit":
                    {
                        ConsoleOutputForInput.QuitTheGame(quoridorController.QuoridorModel.CurrentPlayer);
                        quoridorController.QuoridorModel.IsGameEnded = true;
                    }
                    break;
                default:
                    ConsoleOutputForInput.UnknownCommand();
                    break;
            }
            return isPlayerTurn;
        }
    }
}
