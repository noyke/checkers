using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class Game
    {
        public enum eRoundResult
        {
            Draw,
            Player1Win,
            Player2Win
        }

        private Board m_GameBoard;
        private Player[] m_Players = new Player[2];
        private int m_CurrentPlayerIndex;
        private Move? m_LastMove;

        public Move? LastMove
        {
            get
            {
                return m_LastMove;
            }
        }

        public void InitGame(string i_Player1Name, string i_Player2Name, Player.eType i_Player2Type, Board.eBoardSize i_BoardSize)
        {
            m_GameBoard = new Board(i_BoardSize);
            m_Players[0] = new Player(i_Player1Name, Player.eType.Human, Player.ePlayerNumber.Player1);
            m_Players[1] = new Player(i_Player2Name, i_Player2Type, Player.ePlayerNumber.Player2);

            SetNewRound();
        }

        public void SetNewRound()
        {
            foreach (Player player in m_Players)
            {
                player.Reset();
            }

            m_GameBoard.SetBoardAndGamePieces(m_Players);

            foreach(Player player in m_Players)
            {
                player.UpdatePossibleRegularMoves(m_GameBoard);
            }

            m_CurrentPlayerIndex = 0;
            m_LastMove = null;
        }

        private Player getCurrentPlayer()
        {
            return m_Players[m_CurrentPlayerIndex];
        }

        private Player getNextPlayer()
        {
            return m_Players[(m_CurrentPlayerIndex + 1) % 2];
        }

        public Player.eType CurrentPlayerType()
        {
            return getCurrentPlayer().Type;
        }

        public int GetBoardSize()
        {
            return m_GameBoard.Size;
        }

        public GamePiece.eColor? GetGamePieceColor(Point i_Location)
        {
            GamePiece.eColor? gamePieceColor;

            if(m_GameBoard[i_Location] == null)
            {
                gamePieceColor = null;
            }
            else
            {
                gamePieceColor = m_GameBoard[i_Location].Color;
            }

            return gamePieceColor;
        }

        public bool CheckIsGamePieceKing(Point i_Location)
        {
            return m_GameBoard[i_Location].Type == GamePiece.eType.King;
        }

        public string GetLastMovePlayerName()
        {
            return getLastMovePlayer().Name;
        }

        public Player.ePlayerNumber GetLastMovePlayerNumber()
        {
            return getLastMovePlayer().PlayerNumber;
        }

        public string GetCurrentPlayerName()
        {
            return getCurrentPlayer().Name;
        }

        public string GetWinnerName(Game.eRoundResult i_RoundResult)
        {
            return getWinner(i_RoundResult).Name;
        }

        public int GetWinnerScore(Game.eRoundResult i_RoundResult)
        {
            return getWinner(i_RoundResult).Score;
        }

        public string GetLoserName(Game.eRoundResult i_RoundResult)
        {
            return getLoser(i_RoundResult).Name;
        }

        public int GetLoserScore(Game.eRoundResult i_RoundResult)
        {
            return getLoser(i_RoundResult).Score;
        }

        public Player.ePlayerNumber GetCurrentPlayerNumber()
        {
            return getCurrentPlayer().PlayerNumber;
        }

        private void swapCurrentPlayer()
        {
            m_CurrentPlayerIndex = (m_CurrentPlayerIndex + 1) % 2;
        }

        public void MakeComputerMove()
        {
            Move computerMove = getComputerMove();

            makeMove(computerMove);
        }

        private Move getComputerMove()
        {
            bool isValidMove;
            Move computerMove;
            List<Move> possibleMoves;
            Random random = new Random();

            if (getCurrentPlayer().CheckIsHaveAPossibleJumpMove())
            {
                possibleMoves = getCurrentPlayer().GetPossibleJumpMoves();
            }
            else
            {
                possibleMoves = getCurrentPlayer().GetPossibleRegularMoves();
            }

            do
            {
                computerMove = possibleMoves[random.Next(possibleMoves.Count)];
                isValidMove = checkIsValidMove(computerMove);
            } while (!isValidMove);

            return computerMove;
        }

        public bool MakeUserMove(Move i_UserMove)
        {
            bool isValidMove;

            if (isValidMove = checkIsValidMove(i_UserMove))
            {
                makeMove(i_UserMove);
            }

            return isValidMove;
        }

        private bool checkIsValidMove(Move i_Move)
        {
            bool isValidMove = true;
            GamePiece currGamePiece;

            if (m_GameBoard.CheckIsLocationInBound(i_Move.Source) && m_GameBoard.CheckIsLocationInBound(i_Move.Destination))
            {
                if (m_GameBoard[i_Move.Source] != null)
                {
                    currGamePiece = m_GameBoard[i_Move.Source];

                    if (getCurrentPlayer().CheckIsOwner(currGamePiece))
                    {
                        if (currGamePiece.CheckIsMoveInPossibleMovesLists(i_Move))
                        {
                            if (getCurrentPlayer().CheckIsHaveAPossibleJumpMove())
                            {
                                if (i_Move.CheckIsJumpMove())
                                {
                                    if (getCurrentPlayer().CheckIsOwner(getLastMovedGamePiece()) && !currGamePiece.Equals(getLastMovedGamePiece()))
                                    {
                                        isValidMove = false;
                                    }
                                }
                                else
                                {
                                    isValidMove = false;
                                }
                            }
                        }
                        else
                        {
                            isValidMove = false;
                        }
                    }
                    else
                    {
                        isValidMove = false;
                    }
                }
                else
                {
                    isValidMove = false;
                }
            }
            else
            {
                isValidMove = false;
            }

            return isValidMove;
        }

        private void makeMove(Move i_Move)
        {
            GamePiece currGamePiece = m_GameBoard[i_Move.Source];

            currGamePiece.Location = i_Move.Destination;
            m_GameBoard[i_Move.Destination] = currGamePiece;
            m_GameBoard[i_Move.Source] = null;

            crownIfNeeded(currGamePiece);

            if (i_Move.CheckIsJumpMove())
            {
                makeCapture(i_Move);
            }

            updatePlayersPossibleMoves();
            m_LastMove = i_Move;

            if(!(i_Move.CheckIsJumpMove() && currGamePiece.CheckIsHaveAPossibleJumpMove()))
            {
                swapCurrentPlayer();
            }
        }

        private void makeCapture(Move i_Move)
        {
            GamePiece capturedGamePiece;

            if (i_Move.Source.X < i_Move.Destination.X)
            {
                if (i_Move.Source.Y < i_Move.Destination.Y)
                {
                    capturedGamePiece = m_GameBoard[i_Move.Source.Y + 1, i_Move.Source.X + 1];
                }
                else
                {
                    capturedGamePiece = m_GameBoard[i_Move.Source.Y - 1, i_Move.Source.X + 1];
                }
            }
            else
            {
                if (i_Move.Source.Y < i_Move.Destination.Y)
                {
                    capturedGamePiece = m_GameBoard[i_Move.Source.Y + 1, i_Move.Source.X - 1];
                }
                else
                {
                    capturedGamePiece = m_GameBoard[i_Move.Source.Y - 1, i_Move.Source.X - 1];
                }
            }

            m_GameBoard[capturedGamePiece.Location] = null;
            getNextPlayer().RemoveGamePiece(capturedGamePiece);
        }

        public bool CheckIsRoundOver()
        {
            bool isRoundOver = !getCurrentPlayer().CheckIsHaveAPossibleMove();

            foreach(Player player in m_Players)
            {
                if(isRoundOver)
                {
                    break;
                }

                isRoundOver = !player.CheckIsStillHaveGamePieces();
            }

            return isRoundOver;
        }

        public eRoundResult RoundResult()
        {
            eRoundResult roundResult = eRoundResult.Draw;

            foreach(Player player in m_Players)
            {
                if(!roundResult.Equals(eRoundResult.Draw))
                {
                    break;
                }
                
                if(!player.CheckIsStillHaveGamePieces())
                {
                    roundResult = player.PlayerNumber.Equals(Player.ePlayerNumber.Player1) ? eRoundResult.Player2Win : eRoundResult.Player1Win;
                }
            }

            if(roundResult.Equals(eRoundResult.Draw))
            {
                if (getCurrentPlayer().CheckIsHaveAPossibleMove())
                {
                    roundResult = getCurrentPlayer().PlayerNumber.Equals(Player.ePlayerNumber.Player1) ? eRoundResult.Player2Win : eRoundResult.Player1Win;
                }
                else
                {
                    if (getNextPlayer().CheckIsHaveAPossibleMove())
                    {
                        roundResult = getCurrentPlayer().PlayerNumber.Equals(Player.ePlayerNumber.Player1) ? eRoundResult.Player2Win : eRoundResult.Player1Win;
                    }
                    else
                    {
                        roundResult = eRoundResult.Draw;
                    }
                }
            }

            if (roundResult != eRoundResult.Draw)
            {
                calculateWinnerScore(roundResult);
            }

            return roundResult;
        }

        private void calculateWinnerScore(eRoundResult i_RoundResult)
        {
            Player winner = getWinner(i_RoundResult);
            Player loser = getLoser(i_RoundResult);

            winner.Score += Math.Max(Math.Abs(winner.GetAmountOfGamePiecesForScore() - loser.GetAmountOfGamePiecesForScore()), 1);
        }

        private Player getWinner(eRoundResult i_RoundResult)
        {
            return i_RoundResult == eRoundResult.Player1Win ? m_Players[0] : m_Players[1];
        }

        private Player getLoser(eRoundResult i_RoundResult)
        {
            return i_RoundResult == eRoundResult.Player1Win ? m_Players[1] : m_Players[0];
        }

        private void updatePlayersPossibleMoves()
        {
            foreach(Player player in m_Players)
            {
                player.UpdatePossibleRegularMoves(m_GameBoard);
                player.UpdatePossibleJumpMoves(m_GameBoard);
            }
        }

        private void crownIfNeeded(GamePiece i_GamePiece)
        {
            if (i_GamePiece.Type.Equals(GamePiece.eType.Man))
            {
                if(i_GamePiece.Location.Y.Equals(i_GamePiece.Color.Equals(GamePiece.eColor.Black) ? 0 : m_GameBoard.Size - 1))
                {
                    i_GamePiece.Type = GamePiece.eType.King;
                }
            }
        }

        private GamePiece getLastMovedGamePiece()
        {
            return m_GameBoard[m_LastMove.Value.Destination];
        }

        private Player getLastMovePlayer()
        {
            return getLastMovedGamePiece().Color == GamePiece.eColor.Black ? m_Players[0] : m_Players[1];
        }
    }
}