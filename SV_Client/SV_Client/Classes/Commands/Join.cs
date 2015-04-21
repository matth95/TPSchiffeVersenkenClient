using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SV_Client.Classes.ProgramLogic;

namespace SV_Client.Classes.Commands
{

    [XmlType(AnonymousType = true)]
    public class Join
    {
        private string _gameName;

        public string Game
        {
            get { return _gameName; }
            set { _gameName = value; }
        }

        public User User2
        {
            get { return _user2; }
            set { _user2 = value; }
        }

        private User _user2;
    }
}
