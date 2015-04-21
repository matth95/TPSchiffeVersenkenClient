using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SV_Client.Classes.ProgramLogic.PlayField;

namespace SV_Client.Classes.ProgramLogic
{
    public class Attack
    {
        public Attack(int x, int y)
        {
            Point = new CPoint {X = x, Y = y};
        }

        public Attack()
        {
            
        }

        public CPoint Point { get; set; }
    }
}
