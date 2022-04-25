using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameLogic;

namespace ConsoleUI
{
    public class UserInterface
    {
        private Game m_Game = new Game();

        public void Run()
        {
            initGame();

            bool isGameOver = false; 

            while(!isGameOver)
            {
                bool isRoundOver = false;
                
                while (!isRoundOver)
                {
                    // printBoard(); // TODO
                    // roundOver = m_Game.CheckIsRoundOver(); // TODO

                    if (m_Game.CurrentPlayerType() == Player.eType.Human)
                    {
                        
                        if (!makeUserMove())
                        {
                            isRoundOver = true;
                        }

                    }
                    else
                    {
                        // m_Game.MakeComputerMove();
                    }

                   

                }

                
            }
        }

        private bool makeUserMove()
        {
            bool isValidMove = false , anotherJump = false; 
            string userMoveStr;
            Move userMove;
            Point lastLocation = new Point(-1, -1); // SNIR out of bounds KASTACH 

            do
            {
                userMoveStr = getMoveFromUser();

                if (isValidMove = checkIsQuitInput(userMoveStr))
                {
                    break;
                }
                else
                {
                    userMove = Move.Parse(userMoveStr);

                    if (anotherJump) // SNIR if it jump loops check if the user chose the right source (the last destination)
                    {
                        if (m_Game.IsContinueJump(lastLocation, userMove))
                        {
                            isValidMove = m_Game.MakeUserMoveAndUpdates(userMove, out anotherJump);
                        }
                    }

                    else
                    {
                        isValidMove = m_Game.MakeUserMoveAndUpdates(userMove, out anotherJump);
                    }

                    if(!isValidMove) 
                    { 
                        Console.WriteLine("The input you entered is invalid. Please try again.");
                    }

                    lastLocation = userMove.Destination;
                }
            } while (!(isValidMove) || anotherJump);


            return !checkIsQuitInput(userMoveStr);
        }

        private string getMoveFromUser()
        {
            string userInput;
            bool isValidInput;
            
            do
            {
                userInput = Console.ReadLine();

                isValidInput = checkIsQuitInput(userInput) || Move.IsValidMoveFormat(userInput);


                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            return userInput;
        }

        private bool checkIsQuitInput(string i_UserInput)
        {
            return String.Equals(i_UserInput, "q") || String.Equals(i_UserInput, "Q");
        }

        private void initGame()
        {
            string player1Name, player2Name;
            Player.eType player2Type;
            Board.eBoardSize boardSize;

            Console.WriteLine("Hello! Welcome to The Checkers game!");

            getInitInputs(out player1Name, out player2Name, out player2Type, out boardSize);
            m_Game.InitGame(player1Name, player2Name, player2Type, boardSize);
        }

        private void getInitInputs(out string o_Player1Name, out string o_Player2Name, out Player.eType o_Player2Type, out Board.eBoardSize o_BoardSize)
        {
            o_Player1Name = getUserName();
            o_BoardSize = getBoardSize();
            o_Player2Type = getOpponentType();
            o_Player2Name = o_Player2Type == Player.eType.Human ? getUserName() : "Computer";
        }

        private string getUserName()
        {
            string userInput;
            bool isValidInput;

            Console.WriteLine("Please enter your name (without any spaces, 20 characters max):");

            do
            {
                userInput = Console.ReadLine();

                isValidInput = userInput.Length <= Player.k_MaxNameLength && !userInput.Contains(' ');

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            return userInput;
        }

        private Board.eBoardSize getBoardSize()
        {
            string userInput;
            Board.eBoardSize boardSize;
            bool isValidInput;

            string msg = string.Format(
@"Please choose board size:
(1) {0}X{0}
(2) {1}X{1}
(3) {2}X{2}",
            (int)Board.eBoardSize.Small, (int)Board.eBoardSize.Medium, (int)Board.eBoardSize.Large);

            Console.WriteLine(msg);

            do
            {
                userInput = Console.ReadLine();

                isValidInput = String.Equals(userInput, "1") || String.Equals(userInput, "2") || String.Equals(userInput, "3");

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            if (String.Equals(userInput, "1"))
            {
                boardSize = Board.eBoardSize.Small;
            }
            else if (String.Equals(userInput, "2"))
            {
                boardSize = Board.eBoardSize.Medium;
            }
            else // String.Equals(userInput, "3")
            {
                boardSize = Board.eBoardSize.Large;
            }

            return boardSize;
        }

        private Player.eType getOpponentType()
        {
            string userInput;
            Player.eType opponentType;
            bool isValidInput;

            Console.WriteLine(
           @"Playing against:
(1) A friend
(2) The computer");

            do
            {
                userInput = Console.ReadLine();

                isValidInput = String.Equals(userInput, "1") || String.Equals(userInput, "2");

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            opponentType = String.Equals(userInput, "1") ? Player.eType.Human : Player.eType.Computer;

            return opponentType;
        }

        // public void PrintBoard(Board i_GameBoard)   // TODO
    }
}