using MigAz.Azure.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Forms
{
    public partial class AzureNewOrExistingLoginContextDialog : Form
    {
        public AzureNewOrExistingLoginContextDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureLoginContextViewer azureLoginContextViewer)
        {
            azureLoginContextViewer.SelectedAzureContext.LogProvider.WriteLog("InitializeDialog", "Start AzureSubscriptionContextDialog InitializeDialog");

            lblAzureEnvironment.Text = azureLoginContextViewer.SelectedAzureContext.AzureEnvironment.ToString();
            lblAzureUsername.Text = azureLoginContextViewer.SelectedAzureContext.TokenProvider.AuthenticationResult.UserInfo.DisplayableId;

            cboTenant.Items.Clear();
            if (azureLoginContextViewer.SelectedAzureContext.AzureRetriever != null && azureLoginContextViewer.SelectedAzureContext.TokenProvider != null && azureLoginContextViewer.SelectedAzureContext.TokenProvider.AuthenticationResult != null)
            {
                azureLoginContextViewer.SelectedAzureContext.LogProvider.WriteLog("InitializeDialog", "Loading Azure Tenants");
                foreach (AzureTenant azureTenant in await azureLoginContextViewer.SelectedAzureContext.AzureRetriever.GetAzureARMTenants())
                {
                    if (azureTenant.Subscriptions.Count > 0) // Only add Tenants that have one or more Subscriptions
                    {
                        cboTenant.Items.Add(azureTenant);
                        azureLoginContextViewer.SelectedAzureContext.LogProvider.WriteLog("InitializeDialog", "Added Azure Tenant '" + azureTenant.ToString() + "'");
                    }
                    else
                    {
                        azureLoginContextViewer.SelectedAzureContext.LogProvider.WriteLog("InitializeDialog", "Not adding Azure Tenant '" + azureTenant.ToString() + "'.  Contains no subscriptions.");
                    }
                }
                cboTenant.Enabled = true;

                if (azureLoginContextViewer.SelectedAzureContext.AzureTenant != null)
                {
                    foreach (AzureTenant azureTenant in cboTenant.Items)
                    {
                        if (azureLoginContextViewer.SelectedAzureContext.AzureTenant == azureTenant)
                            cboTenant.SelectedItem = azureTenant;
                    }
                }
            }

            azureLoginContextViewer.SelectedAzureContext.LogProvider.WriteLog("InitializeDialog", "End AzureSubscriptionContextDialog InitializeDialog");
        }

    }
}
