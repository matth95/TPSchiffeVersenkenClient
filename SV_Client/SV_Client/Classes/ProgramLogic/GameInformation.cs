namespace SV_Client.Classes.ProgramLogic
{
    public class GameInformation
    {
        private User _user1;

        private User _user2;

        private string _gameName;

        public string GameName
        {
            get { return _gameName; }
            set { _gameName = value; }
        }

        public GameInformation()
        {
            
        }

        public GameInformation(Commands.Game game)
        {
            this.User1 = game.User1;
            this.GameName = game.GameName;
        }

        public User User1
        {
            get { return _user1; }
            set { _user1 = value; }
        }

        public User User2
        {
            get { return _user2; }
            set { _user2 = value; }
        }
    }
}
