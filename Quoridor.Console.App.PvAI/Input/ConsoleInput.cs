using Quoridor.Controllers;
using Quoridor.Controllers.PlayerControllers;
using Quoridor.Controllers.PlayerControllers.AI;
using Quoridor.Models.General;
using Quoridor.Models.General.Either;

namespace Quoridor.Console.App.PvAI.Input
{
    class ConsoleInput
    {
        ConsoleCommandAdapter consoleCommandAdapter;

        public void StartProcessingPvAI(QuoridorController quoridorController,
            PlayerController playerController, AIPlayerController aiPlayerController,
            ConsoleCommandAdapter consoleCommandAdapter)
        {
            this.consoleCommandAdapter = consoleCommandAdapter;
            quoridorController.StartGame();

            bool isFirstPlayerTurn = IsFirstPlayerTurn();
            while (!quoridorController.QuoridorModel.IsGameEnded)
            {
                if (isFirstPlayerTurn)
                {
                    isFirstPlayerTurn = HandleConsoleInput(playerController);
                }
                else
                {
                    aiPlayerController.MakeMove();
                    isFirstPlayerTurn = true;
                }
            }
            if (quoridorController.QuoridorModel.IsGameEnded)
                quoridorController.QuoridorModel.Reset();
        }

        string ChooseAIPlayerColor()
        {
            string command;
            string[] splitCommand;
            do
            {
                command = System.Console.ReadLine();
                splitCommand = command.Split(new char[0]);
            }
            while (splitCommand[0].ToLower() != "white" && splitCommand[0].ToLower() != "black");

            return splitCommand[0].ToLower();
        }
        bool IsFirstPlayerTurn() => ChooseAIPlayerColor() == "black"; // White turn first

        bool HandleConsoleInput(PlayerController playerController)
        {
            bool isPlayerTurn = true;
            string command = System.Console.ReadLine();
            string[] splitCommand = command.Split(new char[0]);

            switch (splitCommand[0].ToLower())
            {
                case "move":
                    {
                        Direction direction = consoleCommandAdapter.GetMoveParameters(splitCommand[1]);

                        EitherLeftOrVoid<ValidationError> result =
                            playerController.TryToMakePlayerMove(direction);
                        if (!result.IsLeft)
                            isPlayerTurn = false;
                    }
                    break;
                case "jump":
                    {
                        EitherLeftOrRight<Direction, int> parameters =
                            consoleCommandAdapter.GetJumpParameters(splitCommand[1]);

                        if (parameters.IsLeft)
                        {
                            EitherLeftOrVoid<ValidationError> result =
                                    playerController.TryToMakePlayerMove(parameters.LeftOrDefault());
                            if (!result.IsLeft)
                                isPlayerTurn = false;
                        }
                        else
                        {
                            int cornerFieldsIndex = parameters.RightOrDefault();

                            EitherLeftOrVoid<ValidationError> result = playerController.TryToMakePlayerMove(
                                        cornerFieldsIndex < 2 ? Direction.Up : Direction.Down,
                                        cornerFieldsIndex % 2 == 0 ? Direction.Left : Direction.Right);
                            if (result.IsLeft)
                            {
                                result = playerController.TryToMakePlayerMove(
                                    cornerFieldsIndex % 2 == 0 ? Direction.Left : Direction.Right,
                                    cornerFieldsIndex < 2 ? Direction.Up : Direction.Down);
                            }
                            if (!result.IsLeft)
                                isPlayerTurn = false;
                        }
                    }
                    break;
                case "wall":
                    {
                        (Direction direction, int w, int h) = consoleCommandAdapter.GetWallPlaceParameters
                            (splitCommand[1]);

                        EitherLeftOrVoid<ValidationError> result =
                            playerController.TryToPlaceWall(direction, w, h);

                        if (!result.IsLeft)
                            isPlayerTurn = false;
                    }
                    break;
                default:
                    break;
            }
            return isPlayerTurn;
        }
    }
}