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

            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;

                lblRoleSize.Text = asmVirtualMachine.RoleSize;
                lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
                lblVirtualNetworkName.Text = asmVirtualMachine.VirtualNetworkName;
                lblSubnetName.Text = asmVirtualMachine.SubnetName;
                lblStaticIpAddress.Text = asmVirtualMachine.StaticVirtualNetworkIPAddress;
                txtARMVMName.Text = asmVirtualMachine.TargetName;

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

                if ((asmVirtualMachine.TargetSubnet == null) ||
                        (asmVirtualMachine.TargetSubnet.GetType() == typeof(Azure.Asm.Subnet)) ||
                        (rbExistingARMVNet.Enabled == false))
                {
                    rbVNetInMigration.Checked = true;
                }
                else
                {
                    rbExistingARMVNet.Checked = true;
                }
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;

                lblRoleSize.Text = armVirtualMachine.VmSize;
                //lblOS.Text = armVirtualMachine.OSVirtualHardDiskOS;
                if (armVirtualMachine.VirtualNetwork != null)
                    lblVirtualNetworkName.Text = armVirtualMachine.VirtualNetwork.Name;
                //lblSubnetName.Text = armVirtualMachine.SubnetName;
                //lblStaticIpAddress.Text = armVirtualMachine.StaticVirtualNetworkIPAddress;
                txtARMVMName.Text = armVirtualMachine.TargetName;

            }
        }

        private async Task DiskProperties1_PropertyChanged()
        {
            await PropertyChanged();
        }

        private async void cmbExistingArmVNets_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;

                cmbExistingArmSubnet.Items.Clear();

                if (rbVNetInMigration.Checked)
                {
                    Azure.Asm.VirtualNetwork selectedAsmVirtualNetwork = (Azure.Asm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.Asm.Subnet asmSubnet in selectedAsmVirtualNetwork.Subnets)
                    {
                        if (asmSubnet.Name != ArmConst.GatewaySubnetName)
                            cmbExistingArmSubnet.Items.Add(asmSubnet);
                    }

                    if (asmVirtualMachine.TargetSubnet != null)
                    {
                        foreach (Azure.Asm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                        {
                            if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                                cmbExistingArmSubnet.SelectedItem = listSubnet;
                        }
                    }
                }
                else
                {
                    Azure.Arm.VirtualNetwork selectedArmVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.Arm.Subnet armSubnet in selectedArmVirtualNetwork.Subnets)
                    {
                        if (armSubnet.Name != ArmConst.GatewaySubnetName)
                            cmbExistingArmSubnet.Items.Add(armSubnet);
                    }

                    if (asmVirtualMachine.TargetSubnet != null)
                    {
                        foreach (Azure.Arm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                        {
                            if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                                cmbExistingArmSubnet.SelectedItem = listSubnet;
                        }
                    }
                }
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;
                cmbExistingArmSubnet.Items.Clear();

                if (rbVNetInMigration.Checked)
                {
                    Azure.Arm.VirtualNetwork selectedArmVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;

                    foreach (Azure.Arm.Subnet armSubnet in selectedArmVirtualNetwork.Subnets)
                    {
                        if (armSubnet.Name != ArmConst.GatewaySubnetName)
                            cmbExistingArmSubnet.Items.Add(armSubnet);
                    }

                    if (armVirtualMachine.TargetSubnet != null)
                    {
                        foreach (Azure.Arm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                        {
                            if (listSubnet.Id == armVirtualMachine.TargetSubnet.Id)
                                cmbExistingArmSubnet.SelectedItem = listSubnet;
                        }
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
                    cmbExistingArmVNets.Items.Clear();
                    cmbExistingArmSubnet.Items.Clear();

                    TreeNode targetResourceGroupNode = _AsmToArmForm.SeekARMChildTreeNode(_AsmToArmForm.TargetResourceGroup.Name, _AsmToArmForm.TargetResourceGroup.GetFinalTargetName(), _AsmToArmForm.TargetResourceGroup, false);
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

                    if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
                    {
                        Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
                        if (asmVirtualMachine.TargetVirtualNetwork != null)
                        {
                            // Attempt to match target to list items
                            foreach (Azure.Asm.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                            {
                                if (listVirtualNetwork.Id == asmVirtualMachine.TargetVirtualNetwork.Id)
                                    cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                            }

                            if (cmbExistingArmVNets.SelectedItem != null && asmVirtualMachine.TargetSubnet != null)
                            {
                                foreach (Azure.Asm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                                {
                                    if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                                        cmbExistingArmSubnet.SelectedItem = listSubnet;
                                }
                            }
                        }
                    }
                }
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine asmVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;
            }

            await PropertyChanged();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
                RadioButton rb = (RadioButton)sender;

                if (rb.Checked)
                {
                    cmbExistingArmVNets.Items.Clear();
                    cmbExistingArmSubnet.Items.Clear();

                    foreach (Azure.Arm.VirtualNetwork armVirtualNetwork in await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMVirtualNetworks())
                    {
                        if (armVirtualNetwork.HasNonGatewaySubnet)
                            cmbExistingArmVNets.Items.Add(armVirtualNetwork);
                    }

                    if (asmVirtualMachine.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.Arm.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.Id == asmVirtualMachine.TargetVirtualNetwork.Id)
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                        }
                    }
                }
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine asmVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;
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

                if (cmbSender.SelectedItem == null)
                {
                    asmVirtualMachine.TargetVirtualNetwork = null;
                    asmVirtualMachine.TargetSubnet = null;
                }
                else
                {
                    if (cmbSender.SelectedItem.GetType() == typeof(Azure.Asm.Subnet))
                    {
                        Azure.Asm.Subnet asmSubnet = (Azure.Asm.Subnet)cmbSender.SelectedItem;
                        asmVirtualMachine.TargetVirtualNetwork = asmSubnet.Parent;
                        asmVirtualMachine.TargetSubnet = asmSubnet;
                    }
                    else if (cmbSender.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                    {
                        Azure.Arm.Subnet armSubnet = (Azure.Arm.Subnet)cmbSender.SelectedItem;
                        asmVirtualMachine.TargetVirtualNetwork = armSubnet.Parent;
                        asmVirtualMachine.TargetSubnet = armSubnet;
                    }
                }
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine asmVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;
            }

            PropertyChanged();
        }

        private void txtARMVMName_TextChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;

            if (asmTreeNode.Tag.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;

                asmVirtualMachine.TargetName = txtARMVMName.Text;
                _VirtualMachineNode.Text = asmVirtualMachine.TargetName;
            }
            else if (asmTreeNode.Tag.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine asmVirtualMachine = (Azure.Arm.VirtualMachine)asmTreeNode.Tag;

                asmVirtualMachine.TargetName = txtARMVMName.Text;
                _VirtualMachineNode.Text = asmVirtualMachine.TargetName;
            }

            PropertyChanged();
        }
    }
}
