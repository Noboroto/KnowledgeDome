using System;

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
