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
        Validator m_Validator;

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
        }

        private Player currentPlayer()
        {
            return m_Players[m_CurrentPlayerIndex];
        }

        public Player.eType CurrentPlayerType()
        {
            return currentPlayer().Type;
        }

        private void swapCurrentPlayer()
        {
            m_CurrentPlayerIndex = (m_CurrentPlayerIndex + 1) % 2;
        }

        public void MakeComputerMove()
        {
            // TODO
        }

        public bool MakeUserMoveAndUpdates(Move i_UserMove, out bool o_AnotherJump)
        {
            bool isMoved, inBounds, isBelongs, isValid;
            o_AnotherJump = false;

            inBounds = m_GameBoard.CheckIsLocationInBound(i_UserMove.Source) && m_GameBoard.CheckIsLocationInBound(i_UserMove.Destination); //SNIR checkBounds
            isBelongs = checkIsSourceBelongsToPlayer(m_GameBoard[i_UserMove.Source]); // SNIR check that the gamePiece belongs to player
            isValid = checkIfRegularOrJumpMoveIsValid(i_UserMove); //SNIR check if the move is in the Move\Jump valid list

            isMoved = inBounds || isBelongs || isValid;

            if (isMoved)
            {
                if (i_UserMove.CheckIsJumpMove())
                {
                    makeAnEat(i_UserMove);
                    makePlayersUpdates(m_GameBoard);
                    o_AnotherJump = m_GameBoard[i_UserMove.Destination].CheckIsHaveAPossibleJumpMove(); //SNIR check if there is another jump to make 
                }

                else
                {
                    makeMove(i_UserMove);
                    makePlayersUpdates(m_GameBoard);
                }

                checkIfKingAndCrown(i_UserMove.Destination); // SNIR if needed crown the piece 
                 
                
            }

            if(!o_AnotherJump)
            {
                swapCurrentPlayer(); //SNIR here or in UI?
            }

            return isMoved;


            /////// notice: getting here only if there is at least one possible move (checked on UI loop) //////

            // V check if both of the points are in bound. // using CheckIsLocationInBound from Board / Validator 
            // V check if in source point there is a current player's gamePiece. // 
            // V check if last move made by the same current player, if so, check if current move source equals to last move destination (***).
            // V determine is it regular/jump move or non.     //  Move's method?
            // V if its a regular move, check whether the current player has a mandatory move (the case of (***) or a generally).
            // V check if that move exist in the relevant(regular/jump) moveList.
            // operate the move and updates on board and gamePiece (+ update moveLists of current gamePiece and relevants neighbors. + crown current gamePiece if needed).

            // in case a move was made:
            //      update the m_LastMove to current move.
            //      if jump move operated and the current gamePiece has another possible jump move - don't swap current player
            //      else swap current player.
            // loop to multy eat after one step (check if there another eat to make and enable just the valid jump 
            // is the piece is now king? 
            // update piece that were eaten 

            // return whether a move was made.
        }

        private void makeMove(Move i_UserMove) //SNIR make the actual move 
        {
            m_GameBoard[i_UserMove.Source].Location = m_GameBoard[i_UserMove.Destination].Location;
            m_GameBoard[i_UserMove.Destination] = m_GameBoard[i_UserMove.Source];
            m_GameBoard[i_UserMove.Source] = null;
        }

        private void makeAnEat(Move i_move) // SNIR the piece that was eatan became(insane) null MODOULATE???
        {
            Point eatenPieceLocation;
            GamePiece SourceGamePiece = m_GameBoard[i_move.Source];

            if(i_move.Source.X < i_move.Destination.X)
            {
               if(SourceGamePiece.GetType().Equals(GamePiece.eType.Man))
                {
                    eatenPieceLocation = SourceGamePiece.NeighborForwardRightLocation();
                }

               else
                {
                    eatenPieceLocation = SourceGamePiece.NeighborBackwardRightLocation();
                }
            }

            else
            { 
                if (SourceGamePiece.GetType().Equals(GamePiece.eType.Man))
                {
                    eatenPieceLocation = SourceGamePiece.NeighborForwardLeftLocation();
                }

                else
                {
                    eatenPieceLocation = SourceGamePiece.NeighborBackwardLeftLocation();
                }
            }

            m_GameBoard[eatenPieceLocation] = null;
            makeMove(i_move);
        }

        public bool IsContinueJump(Point i_LastLocation, Move i_CurrUserMove)
        {
            bool isValidJump = false; 

            if(i_LastLocation.Equals(i_CurrUserMove.Source))
            {
                foreach(Move move in m_GameBoard[i_LastLocation].getPossibleJumpMovesList())
                {
                    isValidJump = move.Equals(i_CurrUserMove);
                }
            }

            return isValidJump;
        }

        public bool CheckIsRoundOver()
        {
            bool isRoundOver = false;

            foreach(Player player in m_Players)
            {
                isRoundOver = player.CheckIsHaveAPossibleMove() || player.GetGamePieceList() == null;
            }

            return isRoundOver;
          
        }

        public eRoundResult roundResult()
        {
            eRoundResult result; 
            if(m_Players[m_CurrentPlayerIndex].GetGamePieceList() == null)
            {
                result = m_CurrentPlayerIndex.Equals(0) ? eRoundResult.Player2Win : eRoundResult.Player1Win;
            }

            if(m_Players[m_CurrentPlayerIndex].CheckIsHaveAPossibleMove())
            {
                result = m_CurrentPlayerIndex.Equals(0) ? eRoundResult.Player2Win : eRoundResult.Player1Win;
            }

            else if()
            {

            }

            else
            {
                result = eRoundResult.Draw;
            }

            if (result != eRoundResult.Draw)
            {
                swapCurrentPlayer();
                m_Players[m_CurrentPlayerIndex]
                    }

            // if a player has no gamePieces - he lose.

            // if the current player has possible moves - current player lose (because he quitted).
            // else if the other player has possible moves - current player lose.
            // else - draw.

            // if not a draw, calculate and update winner score as (winner's amount of gamePieces - loser's amount of gamePieces) while a king worth 4 gamePieces.
        }

        private bool checkIsSourceBelongsToPlayer(GamePiece i_Source)
        {
            return i_Source.Color == GamePiece.eColor.Black && m_CurrentPlayerIndex == 0 || i_Source.Color == GamePiece.eColor.White && m_CurrentPlayerIndex == 1;
        }

        private bool checkIfRegularOrJumpMoveIsValid(Move i_move)
        {
            bool validMove = false;
            if (i_move.CheckIsRegularMove())
            {
                List<Move> PossibleRegularMoves = m_GameBoard[i_move.Destination].getPossibleRegularMovesList();

                foreach (Move move in PossibleRegularMoves)
                {
                    validMove = i_move.Equals(move);
                }
            }

            if (i_move.CheckIsJumpMove())
            {
                List<Move> PossibleJumpMoves = m_GameBoard[i_move.Destination].getPossibleJumpMovesList();

                foreach (Move move in PossibleJumpMoves)
                {
                    validMove = i_move.Equals(move);
                }
            }

            return validMove;
        }

        private void makePlayersUpdates(Board i_GameBoard)
        {
            foreach(Player player in m_Players)
            {
                player.UpdatePossibleRegularMoves(i_GameBoard);
                player.UpdatePossibleJumpMoves(i_GameBoard);
            }
        }
        private void checkIfKingAndCrown(Point i_CurrLocation) 
        {
            if (m_GameBoard[i_CurrLocation].GetType().Equals(GamePiece.eType.Man))
            {
                if ((m_CurrentPlayerIndex.Equals(0) && i_CurrLocation.Y.Equals(0)) || (m_CurrentPlayerIndex.Equals(1) && i_CurrLocation.Y.Equals(m_GameBoard.Size)))
                {
                    m_GameBoard[i_CurrLocation].Type = GamePiece.eType.King;
                }
            }
        }
    }
}
