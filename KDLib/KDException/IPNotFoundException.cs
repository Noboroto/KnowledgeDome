using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDLib.KDException
{
    public class IPNotFoundException : Exception
    {
        public static string message = "Không tìm thấy máy chủ";
        public IPNotFoundException() : base(message)
        {

        }
    }
}
