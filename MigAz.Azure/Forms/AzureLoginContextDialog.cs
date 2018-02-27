using MigAz.Azure;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Forms
{
    public partial class AzureLoginContextDialog : Form
    {
        public AzureLoginContextDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureContext azureContext)
        {
            await this.azureArmLoginControl.BindContext(azureContext);
            azureContext.BeforeAzureSubscriptionChange += AzureContext_BeforeAzureSubscriptionChange;
            azureContext.AfterAzureSubscriptionChange += AzureContextSourceASM_AfterAzureSubscriptionChange;
        }

        private async Task AzureContext_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            this.Close();
        }

        private async Task AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            
        }

        private void btnCloseDialog_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}