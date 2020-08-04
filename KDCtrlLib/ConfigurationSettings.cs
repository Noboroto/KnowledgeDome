using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDCtrlLib
{
    public class ConfigurationSettings : ViewModelBase
    {
        private static Configuration config;

        public static string IP
        {
            get
            {
                return (config.AppSettings.Settings["IP"] == null) ? "" : config.AppSettings.Settings["IP"].Value.ToString();
            }
            set
            {
                if (config != null)
                {
                    if (config.AppSettings.Settings["IP"] == null) config.AppSettings.Settings.Add("IP", value);
                    else config.AppSettings.Settings["IP"].Value = value;
                }
            }
        }
        public ConfigurationSettings()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public static void Save()
        {
            config.Save();
        }
    }
}
