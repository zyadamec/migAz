using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class VirtualNetworkProperties : UserControl
    {
        private TargetTreeView _TargetTreeView;
        private VirtualNetwork _VirtualNetwork;
        private bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public VirtualNetworkProperties()
        {
            InitializeComponent();
        }

        public void Bind(VirtualNetwork targetVirtualNetwork, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                _VirtualNetwork = targetVirtualNetwork;
                _TargetTreeView = targetTreeView;

                if (targetVirtualNetwork.SourceVirtualNetwork != null)
                {
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
                }
                else
                    lblVNetName.Text = "(None)";

                txtVirtualNetworkName.Text = targetVirtualNetwork.TargetName;
                dgvAddressSpaces.DataSource = targetVirtualNetwork.AddressPrefixes.Select(x => new { AddressPrefix = x }).ToList();
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _VirtualNetwork.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings); ;

            if (!_IsBinding)
                PropertyChanged?.Invoke();
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
