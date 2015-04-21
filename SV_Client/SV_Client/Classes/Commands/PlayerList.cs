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
    public class PlayerList
    {
        private List<User> _onlineUsers;

        public List<User> OnlineUsers
        {
            get { return _onlineUsers; }
            set { _onlineUsers = value; }
        }
    }
}
