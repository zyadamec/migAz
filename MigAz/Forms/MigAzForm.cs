﻿using System;
using System.Windows.Forms;
using MigAz.Providers;
using MigAz.Core.Interface;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using MigAz.Azure;
using MigAz.Azure.Generator;
using MigAz.Azure.UserControls;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.MigrationTarget;

namespace MigAz.Forms
{
    public partial class MigAzForm : Form
    {
        #region Variables

        private FileLogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private AppSettingsProvider _appSettingsProvider;
        private TemplateGenerator _TemplateGenerator;
        private TreeNode _EventSourceNode;


        private TargetTreeView _TreeTargetARM;

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
            propertyPanel1.Clear();
            splitContainer2.SplitterDistance = this.Height / 2;
            lblLastOutputRefresh.Text = String.Empty;

            this.propertyPanel1.LogProvider = _logProvider;

            if (splitContainer3.Panel2.Controls.Count == 1)
            {
                MigrationTargetAzure a = (MigrationTargetAzure)splitContainer3.Panel2.Controls[0];
                _TreeTargetARM = a.TargetTreeView;
            }

            if (splitContainer3.Panel1.Controls.Count == 1)
            {
                // This will move to be based on the source context (upon instantiation)
                MigrationSource.MigrationSourceAzure control = (MigrationSource.MigrationSourceAzure)splitContainer3.Panel1.Controls[0];
                control.Bind(this._statusProvider, this._logProvider, this._appSettingsProvider, this.imageList1);


                //control.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
                //control.UserAuthenticated += _AzureContext_UserAuthenticated;
                //control.BeforeAzureSubscriptionChange += _AzureContext_BeforeAzureSubscriptionChange;
                //control.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
                //control.BeforeUserSignOut += _AzureContext_BeforeUserSignOut;
                //control.AfterUserSignOut += _AzureContext_AfterUserSignOut;
                //control.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
                //control.BeforeAzureTenantChange += _AzureContextSource_BeforeAzureTenantChange;
                control.AfterNodeChecked += _AzureContextSource_AfterNodeChecked;
            }


            if (splitContainer3.Panel2.Controls.Count == 1)
            {
                MigrationTarget.MigrationTargetAzure control = (MigrationTarget.MigrationTargetAzure)splitContainer3.Panel2.Controls[0];
                control.Bind(this.propertyPanel1);
                control.AfterTargetSelected += Control_AfterTargetSelected;
            }

            // todo _TemplateGenerator = new AzureGenerator(_AzureContextSourceASM.AzureSubscription, _AzureContextTargetARM.AzureSubscription, LogProvider, StatusProvider, _telemetryProvider, _appSettingsProvider);

        }

        private async Task Control_AfterTargetSelected(TreeNode sender)
        {
            if (this.LogProvider != null)
                LogProvider.WriteLog("Control_AfterTargetSelected", "Start");

            _EventSourceNode = sender;
            await this.propertyPanel1.Bind(sender);
            _EventSourceNode = null;

            if (this.LogProvider != null)
                LogProvider.WriteLog("Control_AfterTargetSelected", "End");

            if (this.StatusProvider != null)
                StatusProvider.UpdateStatus("Ready");

        }

        private async Task _AzureContextSource_AfterNodeChecked(IMigrationTarget sender)
        {
            TreeNode resultUpdateARMTree = await _TreeTargetARM.AddMigrationTargetToTargetTree(sender);

        }

        private void _logProvider_OnMessage(string message)
        {
            txtLog.AppendText(message);
            txtLog.SelectionStart = txtLog.TextLength;
        }

        private void AzureRetriever_OnRestResult(AzureRestResponse response)
        {
            txtRest.AppendText(response.RequestGuid.ToString() + " " + response.Url + Environment.NewLine);
            txtRest.AppendText(response.Response + Environment.NewLine + Environment.NewLine);
            txtRest.SelectionStart = txtRest.TextLength;
        }

        #endregion

        #region Properties

        public TemplateGenerator TemplateGenerator
        {
            get { return _TemplateGenerator; }
        }

        public ILogProvider LogProvider
        {
            get { return _logProvider; }
        }

        public IStatusProvider StatusProvider
        {
            get { return _statusProvider; }
        }

        internal AppSettingsProvider AppSettingsProvider
        {
            get { return _appSettingsProvider; }
        }

        #endregion

        #region Form Events

        private void MigAzForm_Load(object sender, EventArgs e)
        {
            _logProvider.WriteLog("MigAzForm_Load", "Program start");
            this.Text = "MigAz";
        }

