using MigAz.Azure;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.AzureStack.Forms
{
    public partial class AzureStackLoginContextDialog : Form
    {
        public AzureStackLoginContextDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureStackContext azureStackContext)
        {
            await this.azureStackArmLoginControl1.BindContext(azureStackContext);
            azureStackContext.AzureContext.AfterAzureSubscriptionChange += AzureContextSourceASM_AfterAzureSubscriptionChange;
        }

        private async Task AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            this.Close();
        }

        private void btnCloseDialog_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}