using MigAz.Azure;
using MigAz.Azure.Arm;
using MigAz.Azure.Asm;
using MigAz.Azure.Generator;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Interface;
using MigAz.Azure.Models;
using MigAz.Core.Interface;
using MigAz.Providers;
using MigAz.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MigAz.Forms.ASM
{
    public partial class PreExportDialog : Form
    {
        AsmArtifacts artifacts;
        ILogProvider _LogProvider;
        IStatusProvider _StatusProvider;
        ITelemetryProvider _TelemetryProvider;
        AppSettingsProvider _AppSettingProvider;
        AzureSubscription _AsmSourceSubscription;
        AzureSubscription _ArmTargetSubscription;
        ArmResourceGroup _ArmResourceGroup;
        List<TreeNode> _SelectedNodes;

        private PreExportDialog() { }

        public PreExportDialog(
            ILogProvider logProvider, 
            IStatusProvider statusProvider,
            ITelemetryProvider telemetryProvider,
            AppSettingsProvider appSettingsProvider,
            AzureSubscription asmSourceSubscription,
            AzureSubscription armTargetSubscription,
            ArmResourceGroup targetResourceGroup,
            List<TreeNode> selectedNodes
            )
        {
            InitializeComponent();
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
            _TelemetryProvider = telemetryProvider;
            _AppSettingProvider = appSettingsProvider;
            _AsmSourceSubscription = asmSourceSubscription;
            _ArmTargetSubscription = armTargetSubscription;
            _ArmResourceGroup = targetResourceGroup;
            _SelectedNodes = selectedNodes;
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
                TemplateGenerator templateGenerator = new TemplateGenerator(_LogProvider, _StatusProvider, _TelemetryProvider, _AppSettingProvider);
                TemplateResult templateResult = await templateGenerator.GenerateTemplate(_AsmSourceSubscription, _ArmTargetSubscription, artifacts, _ArmResourceGroup, txtDestinationFolder.Text);
                var exportResults = new ExportResultsDialog(templateResult);
                exportResults.ShowDialog(this);
                this.Close();
            }
            catch (Exception ex)
            {
                _LogProvider.WriteLog("btnExport_Click", "Error generating template : " + ex.ToString());
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
            txtDestinationFolder.Text = AppDomain.CurrentDomain.BaseDirectory;

            List<TreeNode> selectedNodes = _SelectedNodes;
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
