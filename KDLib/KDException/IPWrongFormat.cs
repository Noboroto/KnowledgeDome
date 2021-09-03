using System;

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
