using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class Player
    {
        public const int k_MaxNameLength = 20;

        public enum ePlayerNumber
        {
            Player1,
            Player2
        }

        public enum eType
        {
            Human,
            Computer
        }

        private readonly string r_Name;
        private readonly eType r_Type;
        private readonly ePlayerNumber r_PlayerNumber;
        private List<GamePiece> m_GamePieces = new List<GamePiece>();
        private int m_Score = 0;

        internal Player(string i_Name, eType i_PlayerType, ePlayerNumber i_PlayerNumber)
        {
            r_Name = i_Name;
            r_Type = i_PlayerType;
            r_PlayerNumber = i_PlayerNumber;
        }

        internal string Name
        {
            get
            {
                return r_Name;
            }
        }

        internal eType Type
        {
            get
            {
                return r_Type;
            }
        }

        internal ePlayerNumber PlayerNumber
        {
            get
            {
                return r_PlayerNumber;
            }
        }

        internal int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        internal bool CheckIsStillHaveGamePieces()
        {
            return !m_GamePieces.Count.Equals(0);
        }

        internal void Reset()
        {
            m_GamePieces.Clear();
        }

        internal void UpdatePossibleRegularMoves(Board i_GameBoard)
        {
            foreach(GamePiece gamePiece in m_GamePieces)
            {
                gamePiece.UpdatePossibleRegularMoves(i_GameBoard);
            }
        }

        internal void UpdatePossibleJumpMoves(Board i_GameBoard)
        {
            foreach (GamePiece gamePiece in m_GamePieces)
            {
                gamePiece.UpdatePossibleJumpMoves(i_GameBoard);
            }
        }

        internal void AddGamePiece(GamePiece i_GamePiece)
        {
            m_GamePieces.Add(i_GamePiece);
        }

        internal void RemoveGamePiece(GamePiece i_GamePiece)
        {
            m_GamePieces.Remove(i_GamePiece);
        }

        internal bool CheckIsHaveAPossibleMove()
        {
            bool isHaveAPossibleMove = false;

            foreach(GamePiece gamePiece in m_GamePieces)
            {
                if(gamePiece.CheckIsHaveAPossibleMove())
                {
                    isHaveAPossibleMove = true;
                    break;
                }
            }

            return isHaveAPossibleMove;
        }

        internal bool CheckIsOwner(GamePiece i_GamePiece)
        {
            return this.r_PlayerNumber.Equals(i_GamePiece.Color == GamePiece.eColor.Black ? Player.ePlayerNumber.Player1 : Player.ePlayerNumber.Player2);
        }

        internal bool CheckIsHaveAPossibleJumpMove()
        {
            bool IsHaveAPossibleJumpMove = false;

            foreach (GamePiece gamePiece in m_GamePieces)
            {
                if(gamePiece.CheckIsHaveAPossibleJumpMove())
                {
                    IsHaveAPossibleJumpMove = true;
                    break;
                }
            }

            return IsHaveAPossibleJumpMove;
        }

        internal int GetAmountOfGamePiecesForScore()
        {
            int AmountOfGamePiecesForScore = 0;

            foreach (GamePiece gamePiece in m_GamePieces)
            {
                if(gamePiece.Type.Equals(GamePiece.eType.Man))
                {
                    AmountOfGamePiecesForScore++;
                }
                else
                {
                    AmountOfGamePiecesForScore += 4;
                }
            }

            return AmountOfGamePiecesForScore;
        }

        internal List<Move> GetPossibleRegularMoves()
        {
            List<Move> possibleRegularMoves = new List<Move>();

            foreach(GamePiece gamePiece in m_GamePieces)
            {
                possibleRegularMoves.AddRange(gamePiece.PossibleRegularMoves);
            }

            return possibleRegularMoves;
        }

        internal List<Move> GetPossibleJumpMoves()
        {
            List<Move> possibleJumpMoves = new List<Move>();

            foreach (GamePiece gamePiece in m_GamePieces)
            {
                possibleJumpMoves.AddRange(gamePiece.PossibleJumpMoves);
            }

            return possibleJumpMoves;
        }
    }
}