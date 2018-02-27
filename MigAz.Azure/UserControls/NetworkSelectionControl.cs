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
                    rbExistingARMVNet.Text = "Existing VNet in " + _TargetTreeView.TargetResourceGroup.TargetLocation.DisplayName;
                    this.ExistingARMVNetEnabled = targetTreeView.GetExistingVirtualNetworksInTargetLocation().Count() > 0;
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

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private async void rbVNetInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                #region Add "In MigAz Migration" Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                foreach (Azure.MigrationTarget.VirtualNetwork targetVirtualNetwork in _TargetTreeView.GetVirtualNetworksInMigration())
                {
                    cmbExistingArmVNets.Items.Add(targetVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_NetworkInterfaceTarget != null)
                {
                    if (_NetworkInterfaceTarget.TargetVirtualNetwork != null)
                    {
                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.VirtualNetwork listVirtualNetwork in cmbExistingArmVNets.Items)
                        {
                            if (listVirtualNetwork.ToString() == _NetworkInterfaceTarget.TargetVirtualNetwork.ToString())
                            {
                                cmbExistingArmVNets.SelectedItem = listVirtualNetwork;
                                break;
                            }
                        }

                        if (cmbExistingArmVNets.SelectedItem != null && _NetworkInterfaceTarget.TargetSubnet != null)
                        {
                            foreach (Azure.MigrationTarget.Subnet listSubnet in cmbExistingArmSubnet.Items)
                            {
                                if (listSubnet.ToString() == _NetworkInterfaceTarget.TargetSubnet.ToString())
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

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private async void rbExistingARMVNet_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {

                #region Add "Existing in Subscription / Location" ARM Virtual Networks to cmbExistingArmVNets

                cmbExistingArmVNets.Items.Clear();
                cmbExistingArmSubnet.Items.Clear();

                foreach (Arm.VirtualNetwork armVirtualNetwork in _TargetTreeView.GetExistingVirtualNetworksInTargetLocation())
                {
                    if (armVirtualNetwork.HasNonGatewaySubnet)
                        cmbExistingArmVNets.Items.Add(armVirtualNetwork);
                }

                #endregion

                #region Seek Target VNet and Subnet as ComboBox SelectedItems

                if (_NetworkInterfaceTarget != null)
                {
                    if (_NetworkInterfaceTarget.GetType() == typeof(MigrationTarget.NetworkInterfaceIpConfiguration))
                    {
                        NetworkInterfaceIpConfiguration targetNetworkInterface = (NetworkInterfaceIpConfiguration)_NetworkInterfaceTarget;
                        if (targetNetworkInterface.SourceIpConfiguration != null)
                        {
                            if (targetNetworkInterface.SourceIpConfiguration.GetType() == typeof(Arm.NetworkInterfaceIpConfiguration))
                            {
                                Arm.NetworkInterfaceIpConfiguration armSourceIpConfiguration = (Arm.NetworkInterfaceIpConfiguration)targetNetworkInterface.SourceIpConfiguration;

                                // Attempt to match target to list items
                                for (int i = 0; i < cmbExistingArmVNets.Items.Count; i++)
                                {
                                    Azure.Arm.VirtualNetwork listVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.Items[i];
                                    if (listVirtualNetwork.ToString() == armSourceIpConfiguration.VirtualNetworkName)
                                    {
                                        cmbExistingArmVNets.SelectedIndex = i;
                                        break;
                                    }
                                }


                                    // Attempt to match target to list items
                                for (int i = 0; i < cmbExistingArmSubnet.Items.Count; i++)
                                {
                                    Azure.Arm.Subnet listSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.Items[i];
                                    if (listSubnet.ToString() == armSourceIpConfiguration.SubnetName)
                                    {
                                        cmbExistingArmSubnet.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                #endregion

                if (cmbExistingArmVNets.SelectedIndex < 0 && cmbExistingArmVNets.Items.Count > 0)
                    cmbExistingArmVNets.SelectedIndex = 0;

                if (cmbExistingArmSubnet.SelectedIndex < 0 && cmbExistingArmSubnet.Items.Count > 0)
                    cmbExistingArmSubnet.SelectedIndex = 0;
            }

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private void cmbExistingArmSubnet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_NetworkInterfaceTarget != null)
            {
                if (cmbExistingArmSubnet.SelectedItem == null)
                {
                    _NetworkInterfaceTarget.TargetVirtualNetwork = null;
                    _NetworkInterfaceTarget.TargetSubnet = null;
                }
                else
                {
                    if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.MigrationTarget.Subnet))
                    {
                        _NetworkInterfaceTarget.TargetVirtualNetwork = (Azure.MigrationTarget.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _NetworkInterfaceTarget.TargetSubnet = (Azure.MigrationTarget.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                    else if (cmbExistingArmSubnet.SelectedItem.GetType() == typeof(Azure.Arm.Subnet))
                    {
                        _NetworkInterfaceTarget.TargetVirtualNetwork = (Azure.Arm.VirtualNetwork)cmbExistingArmVNets.SelectedItem;
                        _NetworkInterfaceTarget.TargetSubnet = (Azure.Arm.Subnet)cmbExistingArmSubnet.SelectedItem;
                    }
                }
            }

            if (!_IsBinding)
                PropertyChanged?.Invoke();
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
