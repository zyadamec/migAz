using MigAz.Core.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.UserControls
{
    public partial class AzureArmLoginControl : UserControl
    {
        private AzureContext _AzureContext;

        public AzureArmLoginControl()
        {
            InitializeComponent();
        }

        public async Task BindContext(AzureContext azureContext)
        {
            _AzureContext = azureContext;

            cboAzureEnvironment.SelectedItem = null;

            int environmentIndex = cboAzureEnvironment.FindStringExact(_AzureContext.AzureEnvironment.ToString());
            if (environmentIndex >= 0)
            {
                cboAzureEnvironment.SelectedIndex = environmentIndex;
            }
            else
                cboAzureEnvironment.SelectedIndex = 0;

            if (_AzureContext.TokenProvider == null || _AzureContext.TokenProvider.LastUserInfo == null)
            {
                lblAuthenticatedUser.Text = "-";
                cboAzureEnvironment.Enabled = true;
                btnAuthenticate.Text = "Sign In";
            }
            else
            {
                lblAuthenticatedUser.Text = _AzureContext.TokenProvider.LastUserInfo.DisplayableId;
                cboAzureEnvironment.Enabled = false;
                btnAuthenticate.Text = "Sign Out";
            }

            cboTenant.Items.Clear();
            if (_AzureContext.AzureRetriever != null && _AzureContext.TokenProvider != null)
            {
                foreach (AzureTenant azureTenant in await _AzureContext.GetAzureARMTenants())
                {
                    if (azureTenant.Subscriptions.Count > 0) // Only add Tenants that have one or more Subscriptions
                        cboTenant.Items.Add(azureTenant);
                }
                cboTenant.Enabled = true;

                if (_AzureContext.AzureTenant != null)
                {
                    foreach (AzureTenant azureTenant in cboTenant.Items)
                    {
                        if (_AzureContext.AzureTenant == azureTenant)
                            cboTenant.SelectedItem = azureTenant;
                    }
                }

                if (cboTenant.SelectedItem != null)
                {
                    AzureTenant selectedTenant = (AzureTenant)cboTenant.SelectedItem;

                    cmbSubscriptions.Items.Clear();
                    if (_AzureContext.AzureRetriever != null)
                    {
                        foreach (AzureSubscription azureSubscription in await selectedTenant.GetAzureARMSubscriptions(_AzureContext, false))
                        {
                            cmbSubscriptions.Items.Add(azureSubscription);
                        }
                        cmbSubscriptions.Enabled = true;
                    }

                    if (_AzureContext.AzureSubscription != null)
                    {
                        foreach (AzureSubscription azureSubscription in cmbSubscriptions.Items)
                        {
                            if (_AzureContext.AzureSubscription == azureSubscription)
                                cmbSubscriptions.SelectedItem = azureSubscription;
                        }
                    }
                }
            }
        }

        internal void RemoveEnvironment(AzureEnvironment azureEnvironment)
        {
            cboAzureEnvironment.Items.Remove(azureEnvironment);

            if (cboAzureEnvironment.SelectedItem == null)
                cboAzureEnvironment.SelectedIndex = 0;
        }

        private void cboAzureEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_AzureContext != null)
            {
                if (cboAzureEnvironment.SelectedItem == null)
                    _AzureContext.AzureEnvironment = _AzureContext.AzureEnvironment;
                else
                    _AzureContext.AzureEnvironment = (AzureEnvironment) Enum.Parse(typeof(AzureEnvironment), cboAzureEnvironment.SelectedItem.ToString());
            }

            Application.DoEvents();
        }

        private async void btnAuthenticate_Click(object sender, EventArgs e)
        {
            _AzureContext.LogProvider.WriteLog("btnAuthenticate_Click", "Start");

            if (btnAuthenticate.Text == "Sign In")
            {
                try
                {
                    cboTenant.Enabled = false;
                    cboTenant.Items.Clear();
                    cmbSubscriptions.Enabled = false;
                    cmbSubscriptions.Items.Clear();

                    await _AzureContext.Login(_AzureContext.AzureServiceUrls.GetAzureLoginUrl(), _AzureContext.AzureServiceUrls.GetASMServiceManagementUrl());

                    if (_AzureContext.TokenProvider != null)
                    {
                        lblAuthenticatedUser.Text = _AzureContext.TokenProvider.LastUserInfo.DisplayableId;
                        btnAuthenticate.Text = "Sign Out";

                        cboTenant.Items.Clear();
                        foreach (AzureTenant azureTenant in await _AzureContext.GetAzureARMTenants())
                        {
                            if (azureTenant.Subscriptions.Count > 0) // Only add Tenants to the drop down that have subscriptions
                                cboTenant.Items.Add(azureTenant);
                        }

                        cboAzureEnvironment.Enabled = false;
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
                            _AzureContext.StatusProvider.UpdateStatus("WAIT: Awaiting user selection of Azure Tenant");
                        }
                    }
                    else
                    {
                        _AzureContext.LogProvider.WriteLog("GetToken_Click", "Failed to get token");
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
                await _AzureContext.Logout();
                lblAuthenticatedUser.Text = "<Not Authenticated>";
                btnAuthenticate.Text = "Sign In";
                cboTenant.Items.Clear();
                cboTenant.Enabled = false;
                cmbSubscriptions.Items.Clear();
                cmbSubscriptions.Enabled = false;
                cboAzureEnvironment.Enabled = true;
            }

            _AzureContext.LogProvider.WriteLog("btnAuthenticate_Click", "End");
        }

        private async void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AzureContext.LogProvider.WriteLog("cmbSubscriptions_SelectedIndexChanged", "Start");

            ComboBox cmbSender = (ComboBox)sender;

            if (cmbSender.SelectedItem != null)
            {
                AzureSubscription selectedSubscription = (AzureSubscription)cmbSender.SelectedItem;
                if (_AzureContext.AzureSubscription != selectedSubscription)
                {
                    await _AzureContext.SetSubscriptionContext((AzureSubscription)cmbSender.SelectedItem);
                }
            }

            _AzureContext.LogProvider.WriteLog("cmbSubscriptions_SelectedIndexChanged", "End");
        }

        private async void cboTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AzureContext.LogProvider.WriteLog("cboTenant_SelectedIndexChanged", "Start");

            cmbSubscriptions.Items.Clear();

            ComboBox cmbSender = (ComboBox)sender;
            if (cmbSender.SelectedItem != null)
            {
                AzureTenant selectedTenant = (AzureTenant)cmbSender.SelectedItem;
                await _AzureContext.SetTenantContext(selectedTenant);

                foreach (AzureSubscription azureSubscription in selectedTenant.Subscriptions)
                {
                    cmbSubscriptions.Items.Add(azureSubscription);
                }

                if (cmbSubscriptions.Items.Count == 1)
                {
                    cmbSubscriptions.SelectedIndex = 0;
                }
                else if (cboTenant.Items.Count > 1)
                {
                    _AzureContext.StatusProvider.UpdateStatus("WAIT: Awaiting user selection of Azure Subscription");
                }
            }

            cmbSubscriptions.Enabled = true;

            _AzureContext.LogProvider.WriteLog("cboTenant_SelectedIndexChanged", "End");

            Application.DoEvents();
        }

        private void ckbIncludePreviewRegions_CheckedChanged(object sender, EventArgs e)
        {
            _AzureContext.IncludePreviewRegions = ckbIncludePreviewRegions.Checked;
        }
    }
}
