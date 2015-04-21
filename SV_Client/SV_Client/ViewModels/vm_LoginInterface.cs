using SV_Client.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private string pr_InputPassword;
        public string pu_InputPassword
        {
            get { return pr_InputPassword; }
            set 
            { 
                pr_InputPassword = value;
                F_NotifyChanged("pu_InputPassword");
            }
        }

        private RelayCommand pr_LoginCommand;
        public RelayCommand pu_LoginCommand
        {
            get { return pr_LoginCommand; }
            set { pr_LoginCommand = value; }
        }

        private RelayCommand pr_RegisterCommand;
        public RelayCommand pu_RegisterCommand
        {
            get { return pr_RegisterCommand; }
            set { pr_RegisterCommand = value; }
        }

        // CONSTRUCTORS

        public vm_LoginInterface()
        {
            pr_LoginCommand = new RelayCommand(param => F_Login(this));
            pr_RegisterCommand = new RelayCommand(param => F_Register(this));
        }

        // FUNCTIONS

        private void F_Register(object source)
        {
            SV_Client.Classes.Client.GeneralInfo.pu_Username = pr_InputUsername;
            SV_Client.Classes.Client.GeneralInfo.pu_Password = pr_InputPassword;
            ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(source);
        }

        private void F_Login(object source)
        {
            SV_Client.Classes.Client.GeneralInfo.pu_Username = pr_InputUsername;
            SV_Client.Classes.Client.GeneralInfo.pu_Password = pr_InputPassword;
            ViewModels.vm_MainInterface.pu_ChangeGUICommand.Execute(source);
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
