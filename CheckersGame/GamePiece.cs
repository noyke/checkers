using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class GamePiece
    {
        public enum eColor
        {
            Black,
            White
        }

        public enum eType
        {
            Man,
            King
        }

        private Point m_Location;
        private readonly eColor r_Color;
        private eType m_Type = eType.Man;
        private List<Move> m_PossibleRegularMoves = new List<Move>();
        private List<Move> m_PossibleJumpMoves = new List<Move>();

        internal GamePiece(Point i_Location, eColor i_Color)
        {
            m_Location = i_Location;
            r_Color = i_Color;
        }

        internal Point Location
        {
            get
            {
                return m_Location;
            }
            set
            {
                m_Location = value;
            }
        }

        internal eColor Color
        {
            get
            {
                return r_Color;
            }
        }

        internal eType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        internal List<Move> PossibleRegularMoves
        {
            get
            {
                return m_PossibleRegularMoves;
            }
        }

        internal List<Move> PossibleJumpMoves
        {
            get
            {
                return m_PossibleJumpMoves;
            }
        }

        internal void UpdatePossibleRegularMoves(Board i_GameBoard)
        {
            m_PossibleRegularMoves.Clear();

            this.tryAddPossibleRegularMove(i_GameBoard, this.NeighborForwardRightLocation());
            this.tryAddPossibleRegularMove(i_GameBoard, this.NeighborForwardLeftLocation());

            if(this.m_Type.Equals(eType.King))
            {
                this.tryAddPossibleRegularMove(i_GameBoard, this.NeighborBackwardRightLocation());
                this.tryAddPossibleRegularMove(i_GameBoard, this.NeighborBackwardLeftLocation());
            }
        }

        private void tryAddPossibleRegularMove(Board i_GameBoard, Point i_NeighborLocation)
        {
            if (i_GameBoard.CheckIsLocationInBound(i_NeighborLocation) && i_GameBoard[i_NeighborLocation] == null)
            {
                this.AddPossibleRegularMove(new Move(this.Location, i_NeighborLocation));
            }
        }

        internal void UpdatePossibleJumpMoves(Board i_GameBoard)
        {
            m_PossibleJumpMoves.Clear();

            this.tryAddPossibleJumpMove(i_GameBoard, this.NeighborForwardRightLocation());
            this.tryAddPossibleJumpMove(i_GameBoard, this.NeighborForwardLeftLocation());

            if(this.m_Type.Equals(eType.King))
            {
                this.tryAddPossibleJumpMove(i_GameBoard, this.NeighborBackwardRightLocation());
                this.tryAddPossibleJumpMove(i_GameBoard, this.NeighborBackwardLeftLocation());
            }
        }

        private void tryAddPossibleJumpMove(Board i_GameBoard, Point i_NeighborLocation)
        {
            if (i_GameBoard.CheckIsLocationInBound(i_NeighborLocation) && this.isOpponent(i_GameBoard[i_NeighborLocation]))
            {
                Point targetLocation;

                if (this.NeighborForwardRightLocation().Equals(i_NeighborLocation))
                {
                    targetLocation = i_GameBoard[i_NeighborLocation].NeighborBackwardRightLocation();
                }
                else if(this.NeighborForwardLeftLocation().Equals(i_NeighborLocation))
                {
                    targetLocation = i_GameBoard[i_NeighborLocation].NeighborBackwardLeftLocation();
                }
                else if(this.NeighborBackwardRightLocation().Equals(i_NeighborLocation))
                {
                    targetLocation = i_GameBoard[i_NeighborLocation].NeighborForwardRightLocation();
                }
                else // this.NeighborBackwardLeftLocation().Equals(i_NeighborLocation)
                {
                    targetLocation = i_GameBoard[i_NeighborLocation].NeighborForwardLeftLocation();
                }

                if (i_GameBoard.CheckIsLocationInBound(targetLocation) && i_GameBoard[targetLocation] == null)
                {
                    this.AddPossibleJumpMove(new Move(this.Location, targetLocation));
                }
            }
        }

        internal void AddPossibleRegularMove(Move i_Move)
        {
            m_PossibleRegularMoves.Add(i_Move);
        }

        internal void AddPossibleJumpMove(Move i_Move)
        {
            m_PossibleJumpMoves.Add(i_Move);
        }

        private bool isOpponent(GamePiece i_OtherGamePiece)
        {
            return i_OtherGamePiece != null && this.Color != i_OtherGamePiece.Color;
        }

        internal Point NeighborForwardRightLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;
            
            return new Point(this.Location.Y - offset, this.Location.X + 1);
        }

        internal Point NeighborForwardLeftLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;

            return new Point(this.Location.Y - offset, this.Location.X - 1);
        }

        internal Point NeighborBackwardRightLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;

            return new Point(this.Location.Y + offset, this.Location.X + 1);
        }

        internal Point NeighborBackwardLeftLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;

            return new Point(this.Location.Y + offset, this.Location.X - 1);
        }

        internal bool CheckIsHaveAPossibleMove()
        {
            return checkIsHaveAPossibleRegularMove() || CheckIsHaveAPossibleJumpMove();
        }

        private bool checkIsHaveAPossibleRegularMove()
        {
            return m_PossibleRegularMoves.Count != 0;
        }

        internal bool CheckIsHaveAPossibleJumpMove()
        {
            return m_PossibleJumpMoves.Count != 0;
        }

        internal bool CheckIsMoveInPossibleMovesLists(Move i_Move)
        {
            return (i_Move.CheckIsRegularMove() && m_PossibleRegularMoves.Contains(i_Move) || (i_Move.CheckIsJumpMove() && m_PossibleJumpMoves.Contains(i_Move)));
        }
    }
}