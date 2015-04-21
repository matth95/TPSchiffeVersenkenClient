using System.Xml.Serialization;
using SV_Client.Classes.ProgramLogic;

namespace SV_Client.Classes.Commands
{
    /// <summary>
    /// class to start a game with all the information of a game
    /// </summary>
    [XmlType(AnonymousType = true)]
    public class Game
    {
        private string _gameName;

        private User _user1;

        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }

        public User User1
        {
            get { return _user1; }
            set { _user1 = value; }
        }
        
    }
}
