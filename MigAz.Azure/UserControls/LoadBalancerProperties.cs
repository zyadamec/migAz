using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class LoadBalancerProperties : UserControl
    {
        private AzureContext _AzureContext;
        private TargetTreeView _TargetTreeView;
        private LoadBalancer _LoadBalancer;
        private bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public LoadBalancerProperties()
        {
            InitializeComponent();
        }

        private void NetworkSelectionControl1_PropertyChanged()
        {
            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        internal async Task Bind(AzureContext azureContext, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                networkSelectionControl1.PropertyChanged += NetworkSelectionControl1_PropertyChanged;
                _AzureContext = azureContext;
                _TargetTreeView = targetTreeView;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        public MigrationTarget.LoadBalancer LoadBalancer
        {
            get { return _LoadBalancer; }
            set
            {
                _LoadBalancer = value;

                cmbLoadBalancerType.SelectedIndex = cmbLoadBalancerType.FindString(_LoadBalancer.LoadBalancerType.ToString());
                txtTargetName.Text = _LoadBalancer.TargetName;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _LoadBalancer.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private async void cmbLoadBalancerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLoadBalancerType.SelectedItem.ToString() == "Public")
            {
                _LoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Public;
                this.networkSelectionControl1.Enabled = false;
                this.networkSelectionControl1.Visible = false;
                this.pnblPublicProperties.Visible = true;

                if (_LoadBalancer.FrontEndIpConfigurations.Count > 0)
                    resourceSummaryPublicIp.Bind(_LoadBalancer.FrontEndIpConfigurations[0].PublicIp, _TargetTreeView);
            }
            else
            {
                _LoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Internal;
                this.networkSelectionControl1.Enabled = true;
                this.networkSelectionControl1.Visible = true;
                this.pnblPublicProperties.Visible = false;

                if (_LoadBalancer.FrontEndIpConfigurations.Count > 0)
                {
                    await networkSelectionControl1.Bind(_AzureContext, _TargetTreeView, _TargetTreeView.GetVirtualNetworksInMigration());
                    networkSelectionControl1.VirtualNetworkTarget = _LoadBalancer.FrontEndIpConfigurations[0];
                }
            }

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }
    }
}
