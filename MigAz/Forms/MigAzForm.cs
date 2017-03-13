using System;
using System.Windows.Forms;
using MigAz.Providers;
using MigAz.Core.Interface;
using System.Reflection;
using MigAz.UserControls.Migrators;

namespace MigAz.Forms
{
    public partial class MigAzForm : Form
    {
        #region Variables

        private ILogProvider _logProvider;
        private IStatusProvider _statusProvider;
        private AppSettingsProvider _appSettingsProvider;

        #endregion

        #region Constructors
        public MigAzForm()
        {
            InitializeComponent();
            _logProvider = new FileLogProvider();
            _statusProvider = new UIStatusProvider(this.toolStripStatusLabel1);
            _appSettingsProvider = new AppSettingsProvider();
            //awsToArm1.Bind(_statusProvider, _logProvider);
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

        private void aSMToARMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            AsmToArm asmToArm = new AsmToArm(StatusProvider, LogProvider);
            asmToArm.TemplateGenerator.AfterTemplateChanged += TemplateGenerator_AfterTemplateChanged;
            parent.Controls.Add(asmToArm);
        }

        private void TemplateGenerator_AfterTemplateChanged(object sender, EventArgs e)
        {
            MessageBox.Show("TODO -- but we have the event back!!");
        }

        private void aRMToARMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            ArmToArm armToArm = new ArmToArm(StatusProvider, LogProvider);
            armToArm.Bind();
            parent.Controls.Add(armToArm);
        }

        private void aWSToARMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SplitterPanel parent = (SplitterPanel)splitContainer2.Panel1;

            AwsToArm awsToArm = new AwsToArm(StatusProvider, LogProvider);
            awsToArm.Bind();
            parent.Controls.Add(awsToArm);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // Todo
            //_T = new AsmToArmGenerator(_AsmSourceSubscription, _ArmTargetSubscription, _ArmResourceGroup, _LogProvider, _StatusProvider, _TelemetryProvider, _AppSettingProvider);
            //var exportResults = new ExportResultsDialog(templateGenerator);
            //exportResults.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OptionsDialog optionsDialog = new OptionsDialog();
            optionsDialog.ShowDialog();
        }
    }
}
