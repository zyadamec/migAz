// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Core;
using MigAz.Azure.MigrationTarget;
using MigAz.Azure.Core.Interface;

namespace MigAz.Azure.UserControls
{
    public partial class LoadBalancerProperties : TargetPropertyControl
    {
        private LoadBalancer _LoadBalancer;

        public LoadBalancerProperties()
        {
            InitializeComponent();
            this.publicIpSelectionControl1.PropertyChanged += PublicIpSelectionControl1_PropertyChanged;
        }


        private void NetworkSelectionControl1_PropertyChanged()
        {
            this.RaisePropertyChangedEvent(_LoadBalancer);
        }

        internal async Task Bind(LoadBalancer loadBalancer, TargetTreeView targetTreeView)
        {
            try
            {
                this.IsBinding = true;
                _LoadBalancer = loadBalancer;
                _TargetTreeView = targetTreeView;
                networkSelectionControl1.PropertyChanged += NetworkSelectionControl1_PropertyChanged;

                await networkSelectionControl1.Bind(_TargetTreeView);

                cmbLoadBalancerType.SelectedIndex = cmbLoadBalancerType.FindString(loadBalancer.LoadBalancerType.ToString());
                txtTargetName.Text = loadBalancer.TargetName;
            }
            finally
            {
                this.IsBinding = false;
            }
        }

        public MigrationTarget.LoadBalancer LoadBalancer
        {
            get { return _LoadBalancer; }
        }


        private void PublicIpSelectionControl1_PropertyChanged()
        {
            _LoadBalancer.FrontEndIpConfigurations[0].PublicIp = this.publicIpSelectionControl1.PublicIp;
            this.RaisePropertyChangedEvent(_LoadBalancer);
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _LoadBalancer.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            this.RaisePropertyChangedEvent(_LoadBalancer);
        }

        private async void cmbLoadBalancerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLoadBalancerType.SelectedItem.ToString() == "Public")
            {
                await this.publicIpSelectionControl1.Bind(_TargetTreeView);
                this.publicIpSelectionControl1.PublicIp = _LoadBalancer.FrontEndIpConfigurations[0].PublicIp;

                _LoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Public;
                this.networkSelectionControl1.Enabled = false;
                this.networkSelectionControl1.Visible = false;
                this.pnblPublicProperties.Visible = true;
            }
            else
            {
                _LoadBalancer.LoadBalancerType = MigrationTarget.LoadBalancerType.Internal;
                this.networkSelectionControl1.Enabled = true;
                this.networkSelectionControl1.Visible = true;
                this.pnblPublicProperties.Visible = false;

                if (_LoadBalancer.FrontEndIpConfigurations.Count > 0)
                {
                    await networkSelectionControl1.Bind(_TargetTreeView);
                    networkSelectionControl1.VirtualNetworkTarget = _LoadBalancer.FrontEndIpConfigurations[0];
                }
            }

            this.RaisePropertyChangedEvent(_LoadBalancer);
        }
    }
}

