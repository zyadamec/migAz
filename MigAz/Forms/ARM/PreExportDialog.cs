using MigAz.Azure.Generator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Forms.ARM
{
    public partial class PreExportDialog : Form
    {
        public PreExportDialog()
        {
            InitializeComponent();
        }

        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                txtDestinationFolder.Text = folderBrowserDialog1.SelectedPath;

        }

        private async Task btnExport_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDestinationFolder.Text))
            {
                MessageBox.Show("The chosen output folder does not exist.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Window parentForm = (Window)this.Owner;

            try
            {
                //TemplateGenerator templateGenerator = new TemplateGenerator(parentForm.LogProvider, parentForm.StatusProvider, parentForm.TelemetryProvider, parentForm.AppSettingsProviders);
                //TemplateResult templateResult = await templateGenerator.GenerateTemplate(cboTenants.SelectedItem, cboSubscription.SelectedItem, artifacts, txtDestinationFolder.Text);
                //var exportResults = new ExportResultsDialog(templateResult);
                //exportResults.ShowDialog(this);
                this.Close();
            }
            catch (Exception ex)
            {
                parentForm.LogProvider.WriteLog("btnExport_Click", "Error generating template : " + ex.ToString());
                MessageBox.Show("Something went wrong when generating the template. Check the log file for details.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnExport.Enabled = true;
                btnCancel.Enabled = true;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}


// TODO NOW
//private async void cboTenants_SelectedIndexChanged(object sender, EventArgs e)
//{
//    string token = GetToken(cboTenants.SelectedItem.ToString(), PromptBehavior.Auto);

//    // Initialise subscriptions
//    AzureSubscription currentSubscription = null;
//    foreach (AzureSubscription azureSubscription in await _AzureRetriever.GetAzureARMSubscriptions())
//    {
//        subscriptions.Add(azureSubscription);
//    }

//    cboSubscription.DataSource = subscriptions;
//    cboRGLocation.DisplayMember = "SubscriptionName";
//}

