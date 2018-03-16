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
    public partial class VirtualNetworkGatewayProperties : UserControl
    {
        VirtualNetworkGateway _VirtualNetworkGateway;
        TargetTreeView _TargetTreeView;
        bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public VirtualNetworkGatewayProperties()
        {
            InitializeComponent();
        }


        internal void Bind(VirtualNetworkGateway virtualNetworkGateway, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                _VirtualNetworkGateway = virtualNetworkGateway;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = _VirtualNetworkGateway.TargetName;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _VirtualNetworkGateway.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }
    }
}
