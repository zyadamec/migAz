using MIGAZ.Core.Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIGAZ.Forms
{
    public partial class AzureContextARMDialog : Form
    {
        AzureContext _AsmAzureContext;
        AzureContext _ArmAzureContext;

        public AzureContextARMDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AzureContext asmAzureContext, AzureContext armAzureContext)
        {
            _AsmAzureContext = asmAzureContext;
            _ArmAzureContext = armAzureContext;

            lblAzureEnvironment.Text = _AsmAzureContext.AzureEnvironment.ToString();
            lblAzureUsername.Text = _AsmAzureContext.TokenProvider.AuthenticationResult.UserInfo.DisplayableId;

            _ArmAzureContext.AzureEnvironment = _AsmAzureContext.AzureEnvironment;

            foreach (AzureSubscription azureSubscription in await _ArmAzureContext.AzureRetriever.GetSubscriptions())
            {
                cmbSubscriptions.Items.Add(azureSubscription);
            }

            cmbSubscriptions.Enabled = cmbSubscriptions.Items.Count > 0;

            foreach (AzureSubscription azureSubscription in cmbSubscriptions.Items)
            {
                if (_ArmAzureContext.AzureSubscription != null && azureSubscription == _ArmAzureContext.AzureSubscription)
                    cmbSubscriptions.SelectedItem = azureSubscription;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbSubscriptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _ArmAzureContext.SetSubscriptionContext((AzureSubscription)cmbSubscriptions.SelectedItem);
        }

        private void AzureContextARMDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_ArmAzureContext.AzureSubscription == null && cmbSubscriptions.Items.Count > 0)
                    e.Cancel = true;

                if (e.Cancel)
                    MessageBox.Show("An Azure Subscription must be selected from the Source (ASM) or Target (ARM) login.");
            }
        }
    }
}
