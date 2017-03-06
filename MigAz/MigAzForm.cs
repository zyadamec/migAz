using System;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using MigAz.Providers;
using System.Threading.Tasks;
using System.Linq;
using MigAz.UserControls;
using MigAz.Azure.Asm;
using MigAz.Azure.Arm;
using MigAz.Azure;
using MigAz.Interface;
using MigAz.Core.Interface;

namespace MigAz
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
            this.asmToArm1.Bind(_statusProvider, _logProvider);
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

        #region New Version Check

        private async Task NewVersionAvailable()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://asmtoarmtoolapi.azurewebsites.net/api/version");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                string version = "\"" + Assembly.GetEntryAssembly().GetName().Version.ToString() + "\"";
                string availableversion = result.ToString();

                if (version != availableversion)
                {
                    DialogResult dialogresult = MessageBox.Show("New version " + availableversion + " is available at http://aka.ms/MigAz", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception exception)
            {
                DialogResult dialogresult = MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Form Events

        private async void MigAzForm_Load(object sender, EventArgs e)
        {
            _logProvider.WriteLog("MigAzForm_Load", "Program start");

            this.Text = "MigAz (" + Assembly.GetEntryAssembly().GetName().Version.ToString() + ")";

            await NewVersionAvailable(); // check if there a new version of the app
        }

        #endregion

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://aka.ms/MigAz");
        }
    }
}
