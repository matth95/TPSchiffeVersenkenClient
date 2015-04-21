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
    public class Invite
    {
        private User _userToInvite;

        public User UserToInvite
        {
            get { return _userToInvite; }
            set { _userToInvite = value; }
        }

        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }

        private string _gameName;
    }
}
