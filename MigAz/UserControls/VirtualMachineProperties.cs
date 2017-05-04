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

                this.diskProperties1.Bind(asmToArmForm, asmVirtualMachine.OSVirtualHardDisk);

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

                // TODO Now Russell
                //if ((asmVirtualMachine.TargetSubnet == null) ||
                //        (asmVirtualMachine.TargetSubnet.GetType() == typeof(Azure.Asm.Subnet)) ||
                //        (rbExistingARMVNet.Enabled == false))
                //{
                //    rbVNetInMigration.Checked = true;
                //}
                //else
                //{
                //    rbExistingARMVNet.Checked = true;
                //}
            }
            else if (targetVirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)targetVirtualMachine.Source;

                lblRoleSize.Text = armVirtualMachine.VmSize;
                // todo russell lblOS.Text = armVirtualMachine.OSVirtualHardDiskOS;
                if (armVirtualMachine.VirtualNetwork != null)
                    lblVirtualNetworkName.Text = armVirtualMachine.VirtualNetwork.Name;
                // todo now russell lblSubnetName.Text = armVirtualMachine.PrimaryNetworkInterface.SubnetName;
                // todo russell lblStaticIpAddress.Text = armVirtualMachine.StaticVirtualNetworkIPAddress;

                this.diskProperties1.Bind(asmToArmForm, armVirtualMachine.OSVirtualHardDisk);
            }
        }

        private async Task DiskProperties1_PropertyChanged()
        {
            await PropertyChanged();
        }

        private async void cmbExistingArmVNets_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            cmbExistingArmSubnet.Items.Clear();
            if (cmbExistingArmVNets.SelectedItem != null)
            {
                if (cmbExistingArmVNets.SelectedItem.GetType() == typeof(Azure.Asm.VirtualNetwork))
                {
                    Azure.Asm.VirtualNetwork selectedNetwork = (Azure.Asm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.Asm.Subnet subnet in selectedNetwork.Subnets)
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
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine) || asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                RadioButton rb = (RadioButton)sender;

                if (rb.Checked)
                {
                    #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                    cmbExistingArmVNets.Items.Clear();
                    cmbExistingArmSubnet.Items.Clear();

                    TreeNode targetResourceGroupNode = _AsmToArmForm.SeekARMChildTreeNode(_AsmToArmForm.TargetResourceGroup.TargetName, _AsmToArmForm.TargetResourceGroup.ToString(), _AsmToArmForm.TargetResourceGroup, false);
                    TreeNode virtualNetworksNode = _AsmToArmForm.SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Virtual Networks", "Virtual Networks", "Virtual Networks", false);

                    if (virtualNetworksNode != null)
                    {
                        foreach (TreeNode asmVirtualNetworkNode in virtualNetworksNode.Nodes)
                        {
                            TreeNode asmVirtualNetworkAsmParentNode = (TreeNode)asmVirtualNetworkNode.Tag;

                            if (asmVirtualNetworkAsmParentNode.Tag.GetType() == typeof(Azure.Asm.VirtualNetwork))
                            {
                                if (((Azure.Asm.VirtualNetwork)asmVirtualNetworkAsmParentNode.Tag).HasNonGatewaySubnet)
                                    cmbExistingArmVNets.Items.Add(asmVirtualNetworkAsmParentNode.Tag);
                            }
                            else if (asmVirtualNetworkAsmParentNode.Tag.GetType() == typeof(Azure.Arm.VirtualNetwork))
                            {
                                if (((Azure.Arm.VirtualNetwork)asmVirtualNetworkAsmParentNode.Tag).HasNonGatewaySubnet)
                                    cmbExistingArmVNets.Items.Add(asmVirtualNetworkAsmParentNode.Tag);
                            }
                        }
                    }

                    #endregion

                    #region Seek Target VNet and Subnet as ComboBox SelectedItems

                    if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                    {
                        Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
                        //Todo now russell
                        //if (asmVirtualMachine.TargetVirtualNetwork != null)
                        //{
                        //    // Attempt to match target to list items
                        //    foreach (Azure.Asm.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        //    {
                        //        if (listVirtualNetwork.Id == asmVirtualMachine.TargetVirtualNetwork.Id)
                        //        {
                        //            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                        //            break;
                        //        }
                        //    }

                        //    if (cmbExistingArmVNets.SelectedItem != null && asmVirtualMachine.TargetSubnet != null)
                        //    {
                        //        foreach (Azure.Asm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                        //        {
                        //            if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                        //            {
                        //                cmbExistingArmSubnet.SelectedItem = listSubnet;
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
                    {
                        //Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;
                        //if (armVirtualMachine.TargetVirtualNetwork != null)
                        //{
                        //    // Attempt to match target to list items
                        //    foreach (Azure.Asm.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        //    {
                        //        if (listVirtualNetwork.Id == armVirtualMachine.TargetVirtualNetwork.Id)
                        //            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                        //    }

                        //    if (cmbExistingArmVNets.SelectedItem != null && armVirtualMachine.TargetSubnet != null)
                        //    {
                        //        foreach (Azure.Asm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                        //        {
                        //            if (listSubnet.Id == armVirtualMachine.TargetSubnet.Id)
                        //                cmbExistingArmSubnet.SelectedItem = listSubnet;
                        //        }
                        //    }
                        //}
                    }

                    #endregion
                }
            }

            await PropertyChanged();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

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

                if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                {
                    Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;

                    // todo now russell
                    //if (asmVirtualMachine.TargetVirtualNetwork != null)
                    //{
                    //    // Attempt to match target to list items
                    //    for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                    //    {
                    //        Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                    //        if (listVirtualNetwork.Id == asmVirtualMachine.TargetVirtualNetwork.Id)
                    //        {
                    //            cmbExistingArmVNets.SelectedIndex = i;
                    //            break;
                    //        }
                    //    }
                    //}

                    //if (asmVirtualMachine.TargetSubnet != null)
                    //{
                    //    // Attempt to match target to list items
                    //    for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                    //    {
                    //        Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                    //        if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                    //        {
                    //            cmbExistingArmSubnet.SelectedIndex = i;
                    //            break;
                    //        }
                    //    }
                    //}
                }

                #endregion

            }

            await PropertyChanged();
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
                ComboBox cmbSender = (ComboBox)sender;

                // TODO NOW RUssell
                //if (cmbSender.SelectedItem == null)
                //{
                //    asmVirtualMachine.TargetVirtualNetwork = null;
                //    asmVirtualMachine.TargetSubnet = null;
                //}
                //else
                //{
                //    if (cmbSender.SelectedItem.GetType() == typeof(Azure.Asm.Subnet))
                //    {
                //        Azure.Asm.Subnet asmSubnet = (Azure.Asm.Subnet)cmbSender.SelectedItem;
                //        asmVirtualMachine.TargetVirtualNetwork = asmSubnet.Parent;
                //        asmVirtualMachine.TargetSubnet = asmSubnet;
                //    }
                //    else if (cmbSender.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                //    {
                //        Azure.Arm.Subnet armSubnet = (Azure.Arm.Subnet)cmbSender.SelectedItem;
                //        asmVirtualMachine.TargetVirtualNetwork = armSubnet.Parent;
                //        asmVirtualMachine.TargetSubnet = armSubnet;
                //    }
                //}
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine asmVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;
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
