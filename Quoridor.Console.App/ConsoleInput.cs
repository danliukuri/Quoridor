using Quoridor.Controllers;
using Quoridor.Models;
using System;

namespace Quoridor.Console.App
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
            string command;
            ConsoleOutputForInput.OutputPvPModeTitle();
            quoridorController.StartGame();
            
            while (!quoridorController.QuoridorModel.IsGameEnded)
            {
                ConsoleOutputForInput.PlayerInputView(quoridorController.QuoridorModel.CurrentPlayer);
                command = System.Console.ReadLine();
                string[] splitCommand = command.Split(new char[0]);
                try
                {
                    switch (splitCommand[0].ToLower())
                    {
                        case "move":
                            {
                                Direction direction = splitCommand[1].ToLower() switch
                                {
                                    "up" => Direction.Up,
                                    "down" => Direction.Down,
                                    "left" => Direction.Left,
                                    "right" => Direction.Right,
                                    _ => throw new ArgumentException($"There is no such direction ({splitCommand[1]})")
                                };
                                if (splitCommand.Length == 3)
                                {
                                    Direction directionFromAnotherPlayer = splitCommand[2].ToLower() switch
                                    {
                                        "up" => Direction.Up,
                                        "down" => Direction.Down,
                                        "left" => Direction.Left,
                                        "right" => Direction.Right,
                                        _ => throw new ArgumentException($"There is no such direction ({splitCommand[2]})")
                                    };
                                    playerController.MakePlayerMove(direction, directionFromAnotherPlayer);
                                }
                                else
                                    playerController.MakePlayerMove(direction);
                            }
                            break;
                        case "place":
                            {
                                if (splitCommand[1].ToLower() != "wall")
                                {
                                    throw new ArgumentException($"\nUnknown command. Can't place '{splitCommand[1].ToLower()}'");
                                }

                                Direction direction = splitCommand[2].ToLower() switch
                                {
                                    "up" => Direction.Up,
                                    "down" => Direction.Down,
                                    "left" => Direction.Left,
                                    "right" => Direction.Right,
                                    _ => throw new ArgumentException($"There is no such direction ({splitCommand[2]})")
                                };
                                int w = int.Parse(splitCommand[3]) - 1;
                                int h = int.Parse(splitCommand[4]) - 1;
                                playerController.PlaceWall(direction, w, h);
                            }
                            break;
                        case "quit":
                            {
                                ConsoleOutputForInput.QuitTheGame(quoridorController.QuoridorModel.CurrentPlayer);
                                quoridorController.QuoridorModel.IsGameEnded = true;
                                break;
                            }
                        default:
                            ConsoleOutputForInput.UnknownCommand();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleOutputForInput.Exception(ex.Message);
                }
            }
        }

        public void StartProcessingPvAI(QuoridorController quoridorController, PlayerController playerController,
            WeakAIController weakAIController)
        {
            string command;
            ConsoleOutputForInput.OutputPvAIModeTitle();
            quoridorController.StartGame();

            bool isPlayerTurn = true;
            while (!quoridorController.QuoridorModel.IsGameEnded)
            { 
                if (isPlayerTurn)
                {
                    ConsoleOutputForInput.PlayerInputView(quoridorController.QuoridorModel.CurrentPlayer);
                    command = System.Console.ReadLine();
                    string[] splitCommand = command.Split(new char[0]);
                    try
                    {
                        switch (splitCommand[0].ToLower())
                        {
                            case "move":
                                {
                                    Direction direction = splitCommand[1].ToLower() switch
                                    {
                                        "up" => Direction.Up,
                                        "down" => Direction.Down,
                                        "left" => Direction.Left,
                                        "right" => Direction.Right,
                                        _ => throw new ArgumentException($"There is no such direction ({splitCommand[1]})")
                                    };
                                    if (splitCommand.Length == 3)
                                    {
                                        Direction directionFromAnotherPlayer = splitCommand[2].ToLower() switch
                                        {
                                            "up" => Direction.Up,
                                            "down" => Direction.Down,
                                            "left" => Direction.Left,
                                            "right" => Direction.Right,
                                            _ => throw new ArgumentException($"There is no such direction ({splitCommand[2]})")
                                        };
                                        playerController.MakePlayerMove(direction, directionFromAnotherPlayer);
                                    }
                                    else
                                        playerController.MakePlayerMove(direction);
                                    isPlayerTurn = false;
                                }
                                break;
                            case "place":
                                {
                                    if (splitCommand[1].ToLower() != "wall")
                                    {
                                        throw new ArgumentException($"\nUnknown command. Can't place '{splitCommand[1].ToLower()}'");
                                    }

                                    Direction direction = splitCommand[2].ToLower() switch
                                    {
                                        "up" => Direction.Up,
                                        "down" => Direction.Down,
                                        "left" => Direction.Left,
                                        "right" => Direction.Right,
                                        _ => throw new ArgumentException($"There is no such direction ({splitCommand[2]})")
                                    };
                                    int w = int.Parse(splitCommand[3]) - 1;
                                    int h = int.Parse(splitCommand[4]) - 1;
                                    playerController.PlaceWall(direction, w, h);
                                }
                                break;
                            case "quit":
                                {
                                    ConsoleOutputForInput.QuitTheGame(quoridorController.QuoridorModel.CurrentPlayer);
                                    quoridorController.QuoridorModel.IsGameEnded = true;
                                    break;
                                }
                            default:
                                ConsoleOutputForInput.UnknownCommand();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ConsoleOutputForInput.Exception(ex.Message);
                    }
                }
                else
                {
                    ConsoleOutputForInput.AITurn(quoridorController.QuoridorModel.CurrentPlayer);
                    while(!isPlayerTurn)
                    {
                        try
                        {
                            weakAIController.MakeMove();
                            isPlayerTurn = true;
                        }
                        catch
                        {
                            ConsoleOutputForInput.AIMistake(quoridorController.QuoridorModel.CurrentPlayer);
                        }
                    }
                }
            }
        }
    }
}
