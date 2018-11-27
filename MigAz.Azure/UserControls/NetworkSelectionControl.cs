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
using MigAz.Azure.Core.Interface;
using MigAz.Azure.Interface;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class NetworkSelectionControl : UserControl
    {
        private IVirtualNetworkTarget _NetworkInterfaceTarget;
        private Azure.UserControls.TargetTreeView _TargetTreeView;
        private bool _IsBinding = false;

        public delegate void AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkSelectionControl()
        {
            InitializeComponent();
            txtStaticIp.TextChanged += txtStaticIp_TextChanged;
        }

        public async Task Bind(TargetTreeView targetTreeView)
        {
            _TargetTreeView = targetTreeView;

            try
            {
                _IsBinding = true;

                if (_TargetTreeView.TargetResourceGroup != null && _TargetTreeView.TargetResourceGroup.TargetLocation != null)
                {
                    int existingVNetCount = targetTreeView.GetExistingVirtualNetworksInTargetLocation().Count();

                    rbExistingARMVNet.Text = "Existing VNet(s) in " + _TargetTreeView.TargetResourceGroup.TargetLocation.DisplayName;
                    if (existingVNetCount == 0)
                        rbExistingARMVNet.Text = "No " + rbExistingARMVNet.Text;

                    this.ExistingARMVNetEnabled = existingVNetCount > 0;
                }
                else
                {
                    // Cannot use existing ARM VNet without Target Location
                    rbExistingARMVNet.Enabled = false;
                    rbExistingARMVNet.Text = "<Set Target Resource Group Location>";
                }
            }
            catch (Exception exc)
            {
                targetTreeView.LogProvider.WriteLog("VirtualMachineProperties.Bind", exc.Message);
                this.ExistingARMVNetEnabled = false;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        public IVirtualNetworkTarget VirtualNetworkTarget
        {
            get { return _NetworkInterfaceTarget; }
            set
            {
                _NetworkInterfaceTarget = value;

                try
                {
                    _IsBinding = true;

                    if (_NetworkInterfaceTarget != null)
                    {
                        if (this.ExistingARMVNetEnabled == false ||
                            _NetworkInterfaceTarget == null ||
                            _NetworkInterfaceTarget.TargetSubnet == null ||
                            _NetworkInterfaceTarget.TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)
                            )
                        {
                            this.SelectVNetInMigration();
                        }
                        else
                        {
                            this.SelectExistingARMVNet();
                        }

                        if (_NetworkInterfaceTarget != null)
                        {
                            cmbAllocationMethod.SelectedIndex = cmbAllocationMethod.FindString(_NetworkInterfaceTarget.TargetPrivateIPAllocationMethod.ToString());
                        }
                    }
                }
                finally
                {
                    _IsBinding = false;
                }
            }
        }

        public bool ExistingARMVNetEnabled
        {
            get { return rbExistingARMVNet.Enabled; }
            set { rbExistingARMVNet.Enabled = value; }
        }

        public void SelectVNetInMigration()
        {
            rbVNetInMigration.Checked = true;
        }

        public void SelectExistingARMVNet()
        {
            rbExistingARMVNet.Checked = true;
        }

        private async void cmbVirtualNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSubnet.Items.Clear();
            if (cmbVirtualNetwork.SelectedItem != null)
            {
                if (cmbVirtualNetwork.SelectedItem.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    Azure.MigrationTarget.VirtualNetwork selectedNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbVirtualNetwork.SelectedItem;

                    foreach (Azure.MigrationTarget.Subnet subnet in selectedNetwork.TargetSubnets)
                    {
                        if (!subnet.IsGatewaySubnet)
                            cmbSubnet.Items.Add(subnet);
                    }
                }
                else if (cmbVirtualNetwork.SelectedItem.GetType() == typeof(Azure.Arm.VirtualNetwork))
                {
                    Azure.Arm.VirtualNetwork selectedNetwork = (Azure.Arm.VirtualNetwork)cmbVirtualNetwork.SelectedItem;

                    foreach (Azure.Arm.Subnet subnet in selectedNetwork.Subnets)
                    {
                        if (!subnet.IsGatewaySubnet)
                            cmbSubnet.Items.Add(subnet);
                    }

                }

                if (cmbSubnet.SelectedIndex == -1 && cmbSubnet.Items.Count > 0)
                    cmbSubnet.SelectedIndex = 0;
            }
        }

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbVirtualNetwork.Items.Clear();
                cmbSubnet.Items.Clear();

                foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _TargetTreeView.GetVirtualNetworksInMigration())
                {
                    cmbVirtualNetwork.Items.Add(targetVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_NetworkInterfaceTarget != null)
                {
                    if (_NetworkInterfaceTarget.TargetVirtualNetwork != null)
                    {
                        if (_NetworkInterfaceTarget.TargetVirtualNetwork.GetType() == typeof(VirtualNetwork))
                        {
                            VirtualNetwork targetVirtualNetwork = (VirtualNetwork)_NetworkInterfaceTarget.TargetVirtualNetwork;

                            // Attempt to match target to list items
                            foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbVirtualNetwork.Items)
                            {
                                if (listVirtualNetwork == targetVirtualNetwork)
                                {
                                    cmbVirtualNetwork.SelectedItem = listVirtualNetwork;
                                    break;
                                }
                            }

                            if (cmbVirtualNetwork.SelectedItem != null && _NetworkInterfaceTarget.TargetSubnet != null)
                            {
                                if (_NetworkInterfaceTarget.TargetSubnet.GetType() == typeof(Subnet))
                                {
                                    Subnet targetSubnet = (Subnet)_NetworkInterfaceTarget.TargetSubnet;

                                    foreach (Azure.MigrationTarget.Subnet listSubnet in cmbSubnet.Items)
                                    {
                                        if (listSubnet == targetSubnet)
                                        {
                                            cmbSubnet.SelectedItem = listSubnet;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion

                if (cmbVirtualNetwork.SelectedIndex < 0 && cmbVirtualNetwork.Items.Count > 0)
                    cmbVirtualNetwork.SelectedIndex = 0;

                if (cmbSubnet.SelectedIndex < 0 && cmbSubnet.Items.Count > 0)
                    cmbSubnet.SelectedIndex = 0;

                if (!_IsBinding)
                    PropertyChanged?.Invoke();
            }
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {

                #region Add "Existing in Subscription / Location" ARM Virtual Networks to cmbExistingArmVNets

                cmbVirtualNetwork.Items.Clear();
                cmbSubnet.Items.Clear();

                foreach (Arm.VirtualNetwork armVirtualNetwork in _TargetTreeView.GetExistingVirtualNetworksInTargetLocation())
                {
                    if (armVirtualNetwork.HasNonGatewaySubnet)
                        cmbVirtualNetwork.Items.Add(armVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_NetworkInterfaceTarget != null)
                {
                    if (_NetworkInterfaceTarget.GetType() == typeof(NetworkInterfaceIpConfiguration))
                    {
                        NetworkInterfaceIpConfiguration targetNetworkInterface = (NetworkInterfaceIpConfiguration)_NetworkInterfaceTarget;
                        if (targetNetworkInterface.TargetVirtualNetwork != null)
                        {
                            if (targetNetworkInterface.TargetVirtualNetwork.GetType() == typeof(Arm.VirtualNetwork))
                            {
                                Arm.VirtualNetwork targetVirtualNetwork = (Arm.VirtualNetwork)targetNetworkInterface.TargetVirtualNetwork;

                                // Attempt to match target to list items
                                for (int i = 0; i < cmbVirtualNetwork.Items.Count; i++)
                                {
                                    Arm.VirtualNetwork listVirtualNetwork = (Arm.VirtualNetwork)cmbVirtualNetwork.Items[i];
                                    if (listVirtualNetwork == targetVirtualNetwork)
                                    {
                                        cmbVirtualNetwork.SelectedIndex = i;
                                        break;
                                    }
                                }

                                if (targetNetworkInterface.TargetSubnet.GetType() == typeof(Arm.Subnet))
                                {
                                    Arm.Subnet targetSubnet = (Arm.Subnet)targetNetworkInterface.TargetSubnet;

                                    // Attempt to match target to list items
                                    for (int i = 0; i < cmbSubnet.Items.Count; i++)
                                    {
                                        Arm.Subnet listSubnet = (Arm.Subnet)cmbSubnet.Items[i];
                                        if (listSubnet == targetSubnet)
                                        {
                                            cmbSubnet.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                #endregion

                if (cmbVirtualNetwork.SelectedIndex < 0 && cmbVirtualNetwork.Items.Count > 0)
                    cmbVirtualNetwork.SelectedIndex = 0;

                if (cmbSubnet.SelectedIndex < 0 && cmbSubnet.Items.Count > 0)
                    cmbSubnet.SelectedIndex = 0;

                if (!_IsBinding)
                    PropertyChanged?.Invoke();
            }
        }

        private void cmbSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_IsBinding)
            {
                if (_NetworkInterfaceTarget != null)
                {
                    if (cmbSubnet.SelectedItem == null)
                    {
                        _NetworkInterfaceTarget.TargetVirtualNetwork = null;
                        _NetworkInterfaceTarget.TargetSubnet = null;
                    }
                    else
                    {
                        if (cmbSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                        {
                            _NetworkInterfaceTarget.TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbVirtualNetwork.SelectedItem;
                            _NetworkInterfaceTarget.TargetSubnet = (Azure.MigrationTarget.Subnet)cmbSubnet.SelectedItem;
                        }
                        else if (cmbSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                        {
                            _NetworkInterfaceTarget.TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbVirtualNetwork.SelectedItem;
                            _NetworkInterfaceTarget.TargetSubnet = (Azure.Arm.Subnet)cmbSubnet.SelectedItem;
                        }
                    }
                }

                PropertyChanged?.Invoke();
            }
        }

        private void cmbAllocationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_NetworkInterfaceTarget != null)
            {
                if (cmbAllocationMethod.SelectedItem.ToString() == "Static")
                    _NetworkInterfaceTarget.TargetPrivateIPAllocationMethod = IPAllocationMethodEnum.Static;
                else
                    _NetworkInterfaceTarget.TargetPrivateIPAllocationMethod = IPAllocationMethodEnum.Dynamic;

                txtStaticIp.Enabled = _NetworkInterfaceTarget.TargetPrivateIPAllocationMethod == IPAllocationMethodEnum.Static;
                if (txtStaticIp.Enabled)
                    txtStaticIp.Text = _NetworkInterfaceTarget.TargetPrivateIpAddress;
                else
                    txtStaticIp.Text = String.Empty;
            }

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private void txtStaticIp_TextChanged(object sender)
        {
            if (cmbAllocationMethod.SelectedItem.ToString() == "Static")
            {
                if (_NetworkInterfaceTarget != null)
                    _NetworkInterfaceTarget.TargetPrivateIpAddress = txtStaticIp.Text.Trim();

                if (!_IsBinding)
                    PropertyChanged?.Invoke();
            }
        }

    }
}

