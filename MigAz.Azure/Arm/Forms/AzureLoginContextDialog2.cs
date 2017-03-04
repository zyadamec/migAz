using MigAz.Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.Arm.Forms
{
    public partial class AzureLoginContextDialog2 : Form
    {
        public AzureLoginContextDialog2()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureContext azureContext)
        {
            await this.azureArmLoginControl21.BindContext(azureContext);
            azureContext.AfterAzureSubscriptionChange += AzureContextSourceASM_AfterAzureSubscriptionChange;
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