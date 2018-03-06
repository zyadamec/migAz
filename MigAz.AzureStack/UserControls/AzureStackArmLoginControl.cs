// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure;
using MigAz.Core.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.AzureStack.UserControls
{
    public partial class AzureStackArmLoginControl : UserControl
    {
        private AzureStackContext _AzureStackContext;

        public AzureStackArmLoginControl()
        {
            InitializeComponent();
        }

        public async Task BindContext(AzureStackContext azureStackContext)
        {
            _AzureStackContext = azureStackContext;

            if (_AzureStackContext.TokenProvider == null || _AzureStackContext.TokenProvider.LastUserInfo == null)
            {
                lblAuthenticatedUser.Text = "-";
                btnAuthenticate.Text = "Sign In";
            }
            else
            {
                lblAuthenticatedUser.Text = _AzureStackContext.TokenProvider.LastUserInfo.DisplayableId;
                btnAuthenticate.Text = "Sign Out";
                txtAzureStackEnvironment.Enabled = false;
            }

            cboTenant.Items.Clear();
            if (_AzureStackContext.AzureRetriever != null && _AzureStackContext.TokenProvider != null && _AzureStackContext.TokenProvider.LastUserInfo != null)
            {
                foreach (AzureTenant azureTenant in await _AzureStackContext.GetAzureARMTenants())
                {
                    if (azureTenant.Subscriptions.Count > 0) // Only add Tenants that have one or more Subscriptions
                        cboTenant.Items.Add(azureTenant);
                }
                cboTenant.Enabled = true;

                if (_AzureStackContext.AzureTenant != null)
                {
                    foreach (AzureTenant azureTenant in cboTenant.Items)
                    {
                        if (_AzureStackContext.AzureTenant == azureTenant)
                            cboTenant.SelectedItem = azureTenant;
                    }
                }

                if (cboTenant.SelectedItem != null)
                {
                    AzureTenant selectedTenant = (AzureTenant)cboTenant.SelectedItem;

                    cmbSubscriptions.Items.Clear();
                    if (_AzureStackContext.AzureRetriever != null)
                    {
                        foreach (AzureSubscription azureSubscription in await selectedTenant.GetAzureARMSubscriptions(_AzureStackContext))
                        {
                            cmbSubscriptions.Items.Add(azureSubscription);
                        }
                        cmbSubscriptions.Enabled = true;
                    }

                    if (_AzureStackContext.AzureSubscription != null)
                    {
                        foreach (AzureSubscription azureSubscription in cmbSubscriptions.Items)
                        {
                            if (_AzureStackContext.AzureSubscription == azureSubscription)
                                cmbSubscriptions.SelectedItem = azureSubscription;
                        }
                    }
                }
            }

            azureStackContext.StatusProvider.UpdateStatus("Ready");
        }

        private async void btnAuthenticate_Click(object sender, EventArgs e)
        {
            _AzureStackContext.LogProvider.WriteLog("btnAuthenticate_Click", "Start");

            if (btnAuthenticate.Text == "Sign In")
            {
                try
                {
                    cboTenant.Enabled = false;
                    cboTenant.Items.Clear();
                    cmbSubscriptions.Enabled = false;
                    cmbSubscriptions.Items.Clear();

                    // Obtain Azure Stack Envrionment Metadata
                    //await _AzureStackContext.LoadMetadataEndpoints(txtAzureStackEnvironment.Text);
                    txtAzureStackEnvironment.Enabled = false;

                    await _AzureStackContext.Login();

                    if (_AzureStackContext.TokenProvider != null)
                    {
                        lblAuthenticatedUser.Text = _AzureStackContext.TokenProvider.LastUserInfo.DisplayableId;
                        btnAuthenticate.Text = "Sign Out";

                        cboTenant.Items.Clear();
                        foreach (AzureTenant azureTenant in await _AzureStackContext.GetAzureARMTenants())
                        {
                            if (azureTenant.Subscriptions.Count > 0) // Only add Tenants to the drop down that have subscriptions
                                cboTenant.Items.Add(azureTenant);
                        }

                        cboTenant.Enabled = true;

                        Application.DoEvents();

                        if (cboTenant.Items.Count == 0)
                        {
                            MessageBox.Show("This account does not have any Tenants with Azure Subscription(s).  Logging out of Azure AD Account.");
                            btnAuthenticate_Click(this, null); // No tenants, logout
                        }
                        if (cboTenant.Items.Count == 1)
                        {
                            cboTenant.SelectedIndex = 0;
                        }
                        else if (cboTenant.Items.Count > 1)
                        {
                            _AzureStackContext.StatusProvider.UpdateStatus("WAIT: Awaiting user selection of Azure Tenant");
                        }
                    }
                    else
                    {
                        _AzureStackContext.LogProvider.WriteLog("GetToken_Click", "Failed to get token");
                    }
                }
                catch (Microsoft.IdentityModel.Clients.ActiveDirectory.AdalServiceException exc)
                {
                    if (exc.ErrorCode == "authentication_canceled")
                    {
                        // do nothing
                    }
                    else
                        throw exc;
                }
            }
            else
            {
                await _AzureStackContext.Logout();
                lblAuthenticatedUser.Text = "<Not Authenticated>";
                btnAuthenticate.Text = "Sign In";
                cboTenant.Items.Clear();
                cboTenant.Enabled = false;
                cmbSubscriptions.Items.Clear();
                cmbSubscriptions.Enabled = false;
                txtAzureStackEnvironment.Enabled = true;
            }

            _AzureStackContext.LogProvider.WriteLog("btnAuthenticate_Click", "End");
            _AzureStackContext.StatusProvider.UpdateStatus("Ready");
        }

        private async void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AzureStackContext.LogProvider.WriteLog("cmbSubscriptions_SelectedIndexChanged", "Start");

            ComboBox cmbSender = (ComboBox)sender;

            if (cmbSender.SelectedItem != null)
            {
                AzureSubscription selectedSubscription = (AzureSubscription)cmbSender.SelectedItem;
                if (_AzureStackContext.AzureSubscription != selectedSubscription)
                {
                    await _AzureStackContext.SetSubscriptionContext((AzureSubscription)cmbSender.SelectedItem);
                }
            }

            _AzureStackContext.LogProvider.WriteLog("cmbSubscriptions_SelectedIndexChanged", "End");
        }

        private async void cboTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AzureStackContext.LogProvider.WriteLog("cboTenant_SelectedIndexChanged", "Start");

            cmbSubscriptions.Items.Clear();

            ComboBox cmbSender = (ComboBox)sender;
            if (cmbSender.SelectedItem != null)
            {
                AzureTenant selectedTenant = (AzureTenant)cmbSender.SelectedItem;
                await _AzureStackContext.SetTenantContext(selectedTenant);

                foreach (AzureSubscription azureSubscription in await _AzureStackContext.GetAzureStackARMSubscriptions(selectedTenant)) // selectedTenant.Subscriptions)
                {
                    cmbSubscriptions.Items.Add(azureSubscription);
                }

                if (cmbSubscriptions.Items.Count == 1)
                {
                    cmbSubscriptions.SelectedIndex = 0;
                }
                else if (cboTenant.Items.Count > 1)
                {
                    _AzureStackContext.StatusProvider.UpdateStatus("WAIT: Awaiting user selection of Azure Subscription");
                }
            }

            cmbSubscriptions.Enabled = true;

            _AzureStackContext.LogProvider.WriteLog("cboTenant_SelectedIndexChanged", "End");

            Application.DoEvents();
        }
    }
}

