using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;

namespace SV_Client.Classes.ProgramLogic
{
    [XmlType(AnonymousType = true)]
    public class MShip
    {
        public int pr_ShipAnchorPoint;
        public bool pr_ShipDirection;
        public int pr_ShipSize;
        [XmlIgnore]
        public UIElement pr_Element;

        public MShip()
        {

        }

        public MShip(int AnchorPoint, bool ShipDirection, int ShipSize, UIElement Element)
        {
            this.pr_ShipAnchorPoint = AnchorPoint;
            this.pr_ShipDirection = ShipDirection;
            this.pr_ShipSize = ShipSize;
            this.pr_Element = Element;
        }
    }
}
