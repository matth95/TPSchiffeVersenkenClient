using SV_Client.Classes;
using SV_Client.Classes.ProgramLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SV_Client.ViewModels
{
    public class vm_LoginInterface : UserControl, INotifyPropertyChanged
    {
        // VARIABLES

        private string pr_InputUsername;
        public string pu_InputUsername
        {
            get { return pr_InputUsername; }
            set 
            { 
                pr_InputUsername = value;
                F_NotifyChanged("pu_InputUsername");
            }
        }

        private RelayCommand pr_LoginCommand;
        public RelayCommand pu_LoginCommand
        {
            get { return pr_LoginCommand; }
            set { pr_LoginCommand = value; }
        }

        // CONSTRUCTOR

        public vm_LoginInterface()
        {
            pr_InputUsername = "";
            pr_LoginCommand = new RelayCommand(param => F_LoginRegister(this));
        }

        // FUNCTIONS

        /// <summary>
        /// This function is called when the Login / Register Button of the uc_LoginInterface is pressed.
        /// It checks if the input from the user corresponds with the Guidelines and then calls a function 
        /// from the vm_StartInterface that sends a login/register request to the server.
        /// </summary>
        private void F_LoginRegister(object source)
        {
            var CurrentLoginInterface = ViewModels.vm_MainInterface.pu_uc_LoginContent;

            if ((pr_InputUsername.Length > 3) && (CurrentLoginInterface.UserPassword.Password.Length > 3))
            {
                if( !pr_InputUsername.Contains(" ") )
                {
                    User TryLoginUser = new User(pr_InputUsername, CurrentLoginInterface.UserPassword.Password);

                    SV_Client.Classes.Client.GeneralInfo.pu_Username = pr_InputUsername;
                    SV_Client.Classes.Client.GeneralInfo.pu_Password = CurrentLoginInterface.UserPassword.Password;

                    var CurrentStartInterface = ViewModels.vm_MainInterface.pu_uc_StartContent.DataContext as vm_StartInterface;

                    CurrentStartInterface.F_SendDataToServerAndReceive("POST LOGIN\n\n" + XmlSerializer.Serialize<User>(TryLoginUser));
                }
                else
                {
                    MessageBox.Show("Nicht erlaubte Zeichen befinden sich im Nutzernamen!");
                }
            }
            else
            {
                MessageBox.Show("Nutzername und Passwort müssen mindestens 4 Zeichen lang sein!");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void F_NotifyChanged(string ChangedComponent)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(ChangedComponent));
            }
        }
    }
}