        #endregion

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://aka.ms/MigAz");
        }

        private void splitContainer2_Panel2_Resize(object sender, EventArgs e)
        {
            this.tabMigAzMonitoring.Width = splitContainer2.Panel2.Width - 5;
            this.tabMigAzMonitoring.Height = splitContainer2.Panel2.Height - 5;
            this.tabOutputResults.Width = splitContainer2.Panel2.Width - 5;
            this.tabOutputResults.Height = splitContainer2.Panel2.Height - 55;
        }

        private async void TemplateGenerator_AfterTemplateChanged(object sender, EventArgs e)
        {
            TemplateGenerator a = (TemplateGenerator)sender;
            dgvMigAzMessages.DataSource = a.Alerts.Select(x => new { AlertType = x.AlertType, Message = x.Message, SourceObject = x.SourceObject }).ToList();
            dgvMigAzMessages.Columns["Message"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvMigAzMessages.Columns["SourceObject"].Visible = false;
            btnRefreshOutput.Enabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_Resize(object sender, EventArgs e)
        {
            dgvMigAzMessages.Width = tabMigAzMonitoring.Width - 10;
            dgvMigAzMessages.Height = tabMigAzMonitoring.Height - 30;
            txtLog.Width = tabMigAzMonitoring.Width - 10;
            txtLog.Height = tabMigAzMonitoring.Height - 30;
            txtRest.Width = tabMigAzMonitoring.Width - 10;
            txtRest.Height = tabMigAzMonitoring.Height - 30;
        }

        private async Task AzureContextSourceASM_AfterAzureSubscriptionChange(Azure.AzureContext sender)
        {
            dgvMigAzMessages.DataSource = null;
            tabOutputResults.TabPages.Clear();
            btnRefreshOutput.Enabled = false;
            lblLastOutputRefresh.Text = String.Empty;
        }

        #region Menu Items


        private void closeMigrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogProvider.WriteLog("closeMigrationToolStripMenuItem_Click", "Start close current migration session control");

            if (splitContainer2.Panel1.Controls.Count > 0)
            {
                foreach (Control control in splitContainer2.Panel1.Controls)
                {
                    control.Dispose();
                }
            }

            propertyPanel1.Clear();
            dgvMigAzMessages.DataSource = null;
            tabOutputResults.TabPages.Clear();
            btnRefreshOutput.Enabled = false;
            lblLastOutputRefresh.Text = String.Empty;
            this.Text = "MigAz";

            LogProvider.WriteLog("closeMigrationToolStripMenuItem_Click", "End close current migration session control");
        }

        #endregion

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            propertyPanel1.Width = splitContainer1.Panel2.Width - 10;
            propertyPanel1.Height = splitContainer1.Panel2.Height - 100;
            panel1.Top = splitContainer1.Panel2.Height - panel1.Height - 15;
            panel1.Width = splitContainer1.Panel2.Width;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            btnExport.Width = panel1.Width - 15;
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

        private async void btnExport_Click_1Async(object sender, EventArgs e)
        {
            if (splitContainer2.Panel1.Controls.Count == 1)
            {
                IMigratorUserControl migrator = (IMigratorUserControl)splitContainer2.Panel1.Controls[0];

                if (this.TemplateGenerator.HasErrors)
                {
                    tabMigAzMonitoring.SelectTab("tabMessages");
                    MessageBox.Show("There are still one or more error(s) with the template generation.  Please resolve all errors before exporting.");
                    return;
                }

                this.TemplateGenerator.OutputDirectory = txtDestinationFolder.Text;

                // We are refreshing both the MemoryStreams and the Output Tabs via this call, prior to writing to files
                await RefreshOutput();

                this.TemplateGenerator.Write();

                StatusProvider.UpdateStatus("Ready");

                var exportResults = new ExportResultsDialog(this.TemplateGenerator);
                exportResults.ShowDialog(this);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            if (parent.Controls.Count == 1 && e.RowIndex > -1)
            {
                IMigratorUserControl migrator = (IMigratorUserControl)parent.Controls[0];
                object alert = dgvMigAzMessages.Rows[e.RowIndex].Cells["SourceObject"].Value;
                migrator.SeekAlertSource(alert);
            }
        }

        private void tabOutputResults_Resize(object sender, EventArgs e)
        {
            foreach (TabPage tabPage in tabOutputResults.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    control.Width = tabOutputResults.Width - 15;
                    control.Height = tabOutputResults.Height - 30;
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsDialog optionsDialog = new OptionsDialog();
            optionsDialog.ShowDialog();
        }

        private void btnChoosePath_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                txtDestinationFolder.Text = folderBrowserDialog1.SelectedPath;
        }

        private async void btnRefreshOutput_Click(object sender, EventArgs e)
        {
            await RefreshOutput();
        }

        private async Task RefreshOutput()
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            if (parent.Controls.Count == 1)
            {
                IMigratorUserControl migrator = (IMigratorUserControl)parent.Controls[0];

                if (this.TemplateGenerator.HasErrors)
                {
                    tabMigAzMonitoring.SelectTab("tabMessages");
                    MessageBox.Show("There are still one or more error(s) with the template generation.  Please resolve all errors before exporting.");
                    return;
                }

                this.TemplateGenerator.AccessSASTokenLifetimeSeconds = app.Default.AccessSASTokenLifetimeSeconds;

                await this.TemplateGenerator.GenerateStreams();
                await this.TemplateGenerator.SerializeStreams();

                foreach (TabPage tabPage in tabOutputResults.TabPages)
                {
                    if (!this.TemplateGenerator.TemplateStreams.ContainsKey(tabPage.Name))
                        tabOutputResults.TabPages.Remove(tabPage);
                }

                foreach (var templateStream in this.TemplateGenerator.TemplateStreams)
                {
                    TabPage tabPage = null;
                    if (!tabOutputResults.TabPages.ContainsKey(templateStream.Key))
                    {
                        tabPage = new TabPage(templateStream.Key);
                        tabPage.Name = templateStream.Key;
                        tabOutputResults.TabPages.Add(tabPage);

                        if (templateStream.Key.EndsWith(".html"))
                        {
                            WebBrowser webBrowser = new WebBrowser();
                            webBrowser.Width = tabOutputResults.Width - 15;
                            webBrowser.Height = tabOutputResults.Height - 30;
                            webBrowser.AllowNavigation = false;
                            webBrowser.ScrollBarsEnabled = true;
                            tabPage.Controls.Add(webBrowser);
                        }
                        else if (templateStream.Key.EndsWith(".json") || templateStream.Key.EndsWith(".ps1"))
                        {
                            TextBox textBox = new TextBox();
                            textBox.Width = tabOutputResults.Width - 15;
                            textBox.Height = tabOutputResults.Height - 30;
                            textBox.ReadOnly = true;
                            textBox.Multiline = true;
                            textBox.WordWrap = false;
                            textBox.ScrollBars = ScrollBars.Both;
                            tabPage.Controls.Add(textBox);
                        }
                    }
                    else
                    {
                        tabPage = tabOutputResults.TabPages[templateStream.Key];
                    }

                    if (tabPage.Controls[0].GetType() == typeof(TextBox))
                    {
                        TextBox textBox = (TextBox)tabPage.Controls[0];
                        templateStream.Value.Position = 0;
                        textBox.Text = new StreamReader(templateStream.Value).ReadToEnd();
                    }
                    else if (tabPage.Controls[0].GetType() == typeof(WebBrowser))
                    {
                        WebBrowser webBrowser = (WebBrowser)tabPage.Controls[0];
                        templateStream.Value.Position = 0;

                        if (webBrowser.Document == null)
                        {
                            webBrowser.DocumentText = new StreamReader(templateStream.Value).ReadToEnd();
                        }
                        else
                        {
                            webBrowser.Document.OpenNew(true);
                            webBrowser.Document.Write(new StreamReader(templateStream.Value).ReadToEnd());
                        }
                    }
                }

                if (tabOutputResults.TabPages.Count != this.TemplateGenerator.TemplateStreams.Count)
                    throw new ArgumentException("Count mismatch between tabOutputResults TabPages and Migrator TemplateStreams.  Counts should match after addition/removal above.  tabOutputResults. TabPages Count: " + tabOutputResults.TabPages.Count + "  Migration TemplateStream Count: " + this.TemplateGenerator.TemplateStreams.Count);

                // Ensure Tabs are in same order as output streams
                int streamIndex = 0;
                foreach (string templateStreamKey in this.TemplateGenerator.TemplateStreams.Keys)
                {
                    int rotationCounter = 0;

                    // This while loop is to bubble the tab to the end, as to rotate the tab sequence to ensure they match the order returned from the stream outputs
                    // The addition/removal of Streams may result in order of existing tabPages being "out of order" to the streams generated, so we may need to consider reordering
                    while (tabOutputResults.TabPages[streamIndex].Name != templateStreamKey)
                    {
                        TabPage currentTabpage = tabOutputResults.TabPages[streamIndex];
                        tabOutputResults.TabPages.Remove(currentTabpage);
                        tabOutputResults.TabPages.Add(currentTabpage);

                        rotationCounter++;

                        if (rotationCounter > this.TemplateGenerator.TemplateStreams.Count)
                            throw new ArgumentException("Rotated through all tabs, unabled to locate tab '" + templateStreamKey + "' while ensuring tab order/sequencing.");
                    }

                    streamIndex++;
                }


                lblLastOutputRefresh.Text = "Last Refresh Completed: " + DateTime.Now.ToString();
                btnRefreshOutput.Enabled = false;

                // post Telemetry Record to ASMtoARMToolAPI
                if (AppSettingsProvider.AllowTelemetry)
                {
                    StatusProvider.UpdateStatus("BUSY: saving telemetry information");
                    migrator.PostTelemetryRecord();
                }
            }
        }
        
        private void splitContainer2_Panel1_Resize(object sender, EventArgs e)
        {
            if (splitContainer2.Panel1.Controls.Count == 1)
            {
                if (splitContainer2.Panel1.Height < 300)
                    splitContainer2.Panel1.Controls[0].Height = 300;
                else
                    splitContainer2.Panel1.Controls[0].Height = splitContainer2.Panel1.Height - 20;
            }
        }
    }
}
