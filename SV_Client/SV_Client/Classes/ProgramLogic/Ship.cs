using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace SV_Client.Classes.ProgramLogic
{
    [XmlType(AnonymousType = true)]
    public class Ship
    {
        private int _size;

        public CPoint Point { get; set; }

        private List<CPoint> _pointsOfShip;

        public bool Horizontal
        {
            get { return _horizontal; }
            set
            {
                _horizontal = value;
                if (PointsOfShip.Count != 0)
                {
                    PointsOfShip.RemoveRange(0, PointsOfShip.Count - 1);
                }
                PointsOfShip.Add(Point);
                for (var x = 1; x < _size; x++)
                {
                    PointsOfShip.Add(_horizontal
                        ? new CPoint {X = Point.X + x, Y = Point.Y}
                        : new CPoint {X = Point.X, Y = Point.Y + x});
                }
            }
        }

        public List<CPoint> PointsOfShip
        {
            get { return _pointsOfShip; }
            set
            { _pointsOfShip = value; }
        }

        private bool _horizontal;

        public Ship()
        {

        }

        public Ship(int size, bool horizontal=true)
        {
            _size = size;
            Point = new CPoint { X = 0, Y = 0 };
            PointsOfShip = new List<CPoint>();
            Horizontal = horizontal;
        }

        public Ship(int size, int x, int y, bool horizontal=true)
        {
            _size = size;
            Point = new CPoint { X = x, Y = y };
            PointsOfShip = new List<CPoint>();
            Horizontal = horizontal;
        }
    }
}
