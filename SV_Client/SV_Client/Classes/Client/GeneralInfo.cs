using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SV_Client.Classes.Client
{
    public class GeneralInfo
    {
        private static string pr_Username;
        public static string pu_Username
        {
            get { return GeneralInfo.pr_Username; }
            set { GeneralInfo.pr_Username = value; }
        }

        private static string pr_EnemyUsername;
        public static string pu_EnemyUsername
        {
            get { return GeneralInfo.pr_EnemyUsername; }
            set { GeneralInfo.pr_EnemyUsername = value; }
        }

        private static string pr_Password;
        public static string pu_Password
        {
            get { return GeneralInfo.pr_Password; }
            set { GeneralInfo.pr_Password = value; }
        }

        private static IPAddress pr_ServerIP;
        public static IPAddress pu_ServerIP
        {
            get { return GeneralInfo.pr_ServerIP; }
            set { GeneralInfo.pr_ServerIP = value; }
        }

        public GeneralInfo()
        {

        }
    }
}
