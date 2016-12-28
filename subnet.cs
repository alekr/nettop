using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nettop
{
    class Subnet
    {
        private string cidr = null;
         
        public Subnet (string cidr)
        {
            CIDRMask = 0;
            CIDR = cidr;
        }

        public string CIDR
        {
            get { return cidr; }
            private set { SetCIDR(value); }
        }

        public string CIDRBase
        {
            get;
            private set;
        }

        public int CIDRMask
        {
            get;
            private set;
        }

        public string NetMask
        {
            get;
            private set;
        }

        private UInt32 NetMaskInt
        {
            get;
            set;
        }

        public string FirstUsable
        {
            get
            {
                if (cidr == null)
                    return null;

                UInt32 addr32 = Utils.IPAddressStrToInt(CIDRBase);

                return Utils.IpAddressIntToStr(addr32 & NetMaskInt);
            }
        }

        public string LastUsable
        {
            get
            {
                if (cidr == null)
                    return null;

                UInt32 addr32 = Utils.IPAddressStrToInt(FirstUsable);
                UInt32 networkSize = Utils.TwoToPowerOf(32 - CIDRMask);

                addr32 += (networkSize - 1);

                return Utils.IpAddressIntToStr(addr32);
            }
        }

        public UInt32 Size
        {
            get
            {
                if (cidr == null)
                    return 0;

                return Utils.TwoToPowerOf(32 - CIDRMask);
            }
        }

        public Boolean IsSubnetOf(Subnet net)
        {
            UInt32 firstUsableInt = Utils.IPAddressStrToInt(FirstUsable);
            UInt32 netFirstUsableInt = Utils.IPAddressStrToInt(net.FirstUsable);

            if (firstUsableInt < netFirstUsableInt)
                return false;

            UInt32 lastUsableInt = Utils.IPAddressStrToInt(LastUsable);
            UInt32 netLastUsableInt = Utils.IPAddressStrToInt(net.LastUsable);

            if (lastUsableInt > netLastUsableInt)
                return false;

            return true;
        }

        private void SetCIDR(string value)
        {
            if (value == null)
                return;

            value = value.Trim();
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
                return;

            int mask = -1;

            string[] items = value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length != 2)
                return;

            mask = Convert.ToInt32(items[1]);
            if ((mask < 1) || (mask > 32))
                return;

            string[] addressParts = items[0].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (addressParts.Length != 4)
                return;

            foreach (string addressPart in addressParts)
            {
                int i = Convert.ToInt32(addressPart);
                if ((i < 0) || (i > 255))
                    return;
            }

            CIDRBase = items[0];
            CIDRMask = mask;

            NetMaskInt = 0xFFFFFFFF << (32 - CIDRMask);
            NetMask = Utils.IpAddressIntToStr(NetMaskInt);

            cidr = value;
        }
    }
}
