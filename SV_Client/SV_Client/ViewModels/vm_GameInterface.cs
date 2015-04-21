using SV_Client.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SV_Client.ViewModels
{
    public class vm_GameInterface : UserControl, INotifyPropertyChanged
    {
        // VARIABLES

        private const double pr_GameFieldRequiredWidth = 1280;
        private const double pr_GameFieldRequiredHeight = 960;

        public static double pustat_ScaleWidth;
        public static double pustat_ScaleHeight;

        private double pr_ScaleWidth;
        public double pu_ScaleWidth
        {
            get { return pr_ScaleWidth; }
            set 
            { 
                pr_ScaleWidth = value;
                pustat_ScaleWidth = value;
                F_NotifyChanged("pu_ScaleWidth");
            }
        }

        private double pr_ScaleHeight;
        public double pu_ScaleHeight
        {
            get { return pr_ScaleHeight; }
            set 
            { 
                pr_ScaleHeight = value;
                pustat_ScaleHeight = value;
                F_NotifyChanged("pu_ScaleHeight");
            }
        }
        
        private RelayCommand pr_SizeChangeCommand;
        public RelayCommand pu_SizeChangeCommand
        {
            get { return pr_SizeChangeCommand; }
            set { pr_SizeChangeCommand = value; }
        }

        private RelayCommand pr_LoadedSizeCommand;
        public RelayCommand pu_LoadedSizeCommand
        {
            get { return pr_LoadedSizeCommand; }
            set { pr_LoadedSizeCommand = value; }
        }

        private RelayCommand pr_SurrenderCommand;
        public RelayCommand pu_SurrenderCommand
        {
            get { return pr_SurrenderCommand; }
            set { pr_SurrenderCommand = value; }
        }

        private string pr_OwnUsername;
        public string pu_OwnUsername
        {
            get { return pr_OwnUsername; }
            set
            {
                pr_OwnUsername = value;
                F_NotifyChanged("pu_OwnUsername");
            }
        }

        private string pr_OpponentUsername;
        public string pu_OpponentUsername
        {
            get { return pr_OpponentUsername; }
            set
            {
                pr_OpponentUsername = value;
                F_NotifyChanged("pu_OpponentUsername");
            }
        }

        private string pr_OwnShipsRemaining;
        public string pu_OwnShipsRemaining
        {
            get { return pr_OwnShipsRemaining; }
            set
            {
                pr_OwnShipsRemaining = value;
                F_NotifyChanged("pu_OwnShipsRemaining");
            }
        }

        private string pr_OpponentShipsRemaining;
        public string pu_OpponentShipsRemaining
        {
            get { return pr_OpponentShipsRemaining; }
            set
            {
                pr_OpponentShipsRemaining = value;
                F_NotifyChanged("pu_OpponentShipsRemaining");
            }
        }

        // CONSTRUCTORS

        public vm_GameInterface()
        {
            pr_SizeChangeCommand = new RelayCommand(param => F_ChangeUserControlSize());
            pr_LoadedSizeCommand = new RelayCommand(param => F_ChangeUserControlSize());
            pr_SurrenderCommand = new RelayCommand(param => F_Surrender(this));


            pr_OwnUsername = "Mein Username";
            pr_OwnShipsRemaining = "Remaining Ships: ";
            pr_OpponentUsername = "Gegner Username";
            pr_OpponentShipsRemaining = "Remaining Ships: ";
        }

        // FUNCTIONS

        public void F_ChangeUserControlSize()
        {
            pu_ScaleWidth = ViewModels.vm_MainInterface.pu_uc_GameContent.ActualWidth / pr_GameFieldRequiredWidth;
            pu_ScaleHeight = ViewModels.vm_MainInterface.pu_uc_GameContent.ActualHeight / pr_GameFieldRequiredHeight;
        }

        private void F_Surrender(object source)
        {
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