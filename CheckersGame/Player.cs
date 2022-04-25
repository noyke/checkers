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

        public Player(string i_Name, eType i_PlayerType, ePlayerNumber i_PlayerNumber)
        {
            r_Name = i_Name;
            r_Type = i_PlayerType;
            r_PlayerNumber = i_PlayerNumber;
        }

        public string Name
        {
            get
            {
                return r_Name;
            }
        }

        public eType Type
        {
            get
            {
                return r_Type;
            }
        }

        public ePlayerNumber PlayerNumber
        {
            get
            {
                return r_PlayerNumber;
            }
        }

        public int Score
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

        public void Reset()
        {
            m_GamePieces.Clear();
        }

        public List<GamePiece> GetGamePieceList()
        {
            return m_GamePieces;
        }

        public void UpdatePossibleRegularMoves(Board i_GameBoard)
        {
            foreach(GamePiece gamePiece in m_GamePieces)
            {
                gamePiece.UpdatePossibleRegularMoves(i_GameBoard);
            }
        }

        public void UpdatePossibleJumpMoves(Board i_GameBoard)
        {
            foreach (GamePiece gamePiece in m_GamePieces)
            {
                gamePiece.UpdatePossibleJumpMoves(i_GameBoard);
            }
        }

        public void AddGamePiece(GamePiece i_GamePiece)
        {
            m_GamePieces.Add(i_GamePiece);
        }

        public void RemoveGamePiece(GamePiece i_GamePiece)
        {
            m_GamePieces.Remove(i_GamePiece);
        }

        public bool CheckIsHaveAPossibleMove()
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

     
    }
   
}
