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
                    printBoard();

                    if(!(isRoundOver = m_Game.CheckIsRoundOver())) // TODO
                    {
                        if (m_Game.CurrentPlayerType() == Player.eType.Human)
                        {
                            if (!makeUserMove())
                            {
                                isRoundOver = true;
                            }
                        }
                        else
                        {
                            if(!m_Game.MakeComputerMove())
                            {
                                isRoundOver = true;
                            }
                        }
                    }
                }

                displayRoundResult();

                if(!checkForRematch())
                {
                    isGameOver = true;
                }
                else
                {
                    // rematch
                }
            }
        }

        private bool makeUserMove()
        {
            bool isValidMove = false; 
            string userMoveStr;
            Move userMove;

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

                    isValidMove = m_Game.MakeUserMoveAndUpdates(userMove);
                    
                    if(!isValidMove) 
                    { 
                        Console.WriteLine("The input you entered is invalid. Please try again.");
                    }
                }
            } while (!isValidMove);


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

        private void printBoard()
        {
            char currGamePieceSign;
            char rowIndex = 'a';
            int boardSize = m_Game.GetBoardSize();

            printBoardColsIndexes(boardSize);
            printBoardRowsSeparator(boardSize);

            for(int row = 0; row < boardSize; row++, rowIndex++)
            {
                Console.Write(rowIndex.ToString());

                for(int col = 0; col < boardSize; col++)
                {
                    currGamePieceSign = getSignToPrint(row, col);

                    Console.Write("| " + currGamePieceSign.ToString() + " ");
                }

                Console.WriteLine("|");
                printBoardRowsSeparator(boardSize);
            }

            printLastMove(); // TODO
        }

        private void printBoardColsIndexes(int i_BoardSize)
        {
            char colIndex = 'A';

            Console.Write(' ');

            for(int col = 0; col < i_BoardSize; col++, colIndex++)
            {
                Console.Write("  " + colIndex.ToString() + " ");
            }

            Console.WriteLine();
        }

        private void printBoardRowsSeparator(int i_BoardSize)
        {
            Console.Write(' ');

            for(int col = 0; col < i_BoardSize; col++)
            {
                Console.Write("====");
            }

            Console.WriteLine('=');
        }

        private char getSignToPrint(int row, int col) // find a better name
        {
            char signToPrint;
            GamePiece.eColor? gamePieceColor = m_Game.GetGamePieceColor(int row, int col);

            if(gamePieceColor == null)
            {
                signToPrint = ' ';
            }
            else if(gamePieceColor == GamePiece.eColor.Black)
            {
                signToPrint = m_Game.CheckIsGamePieceKing(int row, int col) ? 'K' : 'X';
            }
            else
            {
                signToPrint = m_Game.CheckIsGamePieceKing(int row, int col) ? 'U' : 'O';
            }

            return signToPrint;
        }
    }
}