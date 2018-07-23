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
using MigAz.Core.Interface;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class NetworkInterfaceProperties : TargetPropertyControl
    {
        private Azure.MigrationTarget.NetworkInterface _TargetNetworkInterface;

        public NetworkInterfaceProperties()
        {
            InitializeComponent();
        }

        private void NetworkSelectionControl1_PropertyChanged()
        {
            this.RaisePropertyChangedEvent();
        }

        internal async Task Bind(NetworkInterface targetNetworkInterface, TargetTreeView targetTreeView)
        {
            try
            {
                this.IsBinding = true;
                _TargetTreeView = targetTreeView;
                _TargetNetworkInterface = targetNetworkInterface;
                networkSelectionControl1.PropertyChanged += NetworkSelectionControl1_PropertyChanged;

                await networkSelectionControl1.Bind(targetTreeView);

                if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                {
                    networkSelectionControl1.VirtualNetworkTarget = _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0];
                }

                lblSourceName.Text = _TargetNetworkInterface.SourceName;
                txtTargetName.Text = _TargetNetworkInterface.TargetName;

                if (_TargetNetworkInterface.EnableIPForwarding)
                    rbIPForwardingEnabled.Checked = true;
                else
                    rbIPForwardingDisabled.Checked = true;

                if (_TargetNetworkInterface.EnableAcceleratedNetworking)
                    rbAcceleratedNetworkingEnabled.Checked = true;
                else
                    rbAcceleratedNetworkingDisabled.Checked = true;

                if (_TargetNetworkInterface.SourceNetworkInterface != null)
                {
                    if (_TargetNetworkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Asm.NetworkInterface))
                    {
                        Azure.Asm.NetworkInterface asmNetworkInterface = (Azure.Asm.NetworkInterface)_TargetNetworkInterface.SourceNetworkInterface;

                        lblVirtualNetworkName.Text = asmNetworkInterface.NetworkInterfaceIpConfigurations[0].VirtualNetworkName;
                        lblSubnetName.Text = asmNetworkInterface.NetworkInterfaceIpConfigurations[0].SubnetName;
                        lblStaticIpAddress.Text = asmNetworkInterface.NetworkInterfaceIpConfigurations[0].PrivateIpAddress;
                    }
                    else if (_TargetNetworkInterface.SourceNetworkInterface.GetType() == typeof(Azure.Arm.NetworkInterface))
                    {
                        Azure.Arm.NetworkInterface armNetworkInterface = (Azure.Arm.NetworkInterface)_TargetNetworkInterface.SourceNetworkInterface;

                        lblVirtualNetworkName.Text = armNetworkInterface.NetworkInterfaceIpConfigurations[0].VirtualNetworkName;
                        lblSubnetName.Text = armNetworkInterface.NetworkInterfaceIpConfigurations[0].SubnetName;
                        lblStaticIpAddress.Text = armNetworkInterface.NetworkInterfaceIpConfigurations[0].PrivateIpAddress;
                    }
                }

                virtualMachineSummary.Bind(_TargetNetworkInterface.ParentVirtualMachine, _TargetTreeView);
                networkSecurityGroup.Bind(_TargetNetworkInterface.NetworkSecurityGroup, _TargetTreeView);

                if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count() > 0)
                    resourceSummaryPublicIp.Bind(_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPublicIp, _TargetTreeView);

                this.UpdatePropertyEnablement();
            }
            finally
            {
                this.IsBinding = false;
            }
        }

        internal override void UpdatePropertyEnablement()
        {
            this.pnlAcceleratedNetworking.Enabled = _TargetNetworkInterface.AllowAcceleratedNetworking;
            this.pnlAcceleratedNetworking.Visible = _TargetNetworkInterface.AllowAcceleratedNetworking;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetNetworkInterface.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            this.RaisePropertyChangedEvent();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void rbIPForwardingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIPForwardingEnabled.Checked)
            {
                _TargetNetworkInterface.EnableIPForwarding = true;

                this.RaisePropertyChangedEvent();
            }
        }

        private void rbIPForwardingDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (rbIPForwardingDisabled.Checked)
            {
                _TargetNetworkInterface.EnableIPForwarding = false;

                this.RaisePropertyChangedEvent();
            }
        }

        private void rbAcceleratedNetworkingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAcceleratedNetworkingEnabled.Checked)
            {
                _TargetNetworkInterface.EnableAcceleratedNetworking = true;

                this.RaisePropertyChangedEvent();
            }
        }

        private void rbAcceleratedNetworkingDisabled_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAcceleratedNetworkingDisabled.Checked)
            {
                _TargetNetworkInterface.EnableAcceleratedNetworking = false;

                this.RaisePropertyChangedEvent();
            }
        }
    }
}

