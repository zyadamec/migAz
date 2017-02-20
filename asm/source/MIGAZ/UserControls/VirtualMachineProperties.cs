using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MIGAZ.Asm;
using MIGAZ.Arm;
using MIGAZ.Azure;

namespace MIGAZ.UserControls
{
    public partial class VirtualMachineProperties : UserControl
    {
        private AsmToArmForm _AsmToArmForm;
        private AsmVirtualMachine _AsmVirtualMachine;

        public VirtualMachineProperties()
        {
            InitializeComponent();
        }

        public async void Bind(AsmVirtualMachine asmVirtualMachine, AsmToArmForm asmToArmForm)
        {
            _AsmVirtualMachine = asmVirtualMachine;
            _AsmToArmForm = asmToArmForm;

            lblRoleSize.Text = asmVirtualMachine.RoleSize;
            lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
            lblVirtualNetworkName.Text = asmVirtualMachine.VirtualNetworkName;
            lblSubnetName.Text = asmVirtualMachine.SubnetName;
            lblStaticIpAddress.Text = asmVirtualMachine.StaticVirtualNetworkIPAddress;
            txtARMVMName.Text = asmVirtualMachine.TargetName;

            this.diskProperties1.Bind(asmToArmForm, asmVirtualMachine.OSVirtualHardDisk);

            if ((_AsmVirtualMachine.TargetSubnet == null) ||
                    (_AsmVirtualMachine.TargetSubnet.GetType() == typeof(AsmSubnet)))
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

            if (rbVNetInMigration.Checked)
            {
                AsmVirtualNetwork selectedAsmVirtualNetwork = (AsmVirtualNetwork)cmbExistingArmVNets.SelectedItem;
                
                foreach (AsmSubnet asmSubnet in selectedAsmVirtualNetwork.Subnets)
                {
                    if (asmSubnet.Name != ArmConst.GatewaySubnetName)
                        cmbExistingArmSubnet.Items.Add(asmSubnet);
                }

                if (_AsmVirtualMachine.TargetSubnet != null)
                {
                    foreach (AsmSubnet listSubnet in cmbExistingArmSubnet.Items)
                    {
                        if (listSubnet.Id == _AsmVirtualMachine.TargetSubnet.Id)
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

                if (_AsmVirtualMachine.TargetSubnet != null)
                {
                    foreach (ArmSubnet listSubnet in cmbExistingArmSubnet.Items)
                    {
                        if (listSubnet.Id == _AsmVirtualMachine.TargetSubnet.Id)
                            cmbExistingArmSubnet.SelectedItem = listSubnet;
                    }
                }
            }

            if (cmbExistingArmSubnet.SelectedItem == null && cmbExistingArmSubnet.Items.Count > 0)
                cmbExistingArmSubnet.SelectedIndex = 0;
        }

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
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

                        if (((AsmVirtualNetwork)asmVirtualNetworkAsmParentNode.Tag).HasNonGatewaySubnet)
                            cmbExistingArmVNets.Items.Add(asmVirtualNetworkAsmParentNode.Tag);
                    }
                }

                if (_AsmVirtualMachine.TargetVirtualNetwork != null)
                {
                    // Attempt to match target to list items
                    foreach (AsmVirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                    {
                        if (listVirtualNetwork.Id == _AsmVirtualMachine.TargetVirtualNetwork.Id)
                            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                    }
                }

                if ((cmbExistingArmVNets.SelectedItem == null) && (cmbExistingArmVNets.Items.Count > 0))
                    cmbExistingArmVNets.SelectedIndex = 0;
            }
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
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

                if (_AsmVirtualMachine.TargetVirtualNetwork != null)
                {
                    // Attempt to match target to list items
                    foreach (ArmVirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                    {
                        if (listVirtualNetwork.Id == _AsmVirtualMachine.TargetVirtualNetwork.Id)
                            cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                    }
                }

                if ((cmbExistingArmVNets.SelectedItem == null) && (cmbExistingArmVNets.Items.Count > 0))
                    cmbExistingArmVNets.SelectedIndex = 0;
            }
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;

            if (cmbSender.SelectedItem == null)
            {
                _AsmVirtualMachine.TargetVirtualNetwork = null;
                _AsmVirtualMachine.TargetSubnet = null;
            }
            else
            {
                if (cmbSender.SelectedItem.GetType() == typeof(AsmSubnet))
                {
                    AsmSubnet asmSubnet = (AsmSubnet)cmbSender.SelectedItem;
                    _AsmVirtualMachine.TargetVirtualNetwork = asmSubnet.Parent;
                    _AsmVirtualMachine.TargetSubnet = asmSubnet;
                }
                else if (cmbSender.SelectedItem.GetType() == typeof(ArmSubnet))
                {
                    ArmSubnet armSubnet = (ArmSubnet)cmbSender.SelectedItem;
                    _AsmVirtualMachine.TargetVirtualNetwork = armSubnet.Parent;
                    _AsmVirtualMachine.TargetSubnet = armSubnet;
                }
            }
        }

        private void txtARMVMName_TextChanged(object sender, EventArgs e)
        {
            _AsmVirtualMachine.TargetName = txtARMVMName.Text;
        }
    }
}
