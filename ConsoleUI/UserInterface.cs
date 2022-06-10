using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameLogic;
using System.IO;

namespace ConsoleUI
{
    public class UserInterface
    {
        private const int k_ComputerMoveDelay = 2000;

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

                    if(!(isRoundOver = m_Game.CheckIsRoundOver()))
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
                            makeComputerMove();
                        }
                    }
                }

                displayRoundResult();

                if(checkIsUserWantRematch())
                {
                    m_Game.SetNewRound();
                }
                else
                {
                    isGameOver = true;
                }
            }

            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("Thanks for playing! See you next time :)");
        }

        private bool makeUserMove()
        {
            bool isValidMove; 
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

                    isValidMove = m_Game.MakeUserMove(userMove);
                    
                    if(!isValidMove) 
                    { 
                        Console.WriteLine("The input you entered is invalid. Please try again.");
                    }
                }
            } while (!isValidMove);


            return !checkIsQuitInput(userMoveStr);
        }

        private void makeComputerMove()
        {
            Console.Write("thinking...");
            System.Threading.Thread.Sleep(k_ComputerMoveDelay);
            m_Game.MakeComputerMove();
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
            return i_UserInput.ToUpper().Equals("Q");
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

                isValidInput =  0 < userInput.Length && userInput.Length <= Player.k_MaxNameLength && !userInput.Contains(' ');

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            Ex02.ConsoleUtils.Screen.Clear();

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

                isValidInput = userInput.Equals("1") || userInput.Equals("2") || userInput.Equals("3");

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            Ex02.ConsoleUtils.Screen.Clear();

            if (userInput.Equals("1"))
            {
                boardSize = Board.eBoardSize.Small;
            }
            else if (userInput.Equals("2"))
            {
                boardSize = Board.eBoardSize.Medium;
            }
            else // userInput.Equals("3")
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

                isValidInput = userInput.Equals("1") || userInput.Equals("2");

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            Ex02.ConsoleUtils.Screen.Clear();
            opponentType = userInput.Equals("1") ? Player.eType.Human : Player.eType.Computer;

            return opponentType;
        }

        private void printBoard()
        {
            char currGamePieceSign;
            char rowIndex = 'a';
            int boardSize = m_Game.GetBoardSize();

            Ex02.ConsoleUtils.Screen.Clear();
            printBoardColsIndexes(boardSize);
            printBoardRowsSeparator(boardSize);

            for(int row = 0; row < boardSize; row++, rowIndex++)
            {
                Console.Write(rowIndex.ToString());

                for(int col = 0; col < boardSize; col++)
                {
                    currGamePieceSign = getSignToPrint(new Point(row, col));

                    Console.Write("| " + currGamePieceSign.ToString() + " ");
                }

                Console.WriteLine("|");
                printBoardRowsSeparator(boardSize);
            }

            printMovesDescription();
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

        private char getSignToPrint(Point i_Location)
        {
            char signToPrint;
            GamePiece.eColor? gamePieceColor = m_Game.GetGamePieceColor(i_Location);

            if(gamePieceColor == null)
            {
                signToPrint = ' ';
            }
            else if(gamePieceColor == GamePiece.eColor.Black)
            {
                signToPrint = m_Game.CheckIsGamePieceKing(i_Location) ? 'K' : 'X';
            }
            else
            {
                signToPrint = m_Game.CheckIsGamePieceKing(i_Location) ? 'U' : 'O';
            }

            return signToPrint;
        }

        private void printMovesDescription()
        {
            Move? lastMove = m_Game.LastMove;

            if (lastMove.HasValue)
            {
                string lastMovePlayerName = m_Game.GetLastMovePlayerName();
                char lastMovePlayerSign = m_Game.GetLastMovePlayerNumber().Equals(Player.ePlayerNumber.Player1) ? 'X' : 'O';
                string lastMoveDescription = string.Format("{0}'s move was ({1}): {2}", lastMovePlayerName, lastMovePlayerSign, lastMove.Value.ToString());
                Console.WriteLine(lastMoveDescription);
            }

            string currentPlayerName = m_Game.GetCurrentPlayerName();
            char currentPlayerSign = m_Game.GetCurrentPlayerNumber().Equals(Player.ePlayerNumber.Player1) ? 'X' : 'O';
            string currentMoveDescription = string.Format("{0}'s turn ({1}): ", currentPlayerName, currentPlayerSign);
            Console.Write(currentMoveDescription);
        }

        private void displayRoundResult()
        {
            Ex02.ConsoleUtils.Screen.Clear();

            StringBuilder roundResultMsgSB = new StringBuilder();
            Game.eRoundResult roundResult= m_Game.RoundResult();
            
            if(roundResult == Game.eRoundResult.Draw)
            {
                roundResultMsgSB.Append("It's a draw!");
            }
            else
            {
                roundResultMsgSB.AppendFormat("{0} wins!", m_Game.GetWinnerName(roundResult));
            }

            roundResultMsgSB.Append(Environment.NewLine + Environment.NewLine);
            roundResultMsgSB.Append("Score:" + Environment.NewLine);
            roundResultMsgSB.AppendFormat("{0}: {1}{2}", m_Game.GetWinnerName(roundResult), m_Game.GetWinnerScore(roundResult), Environment.NewLine);
            roundResultMsgSB.AppendFormat("{0}: {1}{2}", m_Game.GetLoserName(roundResult), m_Game.GetLoserScore(roundResult), Environment.NewLine);

            Console.WriteLine(roundResultMsgSB.ToString());
        }

        private bool checkIsUserWantRematch()
        {
            string userInput;
            bool isValidInput;

            Console.WriteLine("Do you want a rematch? (Y / N)");

            do
            {
                userInput = Console.ReadLine();

                isValidInput = userInput.ToUpper().Equals("Y") || userInput.ToUpper().Equals("N");

                if (!isValidInput)
                {
                    Console.WriteLine("The input you entered is invalid. Please try again.");
                }

            } while (!isValidInput);

            return userInput.Equals("Y") || userInput.Equals("y");
        }
    }
}