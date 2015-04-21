using System;
using System.Windows;

namespace SV_Client.Classes.ProgramLogic
{
    public class CPoint : IComparable<CPoint>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool IsHit
        {
            get { return _isHit; }
            set { _isHit = value; }
        }

        private bool _isHit;
        public int CompareTo(CPoint other)
        {
            if (X == other.X && Y == other.Y)
            {
                return 0;
            }
            return 1;
        }
    }


}
