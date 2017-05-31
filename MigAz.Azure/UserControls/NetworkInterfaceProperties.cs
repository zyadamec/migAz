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

namespace MigAz.Azure.UserControls
{
    public partial class NetworkInterfaceProperties : UserControl
    {
        private AzureContext _AzureContext;
        private TargetTreeView _TargetTreeView;
        private Azure.MigrationTarget.NetworkInterface _TargetNetworkInterface;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkInterfaceProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AzureContext azureContext, TargetTreeView targetTreeView, Azure.MigrationTarget.NetworkInterface targetNetworkInterface)
        {
            _AzureContext = azureContext;
            _TargetTreeView = targetTreeView;
            _TargetNetworkInterface = targetNetworkInterface;

            lblSourceName.Text = _TargetNetworkInterface.SourceName;
            txtTargetName.Text = _TargetNetworkInterface.TargetName;


            if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                cmbAllocationMethod.SelectedIndex = cmbAllocationMethod.FindString(_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIPAllocationMethod);
            }

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

            try
            {
                List<Azure.Arm.VirtualNetwork> a = await _AzureContext.AzureRetriever.GetAzureARMVirtualNetworks();
                rbExistingARMVNet.Enabled = a.Count() > 0;
            }
            catch (Exception exc)
            {
                _AzureContext.LogProvider.WriteLog("VirtualMachineProperties.Bind", exc.Message);
                rbExistingARMVNet.Enabled = false;
            }

            if (rbExistingARMVNet.Enabled == false ||
                _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count() == 0 ||
                _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet == null ||
                _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)
                )
            {
                rbVNetInMigration.Checked = true;
            }
            else
            {
                rbExistingARMVNet.Checked = true;
            }
        }
        
        private async void cmbExistingArmVNets_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbExistingArmSubnet.Items.Clear();
            if (cmbExistingArmVNets.SelectedItem != null)
            {
                if (cmbExistingArmVNets.SelectedItem.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                {
                    Azure.MigrationTarget.VirtualNetwork selectedNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.MigrationTarget.Subnet subnet in selectedNetwork.TargetSubnets)
                    {
                        if (!subnet.IsGatewaySubnet)
                            cmbExistingArmSubnet.Items.Add(subnet);
                    }
                }
                else if (cmbExistingArmVNets.SelectedItem.GetType() == typeof(Azure.Arm.VirtualNetwork))
                {
                    Azure.Arm.VirtualNetwork selectedNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.Arm.Subnet subnet in selectedNetwork.Subnets)
                    {
                        if (!subnet.IsGatewaySubnet)
                            cmbExistingArmSubnet.Items.Add(subnet);
                    }
                }
            }

            await PropertyChanged();
        }

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                TreeNode targetResourceGroupNode = _TargetTreeView.ResourceGroupNode;

                if (targetResourceGroupNode != null)
                {
                    foreach (TreeNode treeNode in targetResourceGroupNode.Nodes)
                    {
                        if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                        {
                            Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)treeNode.Tag;
                            cmbExistingArmVNets.Items.Add(targetVirtualNetwork);
                        }
                    }
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_TargetNetworkInterface != null && _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                {
                    if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.ToString() == _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                                break;
                            }
                        }

                        if (cmbExistingArmVNets.SelectedItem != null && _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet != null)
                        {
                            foreach (Azure.MigrationTarget.Subnet listSubnet in cmbExistingArmSubnet.Items)
                            {
                                if (listSubnet.ToString() == _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet.ToString())
                                {
                                    cmbExistingArmSubnet.SelectedItem = listSubnet;
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion
            }

            PropertyChanged();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {

                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in await _AzureContext.AzureRetriever.GetAzureARMVirtualNetworks())
                {
                    if (armVirtualNetwork.HasNonGatewaySubnet)
                        cmbExistingArmVNets.Items.Add(armVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_TargetNetworkInterface != null && _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count() > 0)
                {
                    if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                        {
                            Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                            if (listVirtualNetwork.ToString() == _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                        {
                            Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                            if (listSubnet.ToString() == _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet.ToString())
                            {
                                cmbExistingArmSubnet.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                #endregion

            }

            await PropertyChanged();
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_TargetNetworkInterface != null && _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                if (cmbExistingArmSubnet.SelectedItem == null)
                {
                    _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork = null;
                    _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet = null;
                }
                else
                {
                    if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                    {
                        _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet = (Azure.MigrationTarget.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                    else if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                    {
                        _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                }
            }

            PropertyChanged();
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetNetworkInterface.TargetName = txtSender.Text.Trim();

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmbAllocationMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
            {
                _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIPAllocationMethod = cmbAllocationMethod.SelectedItem.ToString();
                txtStaticIp.Enabled = cmbAllocationMethod.SelectedItem.ToString() == "Static";
                if (txtStaticIp.Enabled)
                    txtStaticIp.Text = _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIpAddress;
                else
                    txtStaticIp.Text = String.Empty;
            }

            PropertyChanged();
        }

        private void txtStaticIp_TextChanged(object sender, EventArgs e)
        {
            if (cmbAllocationMethod.SelectedItem.ToString() == "Static")
            {
                if (_TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations.Count > 0)
                    _TargetNetworkInterface.TargetNetworkInterfaceIpConfigurations[0].TargetPrivateIpAddress = txtStaticIp.Text.Trim();

                PropertyChanged();
            }
        }
    }
}
