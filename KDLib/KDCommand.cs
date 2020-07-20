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
        private MachineType _Machine;

        private CommandType _PrefixCmd;

        private string _Content;

        public MachineType Machine
        {
            get
            {
                return _Machine;
            }
            set
            {
                _Machine = (MachineType)value;
            }
        }

        public CommandType PrefixCmd
        {
            get
            {
                return _PrefixCmd;
            }
            set
            {
                _PrefixCmd = (CommandType)value;
            }
        }

        public string Content
        {
            get
            {
                return _Content;
            }
            set
            {
                _Content = value;
            }
        }

        public KDCommand (CommandType prefix, string cmd = "")
        {
            _Machine = Data.ThisMacineType;
            _PrefixCmd = prefix;
            _Content = cmd;
        }

        [JsonConstructor]
        public KDCommand(MachineType type, CommandType prefix, string cmd = "")
        {
            _Machine = type;
            _PrefixCmd = prefix;
            _Content = cmd;
        }
    }
}
