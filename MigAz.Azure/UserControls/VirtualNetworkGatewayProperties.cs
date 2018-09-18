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
    public partial class VirtualNetworkGatewayProperties : TargetPropertyControl
    {
        VirtualNetworkGateway _VirtualNetworkGateway;

        public VirtualNetworkGatewayProperties()
        {
            InitializeComponent();
        }


        internal void Bind(VirtualNetworkGateway virtualNetworkGateway, TargetTreeView targetTreeView)
        {
            try
            {
                this.IsBinding = true;
                _VirtualNetworkGateway = virtualNetworkGateway;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = _VirtualNetworkGateway.TargetName;
            }
            finally
            {
                this.IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _VirtualNetworkGateway.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            this.RaisePropertyChangedEvent(_VirtualNetworkGateway);
        }
    }
}
