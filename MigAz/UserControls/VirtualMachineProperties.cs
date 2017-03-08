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

namespace MigAz.UserControls
{
    public partial class VirtualMachineProperties : UserControl
    {
        private AsmToArm _AsmToArmForm;
        private TreeNode _VirtualMachineNode;

        public VirtualMachineProperties()
        {
            InitializeComponent();
        }

        public async Task Bind(TreeNode armVirtualMachineNode, AsmToArm asmToArmForm)
        {
            _VirtualMachineNode = armVirtualMachineNode;
            _AsmToArmForm = asmToArmForm;

            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
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
                List<ArmVirtualNetwork> a = await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMVirtualNetworks();
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

        private async void cmbExistingArmVNets_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
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
                ArmVirtualNetwork selectedArmVirtualNetwork = (ArmVirtualNetwork) cmbExistingArmVNets.SelectedItem;

                foreach (ArmSubnet armSubnet in selectedArmVirtualNetwork.Subnets)
                {
                    if (armSubnet.Name != ArmConst.GatewaySubnetName)
                        cmbExistingArmSubnet.Items.Add(armSubnet);
                }

                if (asmVirtualMachine.TargetSubnet != null)
                {
                    foreach (ArmSubnet listSubnet in cmbExistingArmSubnet.Items)
                    {
                        if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                            cmbExistingArmSubnet.SelectedItem = listSubnet;
                    }
                }
            }
        }

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
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

                        if (((Azure.Asm.VirtualNetwork)asmVirtualNetworkAsmParentNode.Tag).HasNonGatewaySubnet)
                            cmbExistingArmVNets.Items.Add(asmVirtualNetworkAsmParentNode.Tag);
                    }
                }

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

                if ((cmbExistingArmVNets.SelectedItem == null) && (cmbExistingArmVNets.Items.Count > 0))
                {
                    foreach (Azure.Asm.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                    {
                        if (listVirtualNetwork.Name == asmVirtualMachine.VirtualNetworkName)
                            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                    }

                    if (cmbExistingArmVNets.SelectedItem == null)
                        cmbExistingArmVNets.SelectedIndex = 0;
                }

                if ((cmbExistingArmSubnet.SelectedItem == null) && (cmbExistingArmSubnet.Items.Count > 0))
                {
                    foreach (Azure.Asm.Subnet listSubnet in cmbExistingArmSubnet.Items)
                    {
                        if (listSubnet.Name == asmVirtualMachine.SubnetName)
                            cmbExistingArmSubnet.SelectedItem = listSubnet;
                    }

                    if (cmbExistingArmSubnet.SelectedItem == null)
                        cmbExistingArmSubnet.SelectedIndex = 0;
                }
            }
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                foreach (ArmVirtualNetwork armVirtualNetwork in await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMVirtualNetworks())
                {
                    if (armVirtualNetwork.HasNonGatewaySubnet)
                        cmbExistingArmVNets.Items.Add(armVirtualNetwork);
                }

                if (asmVirtualMachine.TargetVirtualNetwork != null)
                {
                    // Attempt to match target to list items
                    foreach (ArmVirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                    {
                        if (listVirtualNetwork.Id == asmVirtualMachine.TargetVirtualNetwork.Id)
                            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                    }
                }

                if ((cmbExistingArmVNets.SelectedItem == null) && (cmbExistingArmVNets.Items.Count > 0))
                    cmbExistingArmVNets.SelectedIndex = 0;
            }
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
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
                else if (cmbSender.SelectedItem.GetType() == typeof(ArmSubnet))
                {
                    ArmSubnet armSubnet = (ArmSubnet)cmbSender.SelectedItem;
                    asmVirtualMachine.TargetVirtualNetwork = armSubnet.Parent;
                    asmVirtualMachine.TargetSubnet = armSubnet;
                }
            }
        }

        private void txtARMVMName_TextChanged(object sender, EventArgs e)
        {
            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
            Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)asmTreeNode.Tag;

            asmVirtualMachine.TargetName = txtARMVMName.Text;
            _VirtualMachineNode.Text = asmVirtualMachine.TargetName;
        }
    }
}
