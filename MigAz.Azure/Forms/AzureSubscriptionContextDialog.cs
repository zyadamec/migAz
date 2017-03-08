using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Forms
{
    public partial class AzureSubscriptionContextDialog : Form
    {
        AzureContext _AzureContext;

        public AzureSubscriptionContextDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureContext azureContext)
        {
            _AzureContext = azureContext;

            lblAzureEnvironment.Text = _AzureContext.AzureEnvironment.ToString();
            lblAzureUsername.Text = _AzureContext.TokenProvider.AuthenticationResult.UserInfo.DisplayableId;

            foreach (AzureSubscription azureSubscription in await _AzureContext.AzureRetriever.GetSubscriptions())
            {
                cmbSubscriptions.Items.Add(azureSubscription);
            }

            cmbSubscriptions.Enabled = cmbSubscriptions.Items.Count > 0;

            foreach (AzureSubscription azureSubscription in cmbSubscriptions.Items)
            {
                if (_AzureContext.AzureSubscription != null && azureSubscription == _AzureContext.AzureSubscription)
                    cmbSubscriptions.SelectedItem = azureSubscription;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AzureContext.SetSubscriptionContext((AzureSubscription)cmbSubscriptions.SelectedItem);
        }

        private void AzureContextARMDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_AzureContext.AzureSubscription == null && cmbSubscriptions.Items.Count > 0)
                    e.Cancel = true;

                if (e.Cancel)
                    MessageBox.Show("An Azure Subscription must be selected from the Source (ASM) or Target (ARM) login.");
            }
        }
    }
}
