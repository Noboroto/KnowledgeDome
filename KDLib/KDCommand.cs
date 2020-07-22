using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDLib
{
    public class KDCommand
    {
        public MachineType Machine { get; set; }

        public CommandType PrefixCmd { get; set; }

        public string Content { get; set; }

        public int ID { get; set; }

        public KDCommand (CommandType prefix, string cmd = "")
        {
            Machine = Data.ThisMacineType;
            PrefixCmd = prefix;
            ID = Data.ID;
            Content = cmd;
        }

        [JsonConstructor]
        public KDCommand(MachineType type, CommandType prefix, int id, string cmd = "")
        {
            Machine = type;
            PrefixCmd = prefix;
            Content = cmd;
            ID = id;
        }
    }
}
