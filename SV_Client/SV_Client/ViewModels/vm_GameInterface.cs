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

        public static int pustat_AmountsOfEnemyShips = 6;

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

            pr_OwnUsername = SV_Client.Classes.Client.GeneralInfo.pu_Username;
            pr_OpponentUsername = SV_Client.Classes.Client.GeneralInfo.pu_EnemyUsername;
            pr_OpponentShipsRemaining = "Remaining Ships: " + pustat_AmountsOfEnemyShips;
        }

        // FUNCTIONS

        /// <summary>
        /// This function is called whenever the size of the window is changed.
        /// It updates the scaling values for width and height.
        /// </summary>
        public void F_ChangeUserControlSize()
        {
            pu_ScaleWidth = ViewModels.vm_MainInterface.pu_uc_GameContent.ActualWidth / pr_GameFieldRequiredWidth;
            pu_ScaleHeight = ViewModels.vm_MainInterface.pu_uc_GameContent.ActualHeight / pr_GameFieldRequiredHeight;
        }

        public void F_EnemyShipDestroyed()
        {
            pustat_AmountsOfEnemyShips--;
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