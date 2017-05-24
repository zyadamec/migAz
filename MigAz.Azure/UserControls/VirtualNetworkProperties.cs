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

namespace MigAz.Azure.UserControls
{
    public partial class VirtualNetworkProperties : UserControl
    {
        TreeNode _ARMVirtualNetworkNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public VirtualNetworkProperties()
        {
            InitializeComponent();
        }

        public void Bind(TreeNode armVirtualNetworkNode)
        {
            _ARMVirtualNetworkNode = armVirtualNetworkNode;

            Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)_ARMVirtualNetworkNode.Tag;

            if (targetVirtualNetwork.SourceVirtualNetwork.GetType() == typeof(Azure.Asm.VirtualNetwork))
            {
                Azure.Asm.VirtualNetwork asmVirtualNetwork = (Azure.Asm.VirtualNetwork)targetVirtualNetwork.SourceVirtualNetwork;
                lblVNetName.Text = asmVirtualNetwork.Name;
            }
            else if (targetVirtualNetwork.SourceVirtualNetwork.GetType() == typeof(Azure.Arm.VirtualNetwork))
            {
                Azure.Arm.VirtualNetwork armVirtualNetwork = (Azure.Arm.VirtualNetwork)targetVirtualNetwork.SourceVirtualNetwork;
                lblVNetName.Text = armVirtualNetwork.Name;
            }

            txtVirtualNetworkName.Text = targetVirtualNetwork.TargetName;
            dgvAddressSpaces.DataSource = targetVirtualNetwork.AddressPrefixes.Select(x => new { AddressPrefix = x }).ToList();
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)_ARMVirtualNetworkNode.Tag;

            targetVirtualNetwork.TargetName = txtSender.Text.Trim();
            _ARMVirtualNetworkNode.Text = targetVirtualNetwork.ToString();

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
