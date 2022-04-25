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

        public GamePiece(Point i_Location, eColor i_Color)
        {
            m_Location = i_Location;
            r_Color = i_Color;
        }

        public Point Location
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

        public eColor Color
        {
            get
            {
                return r_Color;
            }
        }

        public eType Type
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

        public List<Move> getPossibleRegularMovesList()
        {
            return m_PossibleRegularMoves;
        }

        public List<Move> getPossibleJumpMovesList()
        {
            return m_PossibleJumpMoves;
        }

        public void UpdatePossibleRegularMoves(Board i_GameBoard)    // TODO - try to modulate
        {
            Point location;
            Move move;

            if(!(this.m_PossibleRegularMoves == null))
            {
                m_PossibleRegularMoves.Clear();
            }

            location = this.NeighborForwardRightLocation();
            if(i_GameBoard.CheckIsLocationInBound(location) && i_GameBoard[location] == null)
            {
                move = new Move(this.Location, location);
                m_PossibleRegularMoves.Add(move);
            }

            location = this.NeighborForwardLeftLocation();
            if (i_GameBoard.CheckIsLocationInBound(location) && i_GameBoard[location] == null)
            {
                move = new Move(this.Location, location);
                m_PossibleRegularMoves.Add(move);
            }

            if(this.m_Type.Equals(eType.King))
            {
                location = this.NeighborBackwardRightLocation();
                if (i_GameBoard.CheckIsLocationInBound(location) && i_GameBoard[location] == null)
                {
                    move = new Move(this.Location, location);
                    m_PossibleRegularMoves.Add(move);
                }

                location = this.NeighborBackwardLeftLocation();
                if (i_GameBoard.CheckIsLocationInBound(location) && i_GameBoard[location] == null)
                {
                    move = new Move(this.Location, location);
                    m_PossibleRegularMoves.Add(move);
                }
            }
        }

        public void UpdatePossibleJumpMoves(Board i_GameBoard)
        {
            Point neighborLocation, targetLocation;
            Move move;

            neighborLocation = this.NeighborForwardRightLocation();
            if(IsAlly(i_GameBoard[neighborLocation]))
            {
                targetLocation = i_GameBoard[neighborLocation].NeighborBackwardRightLocation();
                if (targetLocation.Equals(null))
                {
                    move = new Move(this.Location, targetLocation);
                    this.AddPossibleJumpMove(move);
                }
            }

            neighborLocation = this.NeighborForwardLeftLocation();
            if (IsAlly(i_GameBoard[neighborLocation]))
            {
                targetLocation = i_GameBoard[neighborLocation].NeighborBackwardLeftLocation();
                if (targetLocation.Equals(null))
                {
                    move = new Move(this.Location, targetLocation);
                    this.AddPossibleJumpMove(move);
                }
            }

            if(this.m_Type.Equals(eType.King))
            {

                neighborLocation = this.NeighborBackwardRightLocation();
                if (IsAlly(i_GameBoard[neighborLocation]))
                {
                    targetLocation = i_GameBoard[neighborLocation].NeighborForwardRightLocation();
                    if (targetLocation.Equals(null))
                    {
                        move = new Move(this.Location, targetLocation);
                        this.AddPossibleJumpMove(move);
                    }
                }

                neighborLocation = this.NeighborBackwardLeftLocation();
                if (IsAlly(i_GameBoard[neighborLocation]))
                {
                    targetLocation = i_GameBoard[neighborLocation].NeighborForwardLeftLocation();
                    if (targetLocation.Equals(null))
                    {
                        move = new Move(this.Location, targetLocation);
                        this.AddPossibleJumpMove(move);
                    }
                }

            }
        }

        public void AddPossibleRegularMove(Move i_Move)
        {
            m_PossibleRegularMoves.Add(i_Move);
        }

        public void AddPossibleJumpMove(Move i_Move)
        {
            m_PossibleRegularMoves.Add(i_Move);
        }

        public bool IsAlly(GamePiece i_OtherGamePiece)
        {
            return i_OtherGamePiece != null && this.Color == i_OtherGamePiece.Color;
        }

        public bool isOpponent(GamePiece i_OtherGamePiece)
        {
            return i_OtherGamePiece != null && this.Color != i_OtherGamePiece.Color;
        }

        // TODO - try to modulate neighbors methods
        public Point NeighborForwardRightLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;
            
            return new Point(this.Location.X - offset, this.Location.Y + 1);
        }

        public Point NeighborForwardLeftLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;

            return new Point(this.Location.X - offset, this.Location.Y - 1);
        }

        public Point NeighborBackwardRightLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;

            return new Point(this.Location.X + offset, this.Location.Y + 1);
        }

        public Point NeighborBackwardLeftLocation()
        {
            int offset = this.Color == GamePiece.eColor.Black ? 1 : -1;

            return new Point(this.Location.X + offset, this.Location.Y - 1);
        }

        public bool CheckIsHaveAPossibleMove()
        {
            return checkIsHaveAPossibleRegularMove() || CheckIsHaveAPossibleJumpMove();
        }

        private bool checkIsHaveAPossibleRegularMove()
        {
            return m_PossibleRegularMoves.Count != 0;
        }

        public bool CheckIsHaveAPossibleJumpMove()
        {
            return m_PossibleJumpMoves.Count != 0;
        }

        public void UpdatePossibleRegular(Board i_GameBoard)
        {
            this.m_PossibleRegularMoves.Clear();

            if (i_GameBoard[this.NeighborForwardRightLocation()] == null)
            {
                Move possibleMove = new Move(this.Location, this.NeighborForwardRightLocation());
                this.AddPossibleRegularMove(possibleMove);
            }

            if (i_GameBoard[this.NeighborForwardLeftLocation()] == null)
            {
                Move possibleMove = new Move(this.Location, this.NeighborForwardLeftLocation());
                this.AddPossibleRegularMove(possibleMove);
            }

            if(this.Type == eType.King)
            {
                if (i_GameBoard[this.NeighborBackwardRightLocation()] == null)
                {
                    Move possibleMove = new Move(this.Location, this.NeighborBackwardRightLocation());
                    this.AddPossibleRegularMove(possibleMove);
                }

                if (i_GameBoard[this.NeighborBackwardLeftLocation()] == null)
                {
                    Move possibleMove = new Move(this.Location, this.NeighborBackwardLeftLocation());
                    this.AddPossibleRegularMove(possibleMove);
                }
            }

        }

        public bool IsMoveInPossibleRegularMoves(Move i_UserMove)
        {
            bool isMoveExists = false;

            foreach (Move move in m_PossibleRegularMoves)
            {
                if (move.Equals(i_UserMove))
                {
                    isMoveExists = true;
                    break;
                }
            }

            return isMoveExists;
        }

        public bool IsMoveInPossibleJumpMoves(Move i_UserMove)
        {
            bool isMoveExists = false;

            foreach (Move move in m_PossibleJumpMoves)
            {
                if(move.Equals(i_UserMove))
                {
                    isMoveExists = true;
                    break; 
                }
            }

            return isMoveExists;
        }


    }
}
