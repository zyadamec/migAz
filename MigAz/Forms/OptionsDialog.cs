// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MigAz.Forms
{
    public partial class OptionsDialog : Form
    {
        private bool _HasChanges = false;

        public OptionsDialog()
        {
            InitializeComponent();
        }

        private void chkAllowTelemetry_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowTelemetry.Checked == true)
            {
                string message = String.Empty + "\n";
                message = "\n" + "Tool telemetry data is important for us to keep improving it. Data collected is for tool development usage only and will not be shared, by any reason, out of the tool development team or scope.";
                message += "\n";
                message += "\n" + "Tool telemetry DOES send:";
                message += "\n" + ". TenantId";
                message += "\n" + ". SubscriptionId";
                message += "\n" + ". Processed resources type";
                message += "\n" + ". Processed resources location";
                message += "\n" + ". Execution date";
                message += "\n";
                message += "\n" + "Tool telemetry DOES NOT send:";
                message += "\n" + ". Resources names";
                message += "\n" + ". Any resources configuration or caracteristics";
                message += "\n" + ". Any local computer information";
                message += "\n" + ". Any other information not stated on the \"Tool telemetry DOES send\" section";
                message += "\n";
                message += "\n" + "Do you authorize the tool to send telemetry data?";
                DialogResult dialogresult = MessageBox.Show(message, "Authorization Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogresult == DialogResult.No)
                {
                    chkAllowTelemetry.Checked = false;
                }
            }

            _HasChanges = true;
        }

        private void txtStorageAccountSuffix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                _HasChanges = true;
            }
        }

        private void SaveChanges()
        {
            app.Default.ResourceGroupSuffix = txtResourceGroupSuffix.Text.Trim();
            app.Default.VirtualNetworkSuffix = txtVirtualNetworkSuffix.Text.Trim();
            app.Default.VirtualNetworkGatewaySuffix = txtVirtualNetworkGatewaySuffix.Text.Trim();
            app.Default.NetworkSecurityGroupSuffix = txtNetworkSecurityGroupSuffix.Text.Trim();
            app.Default.StorageAccountSuffix = txtStorageAccountSuffix.Text.Trim();
            app.Default.PublicIPSuffix = txtPublicIPSuffix.Text.Trim();
            app.Default.LoadBalancerSuffix = txtLoadBalancerSuffix.Text.Trim();
            app.Default.AvailabilitySetSuffix = txtAvailabilitySetSuffix.Text.Trim();
            app.Default.VirtualMachineSuffix = txtVirtualMachineSuffix.Text.Trim();
            app.Default.NetworkInterfaceCardSuffix = txtNetworkInterfaceCardSuffix.Text.Trim();
            app.Default.AutoSelectDependencies = chkAutoSelectDependencies.Checked;
            app.Default.SaveSelection = chkSaveSelection.Checked;
            app.Default.BuildEmpty = chkBuildEmpty.Checked;
            app.Default.AllowTelemetry = chkAllowTelemetry.Checked;
            app.Default.AccessSASTokenLifetimeSeconds = Convert.ToInt32(upDownAccessSASMinutes.Value) * 60;
            
            switch (cmbLoginPromptBehavior.SelectedItem)
            {
                case "Always":
                    app.Default.LoginPromptBehavior = PromptBehavior.Always;
                    break;
                case "Auto":
                    app.Default.LoginPromptBehavior = PromptBehavior.Auto;
                    break;
                case "SelectAccount":
                    app.Default.LoginPromptBehavior = PromptBehavior.SelectAccount;
                    break;
                default:
                    app.Default.LoginPromptBehavior = PromptBehavior.Auto;
                    break;
            }

            app.Default.AzureEnvironment = cmbDefaultAzureEnvironment.SelectedItem.ToString();

            if (rbClassicDisk.Checked)
                app.Default.DefaultTargetDiskType = Azure.Core.Interface.ArmDiskType.ClassicDisk;
            else
                app.Default.DefaultTargetDiskType = Azure.Core.Interface.ArmDiskType.ManagedDisk;

            app.Default.Save();

            _HasChanges = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_HasChanges)
                SaveChanges();
        }

        private void formOptions_Load(object sender, EventArgs e)
        {
            txtResourceGroupSuffix.Text = app.Default.ResourceGroupSuffix;
            txtVirtualNetworkSuffix.Text = app.Default.VirtualNetworkSuffix;
            txtVirtualNetworkGatewaySuffix.Text = app.Default.VirtualNetworkGatewaySuffix;
            txtNetworkSecurityGroupSuffix.Text = app.Default.NetworkSecurityGroupSuffix;
            txtStorageAccountSuffix.Text = app.Default.StorageAccountSuffix;
            txtPublicIPSuffix.Text = app.Default.PublicIPSuffix;
            txtLoadBalancerSuffix.Text = app.Default.LoadBalancerSuffix;
            txtAvailabilitySetSuffix.Text = app.Default.AvailabilitySetSuffix;
            txtVirtualMachineSuffix.Text = app.Default.VirtualMachineSuffix;
            txtNetworkInterfaceCardSuffix.Text = app.Default.NetworkInterfaceCardSuffix;
            chkAutoSelectDependencies.Checked = app.Default.AutoSelectDependencies;
            chkSaveSelection.Checked = app.Default.SaveSelection;
            chkBuildEmpty.Checked = app.Default.BuildEmpty;
            chkAllowTelemetry.Checked = app.Default.AllowTelemetry;
            upDownAccessSASMinutes.Value = app.Default.AccessSASTokenLifetimeSeconds / 60;
            
            if (app.Default.DefaultTargetDiskType == Azure.Core.Interface.ArmDiskType.ClassicDisk)
                rbClassicDisk.Checked = true;
            else
                rbManagedDisk.Checked = true;

            int promptBehaviorIndex = cmbLoginPromptBehavior.FindStringExact(app.Default.LoginPromptBehavior.ToString());
            cmbLoginPromptBehavior.SelectedIndex = promptBehaviorIndex;

            _HasChanges = false;
        }

        private void btnApplyDefaultNaming_Click(object sender, EventArgs e)
        {
            txtResourceGroupSuffix.Text = "-rg";
            txtVirtualNetworkSuffix.Text = "-vnet";
            txtVirtualNetworkGatewaySuffix.Text = "-gw";
            txtNetworkSecurityGroupSuffix.Text = "-nsg";
            txtStorageAccountSuffix.Text = "v2";
            txtPublicIPSuffix.Text = "-pip";
            txtLoadBalancerSuffix.Text = "-lb";
            txtAvailabilitySetSuffix.Text = "-as";
            txtVirtualMachineSuffix.Text = "-vm";
            txtNetworkInterfaceCardSuffix.Text = "-nic";
        }

        private void OptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_HasChanges)
            {
                DialogResult result = MessageBox.Show("Do you want to save your MigAz Option changes?", "Pending Changes", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    this.SaveChanges();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void migAzOption_CheckedChanged(object sender, EventArgs e)
        {
            _HasChanges = true;
        }

        private void migAzOption_TextChanged(object sender, EventArgs e)
        {
            _HasChanges = true;
        }

        private void upDownAccessSASMinutes_ValueChanged(object sender, EventArgs e)
        {
            _HasChanges = true;
        }

        private void migAzOption_CheckChanged(object sender, EventArgs e)
        {
            _HasChanges = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _HasChanges = false;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _HasChanges = true;
        }

        internal void Bind(List<AzureEnvironment> standardAzureEnvironments, List<AzureEnvironment> userDefinedAzureEnvironments)
        {
            cmbDefaultAzureEnvironment.Items.Clear();
            foreach (AzureEnvironment azureEnvironment in standardAzureEnvironments)
            {
                cmbDefaultAzureEnvironment.Items.Add(azureEnvironment);
            }
            foreach (AzureEnvironment azureEnvironment in userDefinedAzureEnvironments)
            {
                cmbDefaultAzureEnvironment.Items.Add(azureEnvironment);
            }

            int defaultAzureEnvironmentIndex = cmbDefaultAzureEnvironment.FindStringExact(app.Default.AzureEnvironment);
            if (defaultAzureEnvironmentIndex >= 0)
                cmbDefaultAzureEnvironment.SelectedIndex = defaultAzureEnvironmentIndex;
            else
                cmbDefaultAzureEnvironment.SelectedIndex = 0;
        }
    }
}

