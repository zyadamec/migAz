using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Asm;
using MigAz.Azure.Arm;
using MigAz.UserControls.Migrators;
using MigAz.Core.ArmTemplate;
using MigAz.Core.Interface;

namespace MigAz.UserControls
{
    public partial class VirtualMachineProperties : UserControl
    {
        private AsmToArm _AsmToArmForm;
        private TreeNode _VirtualMachineNode;
        private ILogProvider _LogProvider;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public bool AllowManangedDisk
        {
            get { return this.diskProperties1.AllowManangedDisk; }
            set { this.diskProperties1.AllowManangedDisk = value; }
        }

        public VirtualMachineProperties()
        {
            InitializeComponent();
            this.diskProperties1.PropertyChanged += DiskProperties1_PropertyChanged;
        }

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
            set
            {
                _LogProvider = value;
                this.diskProperties1.LogProvider = value;
            }
        }

        public async Task Bind(TreeNode armVirtualMachineNode, AsmToArm asmToArmForm)
        {
            _VirtualMachineNode = armVirtualMachineNode;
            _AsmToArmForm = asmToArmForm;

            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;
            txtARMVMName.Text = targetVirtualMachine.TargetName;

            if (targetVirtualMachine.Source.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;

                lblRoleSize.Text = asmVirtualMachine.RoleSize;
                lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
                lblVirtualNetworkName.Text = asmVirtualMachine.VirtualNetworkName;
                lblSubnetName.Text = asmVirtualMachine.SubnetName;
                lblStaticIpAddress.Text = asmVirtualMachine.StaticVirtualNetworkIPAddress;

                this.diskProperties1.Bind(asmToArmForm, targetVirtualMachine.OSVirtualHardDisk);

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

                if ((targetVirtualMachine.TargetSubnet == null) ||
                        (targetVirtualMachine.TargetSubnet.GetType() == typeof(Azure.MigrationTarget.Subnet)) ||
                        (rbExistingARMVNet.Enabled == false))
                {
                    rbVNetInMigration.Checked = true;
                }
                else
                {
                    rbExistingARMVNet.Checked = true;
                }
            }
            else if (targetVirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)targetVirtualMachine.Source;

                lblRoleSize.Text = armVirtualMachine.VmSize;
                lblOS.Text = armVirtualMachine.OSVirtualHardDiskOS;
                if (armVirtualMachine.VirtualNetwork != null)
                    lblVirtualNetworkName.Text = armVirtualMachine.VirtualNetwork.Name;
                // todo now russell lblSubnetName.Text = armVirtualMachine.PrimaryNetworkInterface.SubnetName;
                // todo russell lblStaticIpAddress.Text = armVirtualMachine.StaticVirtualNetworkIPAddress;

                this.diskProperties1.Bind(asmToArmForm, targetVirtualMachine.OSVirtualHardDisk);
            }
        }

        private async Task DiskProperties1_PropertyChanged()
        {
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

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;

            if (targetVirtualMachine.Source.GetType() == typeof(Azure.Asm.VirtualMachine) || targetVirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
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

                    if (targetVirtualMachine.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.ToString() == targetVirtualMachine.TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                                break;
                            }
                        }

                        if (cmbExistingArmVNets.SelectedItem != null && targetVirtualMachine.TargetSubnet != null)
                        {
                            foreach (Azure.MigrationTarget.Subnet listSubnet in cmbExistingArmSubnet.Items)
                            {
                                if (listSubnet.ToString() == targetVirtualMachine.TargetSubnet.ToString())
                                {
                                    cmbExistingArmSubnet.SelectedItem = listSubnet;
                                    break;
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            await PropertyChanged();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;

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

                if (targetVirtualMachine.TargetVirtualNetwork != null)
                {
                    // Attempt to match target to list items
                    for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                    {
                        Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                        if (listVirtualNetwork.ToString() == targetVirtualMachine.TargetVirtualNetwork.ToString())
                        {
                            cmbExistingArmVNets.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (targetVirtualMachine.TargetSubnet != null)
                {
                    // Attempt to match target to list items
                    for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                    {
                        Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                        if (listSubnet.ToString() == targetVirtualMachine.TargetSubnet.ToString())
                        {
                            cmbExistingArmSubnet.SelectedIndex = i;
                            break;
                        }
                    }
                }

                #endregion

            }

            await PropertyChanged();
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;

            if (cmbExistingArmSubnet.SelectedItem == null)
            {
                targetVirtualMachine.TargetVirtualNetwork = null;
                targetVirtualMachine.TargetSubnet = null;
            }
            else
            {
                if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                {
                    targetVirtualMachine.TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                    targetVirtualMachine.TargetSubnet = (Azure.MigrationTarget.Subnet)cmbExistingArmSubnet.SelectedItem;
                }
                else if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                {
                    targetVirtualMachine.TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                    targetVirtualMachine.TargetSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.SelectedItem;
                }
            }

            PropertyChanged();
        }

        private void txtARMVMName_TextChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;

            targetVirtualMachine.TargetName = txtARMVMName.Text;
            _VirtualMachineNode.Text = targetVirtualMachine.ToString();


            PropertyChanged();
        }
    }
}
