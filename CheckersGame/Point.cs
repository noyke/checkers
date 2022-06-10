namespace GameLogic
{
    public struct Point
    {
        private int m_X;
        private int m_Y;
        
        public Point(int i_Y, int i_X)
        {
            m_X = i_X;
            m_Y = i_Y;
        }

        public int X
        {
            get 
            {
                return m_X;
            }
            set
            {
                m_X = value;
            }
        }

        public int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                m_Y = value;
            }
        }

        public static bool IsValidPointFormat(string i_PointStr)
        {
            return i_PointStr.Length == 2 && char.IsUpper(i_PointStr[0]) && char.IsLower(i_PointStr[1]);
        }

        public static Point Parse(string i_PointStr)
        {
            int x = (int)(i_PointStr[0] - 'A');
            int y = (int)(i_PointStr[1] - 'a');

            return new Point(y, x);
        }

        public override string ToString()
        {
            return ((char)('A' + m_X)).ToString() + ((char)('a' + m_Y)).ToString();
        }
    }
}