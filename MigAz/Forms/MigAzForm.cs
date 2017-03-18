﻿using System;
using System.Windows.Forms;
using MigAz.Providers;
using MigAz.Core.Interface;
using System.Reflection;
using MigAz.UserControls.Migrators;
using MigAz.Core.Generator;
using System.Linq;

namespace MigAz.Forms
{
    public partial class MigAzForm : Form
    {
        #region Variables

        private FileLogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private AppSettingsProvider _appSettingsProvider;

        #endregion

        #region Constructors
        public MigAzForm()
        {
            InitializeComponent();
            _logProvider = new FileLogProvider();
            _logProvider.OnMessage += _logProvider_OnMessage;
            _statusProvider = new UIStatusProvider(this.toolStripStatusLabel1);
            _appSettingsProvider = new AppSettingsProvider();

            txtDestinationFolder.Text = AppDomain.CurrentDomain.BaseDirectory;

            splitContainer2.SplitterDistance = this.Height / 2;
        }

        private void _logProvider_OnMessage(string message)
        {
            txtLog.AppendText(message);
            txtLog.SelectionStart = txtLog.TextLength;
        }

        private void AzureRetriever_OnRestResult(Guid requestGuid, string url, string response)
        {
            txtRest.AppendText(requestGuid.ToString() + " " + url + Environment.NewLine);
            txtRest.AppendText(response + Environment.NewLine + Environment.NewLine);
            txtRest.SelectionStart = txtRest.TextLength;
        }

        #endregion

        #region Properties

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _statusProvider; }
        }

        internal AppSettingsProvider AppSettingsProviders
        {
            get { return _appSettingsProvider; }
        }

        #endregion

        #region Form Events

        private void MigAzForm_Load(object sender, EventArgs e)
        {
            _logProvider.WriteLog("MigAzForm_Load", "Program start");
            this.Text = "MigAz (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";
        }

        #endregion

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://aka.ms/MigAz");
        }

        private void splitContainer2_Panel2_Resize(object sender, EventArgs e)
        {
            this.tabControl1.Width = splitContainer2.Panel2.Width - 5;
            this.tabControl1.Height = splitContainer2.Panel2.Height - 5;
        }
        private void TemplateGenerator_AfterTemplateChanged(object sender, EventArgs e)
        {
            TemplateGenerator a = (TemplateGenerator)sender;
            dataGridView1.DataSource = a.Messages.Select(x => new { Message = x }).ToList();
        }

     

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = tabControl1.Width - 10;
            dataGridView1.Height = tabControl1.Height - 30;
            txtLog.Width = tabControl1.Width - 10;
            txtLog.Height = tabControl1.Height - 30;
            txtRest.Width = tabControl1.Width - 10;
            txtRest.Height = tabControl1.Height - 30;
        }

        #region Menu Items

        private void aSMToARMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            AsmToArm asmToArm = new AsmToArm(StatusProvider, LogProvider);
            asmToArm.AzureContextSourceASM.AzureRetriever.OnRestResult += AzureRetriever_OnRestResult;
            asmToArm.TemplateGenerator.AfterTemplateChanged += TemplateGenerator_AfterTemplateChanged;
            parent.Controls.Add(asmToArm);

            newMigrationToolStripMenuItem.Enabled = false;
            closeMigrationToolStripMenuItem.Enabled = true;
        }

        private void aRMToARMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            ArmToArm armToArm = new ArmToArm(StatusProvider, LogProvider);
            armToArm.AzureContextARM.AzureRetriever.OnRestResult += AzureRetriever_OnRestResult;
            parent.Controls.Add(armToArm);

            newMigrationToolStripMenuItem.Enabled = false;
            closeMigrationToolStripMenuItem.Enabled = true;
        }

        private void aWSToARMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            AwsToArm awsToArm = new AwsToArm(StatusProvider, LogProvider);
            awsToArm.Bind();
            parent.Controls.Add(awsToArm);

            newMigrationToolStripMenuItem.Enabled = false ;
            closeMigrationToolStripMenuItem.Enabled = true;
        }

        private void closeMigrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (splitContainer2.Panel1.Controls.Count > 0)
            {
                foreach (Control control in splitContainer2.Panel1.Controls)
                {
                    control.Dispose();
                }
            }

            dataGridView1.DataSource = null;
            newMigrationToolStripMenuItem.Enabled = true;
            closeMigrationToolStripMenuItem.Enabled = false;
        }

        #endregion

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            panel1.Top = splitContainer1.Panel2.Height - panel1.Height - 15;
            panel1.Width = splitContainer1.Panel2.Width;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            btnExport.Width = panel1.Width - 15;
            button1.Width = panel1.Width - 15;
            btnChoosePath.Left = panel1.Width - btnChoosePath.Width - 10;
            txtDestinationFolder.Width = panel1.Width - btnChoosePath.Width - 30;
        }

        private void reportAnIssueOnGithubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Azure/migAz/issues/new");
        }

        private void visitMigAzOnGithubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://aka.ms/migaz");
        }

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            if (splitContainer2.Panel1.Controls.Count == 1)
            {
                IMigratorUserControl migrator = (IMigratorUserControl)splitContainer2.Panel1.Controls[0];
                migrator.TemplateGenerator.OutputDirectory = txtDestinationFolder.Text;
                migrator.TemplateGenerator.Write();

                var exportResults = new ExportResultsDialog(migrator.TemplateGenerator);
                exportResults.ShowDialog(this);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OptionsDialog optionsDialog = new OptionsDialog();
            optionsDialog.ShowDialog();
        }
    }
}
