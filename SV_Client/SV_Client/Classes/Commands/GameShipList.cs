using SV_Client.Classes.ProgramLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SV_Client.Classes.Commands
{
    [XmlTypeAttribute(AnonymousType = true)]
    public class GameShipList
    {
        public List<MShip> Ships;

        public GameShipList()
        {
            this.Ships = new List<MShip>();
        }
    }
}
