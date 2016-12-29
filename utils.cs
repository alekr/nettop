using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nettop
{
    class Utils
    {
        public static UInt64 TwoToPowerOf(int power)
        {
            UInt64 result = 1;

            while (power > 0)
            {
                result *= 2;
                power--;
            }

            return result;
        }

        public static UInt32 IPAddressStrToInt(string ipAddr)
        {
            if (String.IsNullOrEmpty(ipAddr) || String.IsNullOrWhiteSpace(ipAddr))
                return 0;

            string[] items = ipAddr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != 4)
                return 0;

            UInt32 addr32 = (Convert.ToUInt32(items[0]) << 24) +
                            (Convert.ToUInt32(items[1]) << 16) +
                            (Convert.ToUInt32(items[2]) << 8) +
                             Convert.ToUInt32(items[3]);

            return addr32;
        }

        public static string IpAddressIntToStr(UInt32 ipAddr)
        {
            UInt32 i1 = (ipAddr & 0xFF000000) >> 24;
            UInt32 i2 = (ipAddr & 0x00FF0000) >> 16;
            UInt32 i3 = (ipAddr & 0x0000FF00) >> 8;
            UInt32 i4 = (ipAddr & 0x000000FF);

            return i1.ToString() + "." +
                   i2.ToString() + "." +
                   i3.ToString() + "." +
                   i4.ToString();
        }
    }
}
