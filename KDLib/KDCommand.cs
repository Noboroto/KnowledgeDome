using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KDLib
{
    public class KDCommand
    {
        #region PrivateMembers
        #endregion

        #region PublicProperties
        public MachineType Machine { get; set; }

        public CommandType PrefixCmd { get; set; }

        public string Content { get; set; }

        public EndPoint ID { get; set; }
        #endregion

        public KDCommand (CommandType prefix, EndPoint local, string cmd = "")
        : this (Data.ThisMacineType, prefix, local, cmd)
        {
        }

        [JsonConstructor]
        public KDCommand(MachineType type, CommandType prefix, EndPoint local, string cmd = "")
        {
            Machine = type;
            PrefixCmd = prefix;
            Content = cmd;
            ID = local;
        }
    }
}
