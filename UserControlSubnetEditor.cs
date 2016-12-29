using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nettop
{
    public partial class UserControlSubnetEditor : UserControl
    {
        private Subnet subnet = new Subnet("0.0.0.0/0");

        public UserControlSubnetEditor()
        {
            InitializeComponent();

            propertyGrid1.SelectedObject = subnet;
        }

        public Subnet Subnet { get { return subnet; } }

        public void Reload()
        {
            propertyGrid1.SelectedObject = subnet;
        }
    }
}
