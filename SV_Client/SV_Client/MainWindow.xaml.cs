using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SV_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private UserControl pr_ActiveContent;

        public UserControl pu_ActiveContent
        {
            get { return pr_ActiveContent; }
            set
            {
                pr_ActiveContent = value;
                NotifyChange("pu_ActiveContent");
            }
        }

        private UserControls.uc_MainInterface pr_uc_MainInterface;

        public MainWindow()
        {
            pu_ActiveContent = new UserControl();
            pr_uc_MainInterface = new UserControls.uc_MainInterface();

            InitializeComponent();
            DataContext = this;

            pu_ActiveContent = pr_uc_MainInterface;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChange(string ChangedComponent)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(ChangedComponent));
            }
        }
    }
}