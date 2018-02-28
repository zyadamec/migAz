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
    public partial class NetworkInterfaceProperties : UserControl
    {
        private TargetTreeView _TargetTreeView;
        private Azure.MigrationTarget.NetworkInterface _TargetNetworkInterface;
        private bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkInterfaceProperties()
        {
            InitializeComponent();
        }

        private void NetworkSelectionControl1_PropertyChanged()
        {
            if (!_IsBinding)
                PropertyChanged?.Invoke(); // bubble event
        }

        internal async Task Bind(NetworkInterface targetNetworkInterface, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
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
            }
            finally
            {
                _IsBinding = false;
            }
        }


        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetNetworkInterface.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

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

        private void rbIPForwardingEnabled_CheckedChanged(object sender, EventArgs e)
        {
            _TargetNetworkInterface.EnableIPForwarding = true;

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private void rbIPForwardingDisabled_CheckedChanged(object sender, EventArgs e)
        {
            _TargetNetworkInterface.EnableIPForwarding = false;

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }
    }
}

