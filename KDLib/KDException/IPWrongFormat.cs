using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDLib.KDException
{
    public class IPWrongFormat : FormatException
    {
        public static string message = "Sai định dạng IP";
        public IPWrongFormat() : base(message)
        {
        }
    }
}
