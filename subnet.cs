using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nettop
{
    public class Subnet
    {
        private string cidr = null;
        private string name = null;
         
        public Subnet (string cidr)
        {
            CIDRMask = 0;
            CIDR = cidr;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Category("CIDR Notation")]
        [Description("Classless Inter-Domain Routing (CIDR) signature of this subnet")]
        public string CIDR
        {
            get { return cidr; }
            set { SetCIDR(value); }
        }

        [Description("Base IP address as entered in CIDR notation (may not be the same as the first usable IP address if CIDR property is not correctly specified")]
        [Category("CIDR Notation")]
        public string CIDRBase
        {
            get;
            private set;
        }

        [Description("Network mask in CIDR notation (the number after the '/' character)")]
        [Category("CIDR Notation")]
        public int CIDRMask
        {
            get;
            private set;
        }

        [Description("Network mask in standard dotted notation")]
        [Category("Dotted Notation")]
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

        [Description("First (lowest) usable IP address in the subnet range")]
        [Category("Dotted Notation")]
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

        [Description("Last (highest) usable IP address in the subnet range")]
        [Category("Dotted Notation")]
        public string LastUsable
        {
            get
            {
                if (cidr == null)
                    return null;

                UInt32 addr32 = Utils.IPAddressStrToInt(FirstUsable);
                UInt64 networkSize = Utils.TwoToPowerOf(32 - CIDRMask);

                addr32 += (UInt32)(networkSize - 1);

                return Utils.IpAddressIntToStr(addr32);
            }
        }

        [Description("Number of IP addresses in the subnet")]
        [Category("General Info")]
        public UInt64 Size
        {
            get
            {
                if (cidr == null)
                    return 0;

                return Utils.TwoToPowerOf(32 - CIDRMask);
            }
        }

        [Description("Descriptive name of the subnet")]
        [Category("General Info")]
        public string Name
        {
            get
            {
                if (name == null)
                    name = CIDR;

                return name;
            }
            set
            {
                name = value;
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
            Boolean autoName = (cidr == name);

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
            if ((mask < 0) || (mask > 32))
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

            if ((mask == 0) && (items[0] != "0.0.0.0"))
                return;

            CIDRBase = items[0];
            CIDRMask = mask;

            if (mask == 0)
                NetMaskInt = 0xFFFFFFFF;
            else
                NetMaskInt = 0xFFFFFFFF << (32 - CIDRMask);

            NetMask = Utils.IpAddressIntToStr(NetMaskInt);

            cidr = value;

            if (autoName)
                name = value;
        }
    }
}
