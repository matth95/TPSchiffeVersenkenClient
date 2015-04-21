using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SV_Client.Classes.ProgramLogic;

namespace SV_Client.Classes.Commands
{
    /// <summary>
    /// list of open games
    /// </summary>
    [XmlTypeAttribute(AnonymousType = true)]
    public class GameList
    {
        private List<GameInformation> _openGames;

        public List<GameInformation> OpenGames
        {
            get { return _openGames; }
            set { _openGames = value; }
        }

        public GameList()
        {
            _openGames=new List<GameInformation>();
        }
    }
}
