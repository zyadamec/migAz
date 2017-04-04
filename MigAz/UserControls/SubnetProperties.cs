using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Asm;

namespace MigAz.UserControls
{
    public partial class SubnetProperties : UserControl
    {
        private TreeNode _AsmSubnetNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public SubnetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TreeNode asmSubnetNode)
        {
            _AsmSubnetNode = asmSubnetNode;

            Subnet asmSubnet = (Subnet)_AsmSubnetNode.Tag;

            lblSourceName.Text = asmSubnet.Name;
            lblAddressSpace.Text = asmSubnet.AddressPrefix;
            txtTargetName.Text = asmSubnet.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;
            Subnet asmSubnet = (Subnet)_AsmSubnetNode.Tag;

            asmSubnet.TargetName = txtSender.Text;
            _AsmSubnetNode.Text = asmSubnet.TargetName;

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
