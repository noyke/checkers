namespace GameLogic
{
    public struct Point
    {
        private int m_X;
        private int m_Y;
        
        public Point(int i_X, int i_Y)
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

        static public bool IsValidPointFormat(string i_PointStr)
        {
            return i_PointStr.Length == 2 && char.IsUpper(i_PointStr[0]) && char.IsLower(i_PointStr[1]);
        }

        static public Point Parse(string i_PointStr)
        {
            int x = (int)(i_PointStr[0] - 'A');
            int y = (int)(i_PointStr[1] - 'a');

            return new Point(x, y);
        }
    }
}
