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
    public class vm_StartInterface
    {
        // VARIABLEN

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

        private RelayCommand pr_RankingCommand;
        public RelayCommand pu_RankingCommand
        {
            get { return pr_RankingCommand; }
            set { pr_RankingCommand = value; }
        }

        private RelayCommand pr_OptionCommand;
        public RelayCommand pu_OptionCommand
        {
            get { return pr_OptionCommand; }
            set { pr_OptionCommand = value; }
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

        // KONSTRUKTOR

        public vm_StartInterface()
        {
            pr_UDPServerReceivePort = 40000;
            pr_UDPServerSendPort = 40001;

            pr_UDPServerSendingStation = new UdpClient(pr_UDPServerSendPort, AddressFamily.InterNetwork);

            BackgroundWorker ReceiveDataWorker = new BackgroundWorker();
            ReceiveDataWorker.DoWork += F_ReceiveDataFromServer;
            ReceiveDataWorker.RunWorkerAsync();

            pr_AvailablePlayerList = new ObservableCollection<ListBoxItem>();
            pr_OpenGameList = new ObservableCollection<ListBoxItem>();

            pr_GameStartCommand = new RelayCommand(param => F_StartGame(this));
            pr_RankingCommand = new RelayCommand(param => F_ShowRanking());
            pr_OptionCommand = new RelayCommand(param => F_ShowOptions());
            pr_ExitCommand = new RelayCommand(param => F_ExitProgram());
            pr_RefreshCommand = new RelayCommand(param => F_RefreshHosts());
            pr_JoinCommand = new RelayCommand(param => F_JoinGame());
            pr_InviteCommand = new RelayCommand(param => F_InvitePlayer());
            pr_GameListFocusCommand = new RelayCommand(param => F_ClearFocus(pr_AvailablePlayerList)); 
            pr_PlayerListFocusCommand = new RelayCommand(param => F_ClearFocus(pr_OpenGameList));
        }


        // FUNKTIONEN

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

            //ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(source);
        }

        private void F_ShowRanking()
        {
        }

        private void F_ShowOptions()
        {
            MessageBox.Show("Options");
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

        private void F_JoinGame()
        {
            ListBoxItem JoinTarget = F_ReturnSelectedItemFrom(pr_OpenGameList);

            if(JoinTarget != null)
            {
                MessageBox.Show(JoinTarget.Content.ToString());
            }
            else
            {
                MessageBox.Show("Um einem Spiel beizutreten müssen Sie zuerst ein Spiel auswählen!");
            }
        }

        private void F_InvitePlayer()
        {
            ListBoxItem InviteTarget = F_ReturnSelectedItemFrom(pr_AvailablePlayerList);

            if (InviteTarget != null)
            {
                MessageBox.Show(InviteTarget.Content.ToString());
            }
            else
            {
                MessageBox.Show("Um einen Spieler einzuladen müssen Sie zuerst einen Spieler auswählen!");
            }
        }

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

        private void F_SendDataToServerAndReceive(string DataToSend)
        {
            IPEndPoint ServerEndPoint = new IPEndPoint(pr_UDPServerEndpoint.Address, pr_UDPServerSendPort);
            var sendCode = Encoding.ASCII.GetBytes(DataToSend);
            pr_UDPServerSendingStation.Connect(ServerEndPoint);
            pr_UDPServerSendingStation.Send(sendCode, sendCode.Length);
            var answer = Encoding.ASCII.GetString(pr_UDPServerSendingStation.Receive(ref ServerEndPoint));
            F_InterpretDataFromServer(answer);
        }

        private void F_SendDataToServer(string DataToSend)
        {
            IPEndPoint ServerEndPoint = new IPEndPoint(pr_UDPServerEndpoint.Address, pr_UDPServerSendPort);
            var sendCode = Encoding.ASCII.GetBytes(DataToSend);
            pr_UDPServerSendingStation.Connect(ServerEndPoint);
            pr_UDPServerSendingStation.Send(sendCode, sendCode.Length);
        }

        private void F_ReceiveDataFromServer(object sender, DoWorkEventArgs e)
        {

            pr_UDPServerReceivingStation = new UdpClient(pr_UDPServerReceivePort, AddressFamily.InterNetwork);
            pr_UDPServerEndpoint = new IPEndPoint(IPAddress.Any, pr_UDPServerReceivePort);
            SV_Client.Classes.Client.GeneralInfo.pu_ServerIP = pr_UDPServerEndpoint.Address;

            while (true)
            {
                var receivedData = pr_UDPServerReceivingStation.Receive(ref pr_UDPServerEndpoint);
                string receivedDataString = Encoding.ASCII.GetString(receivedData);
                Console.WriteLine(pr_UDPServerEndpoint.Address.ToString());
                Console.WriteLine(receivedDataString);
                F_InterpretDataFromServer(receivedDataString);
            }
           
           
        }

        private void F_InterpretDataFromServer(string DataToInterpret)
        {
            var DataToInterpretSplitted = DataToInterpret.Split(new[] { "\n\n" }, StringSplitOptions.None);
            string DataToInterpretHeader = DataToInterpretSplitted[0];

            //ONLINE
            if (DataToInterpretHeader.Split('\n')[0].IndexOf("ONLINE") > 0)
            {
                F_getGameList();
                //F_getPlayerList();
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

                    F_getPlayerList();
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
                        pr_AvailablePlayerList[lauf].Content = CurrentPlayerList.OnlineUsers[lauf];
                    }
                }));
                
            }
            else if ( DataToInterpretHeader.Split('\n')[0].IndexOf("RANKING") > 0 )
            {
                var RankingList = Classes.XmlSerializer.Deserialize<Ranking>(DataToInterpretSplitted[1]);
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