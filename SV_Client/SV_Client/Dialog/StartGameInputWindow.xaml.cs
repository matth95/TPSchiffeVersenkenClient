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
using System.Windows.Shapes;

namespace SV_Client.Dialog
{
    /// <summary>
    /// Interaction logic for StartGameInputWindow.xaml
    /// </summary>
    public partial class StartGameInputWindow : Window, INotifyPropertyChanged
    {
        private static string pr_InputGameName;
        public static string pu_InputGameName
        {
            get { return pr_InputGameName; }
            set { pr_InputGameName = value; }
        }

        public StartGameInputWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
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
