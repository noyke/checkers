using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class Board
    {
        public enum eBoardSize
        {
            Small = 6,
            Medium = 8,
            Large = 10
        }

        private readonly GamePiece[,] r_GameBoard;

        public Board(eBoardSize i_BoardSize)
        {
            r_GameBoard = new GamePiece[(int)i_BoardSize, (int)i_BoardSize];
        }

        public GamePiece this[int i_X, int i_Y]
        {
            get
            {
                return r_GameBoard[i_X, i_Y];
            }

            set
            {
                r_GameBoard[i_X, i_Y] = value;
            }
        }

        public GamePiece this[Point i_position]
        {
            get
            {
                return this[i_position.X, i_position.Y];
            }

            set
            {
                this[i_position.X, i_position.Y] = value;
            }
        }

        public int Size
        {
            get
            {
                return r_GameBoard.GetLength(0);
            }
        }

        public void SetBoardAndGamePieces(Player[] i_Players)
        {
            setNonPlayerLand();

            foreach (Player player in i_Players)
            {
                setPlayerGamePiecesAndLand(player);
            }
        }

        private void setNonPlayerLand()
        {
            Point currLocation;

            for (int row = (this.Size / 2) - 1; row <= this.Size / 2; row++)
            {
                for (int col = 0; col < this.Size; col++)
                {
                    currLocation = new Point(row, col);
                    this[currLocation] = null;
                }
            }
        }

        private void setPlayerGamePiecesAndLand(Player i_Player)
        {
            GamePiece currGamePiece;
            Point currLocation;
            
            GamePiece.eColor pieceColor = i_Player.PlayerNumber == Player.ePlayerNumber.Player1 ? GamePiece.eColor.Black : GamePiece.eColor.White;
            int firstRow = i_Player.PlayerNumber == Player.ePlayerNumber.Player1 ? (this.Size / 2) + 1 : 0;
            int lastRow = i_Player.PlayerNumber == Player.ePlayerNumber.Player1 ? this.Size : (this.Size / 2) - 1;

            for (int row = firstRow; row < lastRow; row++)
            {
                for (int col = 0; col < this.Size; col++)
                {
                    currLocation = new Point(row, col);

                    if (isSquareIsBlack(currLocation))
                    {
                        currGamePiece = new GamePiece(currLocation, pieceColor);
                        i_Player.AddGamePiece(currGamePiece);
                        this[currLocation] = currGamePiece;
                    }
                    else
                    {
                        this[currLocation] = null;
                    }
                }
            }
        }

        private bool isSquareIsBlack(Point i_Location)
        {
            return (i_Location.X % 2 == 0 && i_Location.Y % 2 != 0) || (i_Location.X % 2 != 0 && i_Location.Y % 2 == 0);
        }

        public void UpdateMoveListsBeforeMove(GamePiece i_GamePiece)
        {
            // TODO !!
        }

        public void UpdateMoveListsAfterMove(GamePiece i_GamePiece)
        {
            // TODO !!
        }

        public bool CheckIsLocationInBound(Point i_Location)
        {
            return 0 <= i_Location.X && i_Location.X < this.Size && 0 <= i_Location.Y && i_Location.Y < this.Size;
        }

        // neighbors methods problem - how to notice when there are out of bound neighbors

        //public GamePiece NeighborForwardRight(GamePiece i_GamePiece)
        //{
        //    int offset = i_GamePiece.Color == GamePiece.eColor.Black ? 1 : -1;

        //    return this[i_GamePiece.Location.X - offset, i_GamePiece.Location.Y + 1];
        //}

        //public GamePiece NeighborForwardLeft(GamePiece i_GamePiece)
        //{
        //    int offset = i_GamePiece.Color == GamePiece.eColor.Black ? 1 : -1;

        //    return this[i_GamePiece.Location.X - offset, i_GamePiece.Location.Y - 1];
        //}

        //public GamePiece NeighborBackwardRight(GamePiece i_GamePiece)
        //{
        //    int offset = i_GamePiece.Color == GamePiece.eColor.Black ? 1 : -1;

        //    return this[i_GamePiece.Location.X + offset, i_GamePiece.Location.Y + 1];
        //}

        //public GamePiece NeighborBackwardLeft(GamePiece i_GamePiece)
        //{
        //    int offset = i_GamePiece.Color == GamePiece.eColor.Black ? 1 : -1;

        //    return this[i_GamePiece.Location.X + offset, i_GamePiece.Location.Y - 1];
        //}
    }
}
