using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SV_Client.Classes
{
    public class TestShip
    {
        public int pr_ShipAnchorPoint;
        public bool pr_ShipDirection;
        public int pr_ShipSize;
        public UIElement pr_Element;

        public TestShip(int AnchorPoint, bool ShipDirection, int ShipSize, UIElement Element)
        {
            this.pr_ShipAnchorPoint = AnchorPoint;
            this.pr_ShipDirection = ShipDirection;
            this.pr_ShipSize = ShipSize;
            this.pr_Element = Element;
        }

    }
}
