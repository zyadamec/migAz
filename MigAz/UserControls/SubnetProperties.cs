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

            Azure.MigrationTarget.Subnet targetSubnet = (Azure.MigrationTarget.Subnet)_AsmSubnetNode.Tag;
            txtTargetName.Text = targetSubnet.TargetName;

            if (targetSubnet.Source.GetType() == typeof(Azure.Asm.Subnet))
            {
                Azure.Asm.Subnet asmSubnet = (Azure.Asm.Subnet)targetSubnet.Source;

                lblSourceName.Text = asmSubnet.Name;
                lblAddressSpace.Text = asmSubnet.AddressPrefix;
            }
            else if (targetSubnet.Source.GetType() == typeof(Azure.Arm.Subnet))
            {
                Azure.Arm.Subnet armSubnet = (Azure.Arm.Subnet)targetSubnet.Source;

                lblSourceName.Text = armSubnet.Name;
                lblAddressSpace.Text = armSubnet.AddressPrefix;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;


            Azure.MigrationTarget.Subnet targetSubnet = (Azure.MigrationTarget.Subnet)_AsmSubnetNode.Tag;
            targetSubnet.TargetName = txtSender.Text;
            _AsmSubnetNode.Text = targetSubnet.GetFinalTargetName();

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
