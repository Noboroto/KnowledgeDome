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
        private Configuration config;

        public string IP
        {
            get
            {
                return (config.AppSettings.Settings["IP"] == null) ? "" : config.AppSettings.Settings["IP"].Value.ToString();
            }
            set
            {
                if (config.AppSettings.Settings["IP"] == null) config.AppSettings.Settings.Add("IP", value);
                else config.AppSettings.Settings["ID"].Value = value;
                config.Save();
            }
        }
        public ConfigurationSettings()
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
    }
}
