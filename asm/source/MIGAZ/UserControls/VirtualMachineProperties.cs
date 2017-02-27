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
using MigAz.Azure;

namespace MigAz.UserControls
{
    public partial class VirtualMachineProperties : UserControl
    {
        private AsmToArmForm _AsmToArmForm;
        private TreeNode _VirtualMachineNode;

        public VirtualMachineProperties()
        {
            InitializeComponent();
        }

        public async void Bind(TreeNode armVirtualMachineNode, AsmToArmForm asmToArmForm)
        {
            _VirtualMachineNode = armVirtualMachineNode;
            _AsmToArmForm = asmToArmForm;

            TreeNode asmTreeNode = (TreeNode)_VirtualMachineNode.Tag;
            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;

            lblRoleSize.Text = asmVirtualMachine.RoleSize;
            lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
            lblVirtualNetworkName.Text = asmVirtualMachine.VirtualNetworkName;
            lblSubnetName.Text = asmVirtualMachine.SubnetName;
            lblStaticIpAddress.Text = asmVirtualMachine.StaticVirtualNetworkIPAddress;
            txtARMVMName.Text = asmVirtualMachine.TargetName;

            this.diskProperties1.Bind(asmToArmForm, asmVirtualMachine.OSVirtualHardDisk);

            if ((asmVirtualMachine.TargetSubnet == null) ||
                    (asmVirtualMachine.TargetSubnet.GetType() == typeof(AsmSubnet)))
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
            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;

            cmbExistingArmSubnet.Items.Clear();

            if (rbVNetInMigration.Checked)
            {
                AsmVirtualNetwork selectedAsmVirtualNetwork = (AsmVirtualNetwork)cmbExistingArmVNets.SelectedItem;
                
                foreach (AsmSubnet asmSubnet in selectedAsmVirtualNetwork.Subnets)
                {
                    if (asmSubnet.Name != ArmConst.GatewaySubnetName)
                        cmbExistingArmSubnet.Items.Add(asmSubnet);
                }

                if (asmVirtualMachine.TargetSubnet != null)
                {
                    foreach (AsmSubnet listSubnet in cmbExistingArmSubnet.Items)
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
            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;
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

                        if (((AsmVirtualNetwork)asmVirtualNetworkAsmParentNode.Tag).HasNonGatewaySubnet)
                            cmbExistingArmVNets.Items.Add(asmVirtualNetworkAsmParentNode.Tag);
                    }
                }

                if (asmVirtualMachine.TargetVirtualNetwork != null)
                {
                    // Attempt to match target to list items
                    foreach (AsmVirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                    {
                        if (listVirtualNetwork.Id == asmVirtualMachine.TargetVirtualNetwork.Id)
                            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                    }

                    if (cmbExistingArmVNets.SelectedItem != null && asmVirtualMachine.TargetSubnet != null)
                    {
                        foreach (AsmSubnet listSubnet in cmbExistingArmSubnet.Items)
                        {
                            if (listSubnet.Id == asmVirtualMachine.TargetSubnet.Id)
                                cmbExistingArmSubnet.SelectedItem = listSubnet;
                        }
                    }
                }

                if ((cmbExistingArmVNets.SelectedItem == null) && (cmbExistingArmVNets.Items.Count > 0))
                {
                    foreach (AsmVirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                    {
                        if (listVirtualNetwork.Name == asmVirtualMachine.VirtualNetworkName)
                            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                    }

                    if (cmbExistingArmVNets.SelectedItem == null)
                        cmbExistingArmVNets.SelectedIndex = 0;
                }

                if ((cmbExistingArmSubnet.SelectedItem == null) && (cmbExistingArmSubnet.Items.Count > 0))
                {
                    foreach (AsmSubnet listSubnet in cmbExistingArmSubnet.Items)
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
            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;
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
            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;
            ComboBox cmbSender = (ComboBox)sender;

            if (cmbSender.SelectedItem == null)
            {
                asmVirtualMachine.TargetVirtualNetwork = null;
                asmVirtualMachine.TargetSubnet = null;
            }
            else
            {
                if (cmbSender.SelectedItem.GetType() == typeof(AsmSubnet))
                {
                    AsmSubnet asmSubnet = (AsmSubnet)cmbSender.SelectedItem;
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
            AsmVirtualMachine asmVirtualMachine = (AsmVirtualMachine)asmTreeNode.Tag;

            asmVirtualMachine.TargetName = txtARMVMName.Text;
            _VirtualMachineNode.Text = asmVirtualMachine.TargetName;
        }
    }
}
