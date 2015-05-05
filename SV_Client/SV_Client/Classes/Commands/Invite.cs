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
        private string _userToInvite;

        public string UserToInvite
        {
            get { return _userToInvite; }
            set { _userToInvite = value; }
        }
    }
}
