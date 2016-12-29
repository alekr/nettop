using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nettop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Subnet subnet = new Subnet(textBox1.Text);

            DisplaySubnet(subnet);
        }

        private  void DisplaySubnet(Subnet subnet)
        {
            textBox2.Text =
                "CIDR         = " + subnet.CIDR + "\r\n" +
                "CIDR Base    = " + subnet.CIDRBase + "\r\n" +
                "CIDR Mask    = " + subnet.CIDRMask + "\r\n" +
                "NetMask      = " + subnet.NetMask + "\r\n" +
                "First Usable = " + subnet.FirstUsable + "\r\n" +
                "Last Usable  = " + subnet.LastUsable + "\r\n" +
                "Size         = " + subnet.Size.ToString();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Subnet subnet = userControlSubnetEditor1.Subnet;

            subnet.CIDR = "10.4.1.0/24";

            DisplaySubnet(subnet);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //userControlSubnetEditor1.Subnet.CIDR = "0.0.0.0/0";
            userControlSubnetEditor1.Reload();
        }
    }
}
