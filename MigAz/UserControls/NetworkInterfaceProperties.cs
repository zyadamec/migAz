using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.UserControls.Migrators;
using MigAz.Core.Interface;

namespace MigAz.UserControls
{
    public partial class NetworkInterfaceProperties : UserControl
    {

        private AsmToArm _AsmToArmForm;
        private TreeNode _NetworkInterfaceNode;
        private Azure.MigrationTarget.NetworkInterface _TargetNetworkInterface;
        private ILogProvider _LogProvider;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkInterfaceProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AsmToArm asmToArmForm, Azure.MigrationTarget.NetworkInterface targetNetworkInterface)
        {
            _AsmToArmForm = asmToArmForm;
            _TargetNetworkInterface = targetNetworkInterface;

            if (_TargetNetworkInterface.SourceNetworkInterface != null)
            {
                // todo now russell
                lblVirtualNetworkName.Text = _TargetNetworkInterface.SourceNetworkInterface.ToString();
                lblSubnetName.Text = _TargetNetworkInterface.SourceNetworkInterface.ToString();
                lblStaticIpAddress.Text = _TargetNetworkInterface.SourceNetworkInterface.ToString();
            }

            try
            {
                List<Azure.Arm.VirtualNetwork> a = await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMVirtualNetworks();
                rbExistingARMVNet.Enabled = a.Count() > 0;
            }
            catch (Exception exc)
            {
                _AsmToArmForm.LogProvider.WriteLog("VirtualMachineProperties.Bind", exc.Message);
                rbExistingARMVNet.Enabled = false;
            }

            if (rbExistingARMVNet.Enabled == false ||
                _TargetNetworkInterface.TargetSubnet == null ||
                _TargetNetworkInterface.TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)
                )
            {
                rbVNetInMigration.Checked = true;
            }
            else
            {
                rbExistingARMVNet.Checked = true;
            }
        }

        internal void Bind(AsmToArm asmToArmForm, TreeNode networkInterfaceNode)
        {
            _AsmToArmForm = asmToArmForm;
            _NetworkInterfaceNode = networkInterfaceNode;
            _TargetNetworkInterface = (Azure.MigrationTarget.NetworkInterface)_NetworkInterfaceNode.Tag;

            if (rbExistingARMVNet.Enabled == false ||
                _TargetNetworkInterface.TargetSubnet == null ||
                _TargetNetworkInterface.TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)
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

                TreeNode targetResourceGroupNode = _AsmToArmForm.SeekARMChildTreeNode(_AsmToArmForm.TargetResourceGroup.ToString(), _AsmToArmForm.TargetResourceGroup.ToString(), _AsmToArmForm.TargetResourceGroup, false);

                foreach (TreeNode treeNode in targetResourceGroupNode.Nodes)
                {
                    if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.VirtualNetwork))
                    {
                        Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)treeNode.Tag;
                        cmbExistingArmVNets.Items.Add(targetVirtualNetwork);
                    }
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_TargetNetworkInterface != null)
                {
                    if (_TargetNetworkInterface.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.ToString() == _TargetNetworkInterface.TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                                break;
                            }
                        }

                        if (cmbExistingArmVNets.SelectedItem != null && _TargetNetworkInterface.TargetSubnet != null)
                        {
                            foreach (Azure.MigrationTarget.Subnet listSubnet in cmbExistingArmSubnet.Items)
                            {
                                if (listSubnet.ToString() == _TargetNetworkInterface.TargetSubnet.ToString())
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

            await PropertyChanged();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {

                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMVirtualNetworks())
                {
                    if (armVirtualNetwork.HasNonGatewaySubnet)
                        cmbExistingArmVNets.Items.Add(armVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_TargetNetworkInterface != null)
                {
                    if (_TargetNetworkInterface.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                        {
                            Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                            if (listVirtualNetwork.ToString() == _TargetNetworkInterface.TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    if (_TargetNetworkInterface.TargetSubnet != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                        {
                            Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                            if (listSubnet.ToString() == _TargetNetworkInterface.TargetSubnet.ToString())
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
            if (_TargetNetworkInterface != null)
            {
                if (cmbExistingArmSubnet.SelectedItem == null)
                {
                    _TargetNetworkInterface.TargetVirtualNetwork = null;
                    _TargetNetworkInterface.TargetSubnet = null;
                }
                else
                {
                    if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                    {
                        _TargetNetworkInterface.TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _TargetNetworkInterface.TargetSubnet = (Azure.MigrationTarget.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                    else if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                    {
                        _TargetNetworkInterface.TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _TargetNetworkInterface.TargetSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                }
            }

            PropertyChanged();
        }


    }
}
