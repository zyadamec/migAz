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
    public partial class VirtualNetworkConnectionProperties : TargetPropertyControl
    {
        VirtualNetworkGatewayConnection _VirtualNetworkGatewayConnection;

        public VirtualNetworkConnectionProperties()
        {
            InitializeComponent();
        }

        internal void Bind(VirtualNetworkGatewayConnection virtualNetworkGatewayConnection, TargetTreeView targetTreeView)
        {
            try
            {
                this.IsBinding = true;
                _VirtualNetworkGatewayConnection = virtualNetworkGatewayConnection;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = _VirtualNetworkGatewayConnection.TargetName;

            }
            finally
            {
                this.IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _VirtualNetworkGatewayConnection.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            this.RaisePropertyChangedEvent();
        }
    }
}
