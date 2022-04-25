using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public struct Move
    {
        private Point m_Source;
        private Point m_Destination;

        public Move(Point i_Source, Point i_Destination)
        {
            m_Source = i_Source;
            m_Destination = i_Destination;
        }

        public Point Source
        {
            get
            {
                return m_Source;
            }
        }

        public Point Destination
        {
            get
            {
                return m_Destination;
            }
        }

        public bool CheckIsRegularMove()
        {
            return CheckIsCheckersMove(1);
        }

        public bool CheckIsJumpMove()
        {
            return CheckIsCheckersMove(2);
        }

        private bool CheckIsCheckersMove(int i_Offset)
        {
            bool isCheckersMoveUpward = m_Source.X - i_Offset == m_Destination.X && (m_Source.Y + i_Offset == m_Destination.Y || m_Source.Y - i_Offset == m_Destination.Y);
            bool isCheckersMoveDownward = m_Source.X + i_Offset == m_Destination.X && (m_Source.Y + i_Offset == m_Destination.Y || m_Source.Y - i_Offset == m_Destination.Y);

            return isCheckersMoveUpward || isCheckersMoveDownward;
        }

        public static bool IsValidMoveFormat(string i_MoveStr)
        {
            string[] pointsStrs = i_MoveStr.Split('>');

            return pointsStrs.Length == 2 && Point.IsValidPointFormat(pointsStrs[0]) && Point.IsValidPointFormat(pointsStrs[1]);
        }

        public static Move Parse(string i_MoveStr)
        {
            string[] pointsStrs = i_MoveStr.Split('>');

            Point source = Point.Parse(pointsStrs[0]);
            Point destination = Point.Parse(pointsStrs[1]);

            return new Move(source, destination);
        }
    }
}
