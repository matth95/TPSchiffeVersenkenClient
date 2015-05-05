using SV_Client.Classes;
using SV_Client.Classes.Commands;
using SV_Client.Classes.ProgramLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SV_Client.ViewModels
{
    public class vm_StartInterface : UserControl
    {
        // VARIABLES

        private UdpClient pr_UDPServerReceivingStation;
        private UdpClient pr_UDPServerSendingStation;
        private IPEndPoint pr_UDPServerEndpoint;
        private int pr_UDPServerReceivePort;
        private int pr_UDPServerSendPort;

        private ObservableCollection<ListBoxItem> pr_AvailablePlayerList;
        public ObservableCollection<ListBoxItem> pu_AvailablePlayerList
        {
            get { return pr_AvailablePlayerList; }
            set { pr_AvailablePlayerList = value; }
        }

        private ObservableCollection<ListBoxItem> pr_OpenGameList;
        public ObservableCollection<ListBoxItem> pu_OpenGameList
        {
            get { return pr_OpenGameList; }
            set { pr_OpenGameList = value; }
        }

        private RelayCommand pr_GameStartCommand;
        public RelayCommand pu_GameStartCommand
        {
            get { return pr_GameStartCommand; }
            set { pr_GameStartCommand = value; }
        }

        private RelayCommand pr_ExitCommand;
        public RelayCommand pu_ExitCommand
        {
            get { return pr_ExitCommand; }
            set { pr_ExitCommand = value; }
        }

        private RelayCommand pr_RefreshCommand;
        public RelayCommand pu_RefreshCommand
        {
            get { return pr_RefreshCommand; }
            set { pr_RefreshCommand = value; }
        }

        private RelayCommand pr_JoinCommand;
        public RelayCommand pu_JoinCommand
        {
            get { return pr_JoinCommand; }
            set { pr_JoinCommand = value; }
        }

        private RelayCommand pr_InviteCommand;
        public RelayCommand pu_InviteCommand
        {
            get { return pr_InviteCommand; }
            set { pr_InviteCommand = value; }
        }

        private RelayCommand pr_GameListFocusCommand;
        public RelayCommand pu_GameListFocusCommand
        {
            get { return pr_GameListFocusCommand; }
            set { pr_GameListFocusCommand = value; }
        }

        private RelayCommand pr_PlayerListFocusCommand;
        public RelayCommand pu_PlayerListFocusCommand
        {
            get { return pr_PlayerListFocusCommand; }
            set { pr_PlayerListFocusCommand = value; }
        }

        // CONSTRUCTOR

        public vm_StartInterface()
        {
            pr_UDPServerReceivePort = 40000;
            pr_UDPServerSendPort = 40001;

            try
            {
                pr_UDPServerSendingStation = new UdpClient(pr_UDPServerSendPort, AddressFamily.InterNetwork);

                BackgroundWorker ReceiveDataWorker = new BackgroundWorker();
                ReceiveDataWorker.DoWork += F_ReceiveDataFromServer;
                ReceiveDataWorker.RunWorkerAsync();
            }
            catch(SocketException)
            {
                MessageBox.Show("Sockets für den Datenaustausch mit dem Server werden bereits verwendet!");
                Application.Current.Shutdown();
            }

            pr_AvailablePlayerList = new ObservableCollection<ListBoxItem>();
            pr_OpenGameList = new ObservableCollection<ListBoxItem>();

            pr_GameStartCommand = new RelayCommand(param => F_StartGame(this));
            pr_ExitCommand = new RelayCommand(param => F_ExitProgram());
            pr_RefreshCommand = new RelayCommand(param => F_RefreshHosts());
            pr_JoinCommand = new RelayCommand(param => F_JoinGame());
            pr_InviteCommand = new RelayCommand(param => F_InvitePlayer());
            pr_GameListFocusCommand = new RelayCommand(param => F_ClearFocus(pr_AvailablePlayerList)); 
            pr_PlayerListFocusCommand = new RelayCommand(param => F_ClearFocus(pr_OpenGameList));
        }


        // FUNCTIONS

        /// <summary>
        /// This function is called when the "Create Game" - button of the uc_Startinterface is pressed.
        /// It creates a Dialog for the Input of the Gamename and then sends a put to the server with the
        /// information about the game and the user that created it.
        /// </summary>
        /// <param name="source"></param>
        private void F_StartGame(object source)
        {
            var InputDialogObj = new SV_Client.Dialog.StartGameInputWindow();
            var GameInputDialog = InputDialogObj.ShowDialog();

            if( GameInputDialog.Value )
            {
                User CurrentUser = new User(SV_Client.Classes.Client.GeneralInfo.pu_Username, SV_Client.Classes.Client.GeneralInfo.pu_Password);
              
                Game NewGame = new Game();
                NewGame.GameName = Dialog.StartGameInputWindow.pu_InputGameName;
                NewGame.User1 = CurrentUser;
                F_SendDataToServer("PUT GAME\n\n" + XmlSerializer.Serialize<Game>(NewGame));

                ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(source);
            }
            else
            {
            }
        }

        private void F_ExitProgram()
        {
            Application.Current.Shutdown();
        }

        private void F_RefreshHosts()
        {
            F_getGameList();
            F_getPlayerList();
        }

        /// <summary>
        /// This function is called when a valid target from the GameList is selected and when the join button is pressed.
        /// It sends a request to the server that asks to join the selected server.
        /// </summary>
        private void F_JoinGame()
        {
            ListBoxItem JoinTarget = F_ReturnSelectedItemFrom(pr_OpenGameList);

            if(JoinTarget != null)
            {
                User CurrentUser = new User(SV_Client.Classes.Client.GeneralInfo.pu_Username, SV_Client.Classes.Client.GeneralInfo.pu_Password);

                Join OpenGame = new Join();
                OpenGame.Game = JoinTarget.Content.ToString();
                OpenGame.User2 = CurrentUser;
                F_SendDataToServer("PUT JOIN\n\n" + XmlSerializer.Serialize<Join>(OpenGame));


                ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(this);
            }
            else
            {
                MessageBox.Show("Um einem Spiel beizutreten müssen Sie zuerst ein Spiel auswählen!");
            }
        }

        /// <summary>
        /// This function is called when a valid target from the PlayerList is selected and when the invite button is pressed.
        /// It sends a request to the server that asks for the selected player to be invited.
        /// </summary>
        private void F_InvitePlayer()
        {
            ListBoxItem InviteTarget = F_ReturnSelectedItemFrom(pr_AvailablePlayerList);

            if (InviteTarget != null)
            {
                User CurrentUser = new User(SV_Client.Classes.Client.GeneralInfo.pu_Username, SV_Client.Classes.Client.GeneralInfo.pu_Password);

                Invite AvailableUser = new Invite();
                AvailableUser.UserToInvite = InviteTarget.Content.ToString();
                F_SendDataToServerAndReceive("POST INVITE\n\n" + XmlSerializer.Serialize<Invite>(AvailableUser));
            }
            else
            {
                MessageBox.Show("Um einen Spieler einzuladen müssen Sie zuerst einen Spieler auswählen!");
            }
        }

        /// <summary>
        /// This function makes it so that you can not have Items selected in both the PlayerList and the GameList.
        /// It is called whenever a new Item in either Player- or GameList is selected.
        /// </summary>
        /// <param name="SourceList"></param>
        private void F_ClearFocus(ObservableCollection<ListBoxItem> SourceList)
        {
            for (int lauf = 0; lauf < SourceList.Count; lauf++)
            {
                if (SourceList.ElementAt(lauf).IsSelected)
                {
                    SourceList.ElementAt(lauf).IsSelected = false;
                }
            }
        }

        /// <summary>
        /// Returns the selected ListBoxItem.
        /// </summary>
        /// <param name="SourceList"></param>
        /// <returns></returns>
        private ListBoxItem F_ReturnSelectedItemFrom(ObservableCollection<ListBoxItem> SourceList)
        {
            for (int lauf = 0; lauf < SourceList.Count; lauf++)
            {
                if (SourceList.ElementAt(lauf).IsSelected)
                {
                    return SourceList.ElementAt(lauf);
                }
            }

            return null;
        }

        /// <summary>
        /// This function sends the Data from the parameter to the server and waits afterwards for an answer 
        /// to the data that was sent, which is then sent to the interpreter.
        /// </summary>
        /// <param name="DataToSend"></param>
        public void F_SendDataToServerAndReceive(string DataToSend)
        {
            try
            {
                IPEndPoint ServerEndPoint = new IPEndPoint(pr_UDPServerEndpoint.Address, pr_UDPServerSendPort);
                var sendCode = Encoding.ASCII.GetBytes(DataToSend);
                pr_UDPServerSendingStation.Connect(ServerEndPoint);
                pr_UDPServerSendingStation.Send(sendCode, sendCode.Length);
                var answer = Encoding.ASCII.GetString(pr_UDPServerSendingStation.Receive(ref ServerEndPoint));
                Console.WriteLine(answer);
                F_InterpretDataFromServer(answer);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ein Problem mit der Verbindung zum Server ist aufgetreten!");
                Application.Current.Shutdown();
            }
            
        }

        /// <summary>
        /// This function sends the Data from the parameter to the server.
        /// </summary>
        /// <param name="DataToSend"></param>
        private void F_SendDataToServer(string DataToSend)
        {
            try
            {
                IPEndPoint ServerEndPoint = new IPEndPoint(pr_UDPServerEndpoint.Address, pr_UDPServerSendPort);
                var sendCode = Encoding.ASCII.GetBytes(DataToSend);
                pr_UDPServerSendingStation.Connect(ServerEndPoint);
                pr_UDPServerSendingStation.Send(sendCode, sendCode.Length);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ein Problem mit der Verbindung zum Server ist aufgetreten!");
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// This function is permanently checking for any data sent by the server in the background and
        /// whenever it receives data it is sent to the interpreter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void F_ReceiveDataFromServer(object sender, DoWorkEventArgs e)
        {
            pr_UDPServerReceivingStation = new UdpClient(pr_UDPServerReceivePort, AddressFamily.InterNetwork);
            pr_UDPServerEndpoint = new IPEndPoint(IPAddress.Any, 0);
            
            while (true)
            {
                var receivedData = pr_UDPServerReceivingStation.Receive(ref pr_UDPServerEndpoint);
                SV_Client.Classes.Client.GeneralInfo.pu_ServerIP = pr_UDPServerEndpoint.Address;
                string receivedDataString = Encoding.ASCII.GetString(receivedData);
                F_InterpretDataFromServer(receivedDataString);
            }
           
        }

        /// <summary>
        /// This function interprets the data received over the parameter and executes further code 
        /// depending on the received data.
        /// </summary>
        /// <param name="DataToInterpret"></param>
        private void F_InterpretDataFromServer(string DataToInterpret)
        {
            var DataToInterpretSplitted = DataToInterpret.Split(new[] { "\n\n" }, StringSplitOptions.None);
            string DataToInterpretHeader = DataToInterpretSplitted[0];

            if (DataToInterpretHeader.Split('\n')[0].IndexOf("ONLINE") > 0)
            {
                F_getGameList();
                F_getPlayerList();
            }
            else if ( DataToInterpretHeader.Split('\n')[0].IndexOf("GAMELIST") > 0 )
            {
                try
                {
                    var CurrentGameList = Classes.XmlSerializer.Deserialize<GameList>(DataToInterpretSplitted[1]);

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        for (var lauf = 0; lauf < CurrentGameList.OpenGames.Count; lauf++)
                        {
                            pr_OpenGameList.Add(new ListBoxItem());
                            pr_OpenGameList[lauf].Content = CurrentGameList.OpenGames[lauf].GameName;
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if ( DataToInterpretHeader.Split('\n')[0].IndexOf("PLAYERLIST") > 0 )
            {
                var CurrentPlayerList = Classes.XmlSerializer.Deserialize<PlayerList>(DataToInterpretSplitted[1]);

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    for (var lauf = 0; lauf < CurrentPlayerList.OnlineUsers.Count; lauf++)
                    {
                        pr_AvailablePlayerList.Add(new ListBoxItem());
                        if (CurrentPlayerList.OnlineUsers[lauf].Username != SV_Client.Classes.Client.GeneralInfo.pu_Username)
                        {
                            pr_AvailablePlayerList[lauf].Content = CurrentPlayerList.OnlineUsers[lauf].Username;
                        }
                    }
                }));
                
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("RESPONSE SUCCESS LOGIN") > 0)
            {
                ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(this);
            }
            else if (DataToInterpretHeader.Split('\n')[0].IndexOf("RESPONSE FAIL") > 0)
            {
                MessageBox.Show("Username oder Password falsch! bzw. User nicht vorhanden!");
            }
            else if( DataToInterpretHeader.Split('\n')[0].IndexOf("INVITE FAIL") > 0)
            {
                MessageBox.Show("Gegenüber hat Einladung abgelehnt!");
            }
            else if( DataToInterpretHeader.Split('\n')[0].IndexOf("INVITE SUCCESS") > 0)
            {
                ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(this);
            }
            else if( DataToInterpretHeader.Split('\n')[0].IndexOf("INVITE") > 0)
            {
                var InputDialogObj = new SV_Client.Dialog.InviteReceivedInputWindow();
                var GameInputDialog = InputDialogObj.ShowDialog();

                User CurrentUser = new User(SV_Client.Classes.Client.GeneralInfo.pu_Username, SV_Client.Classes.Client.GeneralInfo.pu_Password);

                if (GameInputDialog.Value)
                {
                    F_SendDataToServer("ACCEPT INVITE\n\n" + XmlSerializer.Serialize<User>(CurrentUser));

                    ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(this);
                }
                else
                {
                    F_SendDataToServer("DECLINE INVITE\n\n" + XmlSerializer.Serialize<User>(CurrentUser));
                }
            }
        }

        private void F_getGameList()
        {
            F_SendDataToServerAndReceive("GET GAMELIST");
        }

        private void F_getPlayerList()
        {
            F_SendDataToServerAndReceive("GET PLAYERLIST");
        }
    }
}