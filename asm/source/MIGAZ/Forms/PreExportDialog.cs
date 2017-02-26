using MigAz.Azure.Asm;
using MigAz.Azure.Asm.Generator;
using MigAz.Azure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MIGAZ.Forms
{
    public partial class PreExportDialog : Form
    {
        AsmToArmForm parentForm;
        AsmArtifacts artifacts;

        public PreExportDialog()
        {
            InitializeComponent();
        }

        private async void btnExport_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(txtDestinationFolder.Text))
            {
                MessageBox.Show("Export path is required.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnChoosePath_Click(this, null);
                return;
            }

            if (TemplateResult.OutputFilesExist(txtDestinationFolder.Text))
            {
                if (MessageBox.Show("The target export path already contains export files.  Do you want proceed and overwrite the existing files?", "Overwrite Existing Export", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            btnExport.Enabled = false;
            btnCancel.Enabled = false;

            try
            {
                AsmToArmForm parentForm = (AsmToArmForm)this.Owner;
                TemplateGenerator templateGenerator = new TemplateGenerator(parentForm.LogProvider, parentForm.StatusProvider, parentForm.TelemetryProvider, parentForm.AppSettingsProviders);
                TemplateResult templateResult = await templateGenerator.GenerateTemplate(parentForm.AzureContextSourceASM.AzureSubscription, parentForm.AzureContextTargetARM.AzureSubscription, artifacts, parentForm.TargetResourceGroup, txtDestinationFolder.Text);
                var exportResults = new ExportResultsDialog(templateResult);
                exportResults.ShowDialog(this);
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

        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                txtDestinationFolder.Text = folderBrowserDialog.SelectedPath;
        }

        private async void PreExportForm_Load(object sender, EventArgs e)
        {
            parentForm = (AsmToArmForm) this.Owner;
            txtDestinationFolder.Text = AppDomain.CurrentDomain.BaseDirectory;

            List<TreeNode> selectedNodes = parentForm.SelectedNodes;
            btnExport.Text = btnExport.Text.Replace("0", selectedNodes.Count().ToString());

            artifacts = new AsmArtifacts();
            foreach (TreeNode selectedNode in selectedNodes)
            {
                Type tagType = selectedNode.Tag.GetType();
                if (tagType == typeof(AsmNetworkSecurityGroup))
                {
                    artifacts.NetworkSecurityGroups.Add((AsmNetworkSecurityGroup)selectedNode.Tag);
                }
                else if (tagType == typeof(AsmVirtualNetwork))
                {
                    artifacts.VirtualNetworks.Add((AsmVirtualNetwork)selectedNode.Tag);
                }
                else if (tagType == typeof(AsmStorageAccount))
                {
                    artifacts.StorageAccounts.Add((AsmStorageAccount) selectedNode.Tag);
                }
                else if (tagType == typeof(AsmVirtualMachine))
                {
                    artifacts.VirtualMachines.Add((AsmVirtualMachine)selectedNode.Tag);
                }
            }

        }
    }
}
