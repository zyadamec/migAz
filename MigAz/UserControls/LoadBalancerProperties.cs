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

namespace MigAz.UserControls
{
    public partial class LoadBalancerProperties : UserControl
    {
        private AsmToArm _AsmToArmForm;
        private TreeNode _TargetLoadBalancerNode;
        private Azure.MigrationTarget.LoadBalancer _TargetLoadBalancer;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public LoadBalancerProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AsmToArm parentForm, TreeNode treeNode)
        {
            _AsmToArmForm = parentForm;
            _TargetLoadBalancerNode = treeNode;
            _TargetLoadBalancer = (Azure.MigrationTarget.LoadBalancer)treeNode.Tag;
            txtTargetName.Text = _TargetLoadBalancer.Name;

            if (rbExistingARMVNet.Enabled == false ||
                    _TargetLoadBalancer == null ||
                    _TargetLoadBalancer.FrontEndIpConfigurations.Count == 0 ||
                    _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet == null ||
                    _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)
                )
            {
                rbVNetInMigration.Checked = true;
            }
            else
            {
                rbExistingARMVNet.Checked = true;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetLoadBalancer.Name = txtSender.Text;
            _TargetLoadBalancerNode.Text = _TargetLoadBalancer.ToString();

            PropertyChanged();
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

                if (_TargetLoadBalancer != null && _TargetLoadBalancer.FrontEndIpConfigurations.Count > 0)
                {
                    if (_TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.ToString() == _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                                break;
                            }
                        }

                        if (cmbExistingArmVNets.SelectedItem != null && _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet != null)
                        {
                            foreach (Azure.MigrationTarget.Subnet listSubnet in cmbExistingArmSubnet.Items)
                            {
                                if (listSubnet.ToString() == _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet.ToString())
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

                if (_TargetLoadBalancer != null && _TargetLoadBalancer.FrontEndIpConfigurations.Count > 0)
                {
                    if (_TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                        {
                            Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                            if (listVirtualNetwork.ToString() == _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                    if (_TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet != null)
                    {
                        // Attempt to match target to list items
                        for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                        {
                            Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                            if (listSubnet.ToString() == _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet.ToString())
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

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_TargetLoadBalancer != null && _TargetLoadBalancer.FrontEndIpConfigurations.Count > 0)
            {
                if (cmbExistingArmSubnet.SelectedItem == null)
                {
                    _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork = null;
                    _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet = null;
                }
                else
                {
                    if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                    {
                        _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet = (Azure.MigrationTarget.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                    else if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                    {
                        _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _TargetLoadBalancer.FrontEndIpConfigurations[0].TargetSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                }
            }

            PropertyChanged();
        }

    }
}
