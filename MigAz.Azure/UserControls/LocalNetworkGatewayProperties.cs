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
    public partial class LocalNetworkGatewayProperties : UserControl
    {
        LocalNetworkGateway _LocalNetworkGateway;
        TargetTreeView _TargetTreeView;
        bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public LocalNetworkGatewayProperties()
        {
            InitializeComponent();
        }

        internal void Bind(LocalNetworkGateway localNetworkGateway, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                _LocalNetworkGateway = localNetworkGateway;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = _LocalNetworkGateway.TargetName;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _LocalNetworkGateway.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }
    }
}
