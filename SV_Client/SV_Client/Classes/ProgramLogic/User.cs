using System.Xml.Serialization;
using SV_Client.Classes.ProgramLogic.PlayField;

namespace SV_Client.Classes.ProgramLogic
{
    /// <summary>
    /// the user class in
    /// each game contains 2 users with 2 gamefields
    /// </summary>
    [XmlRoot("User")]
    public class User
    {
        private string _username;

        private string _password;

        private PlayField.PlayField _playField;

        private EnemyPlayField _enemyPlayField;

        /// <summary>
        /// creates a instance of a user with specific information
        /// </summary>
        /// <param name="username">username to login</param>
        /// <param name="password">password to login</param>
        /// <param name="sizeX">size of the gamefields in x axis</param>
        /// <param name="sizeY">size of the gamefields in y axis</param>
        public User(string username, string password,int sizeX=10,int sizeY=10)
        {
            Username = username;
            Password = password;
            PlayField=new PlayField.PlayField(sizeX,sizeY);
            EnemyPlayField=new EnemyPlayField(sizeX,sizeY);

        }
        public User()
        {
            
        }

        public PlayField.PlayField PlayField
        {
            get { return _playField; }
            set { _playField = value; }
        }

        public EnemyPlayField EnemyPlayField
        {
            get { return _enemyPlayField; }
            set { _enemyPlayField = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
    }
}
