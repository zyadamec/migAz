using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Arm.Forms;

namespace MigAz.Azure.Arm.UserControls
{
    public enum ChangeType
    {
        Full,
        SubscriptionOnly
    }

    public partial class AzureLoginContextViewer : UserControl
    {
        private AzureContext _AzureContext;
        private ChangeType _ChangeType = ChangeType.Full;

        public AzureLoginContextViewer()
        {
            InitializeComponent();
        }

        public async Task Bind(AzureContext azureContext)
        {
            _AzureContext = azureContext;
            _AzureContext.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            _AzureContext.UserAuthenticated += _AzureContext_UserAuthenticated;
            _AzureContext.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            _AzureContext.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
        }

        private async Task _AzureContext_AfterUserSignOut()
        {
            lblSourceUser.Text = "-";
            lblSourceSubscriptionName.Text = "-";
            lblSourceSubscriptionId.Text = "-";
        }

        private async Task _AzureContext_AzureEnvironmentChanged(AzureContext sender)
        {
            lblSourceEnvironment.Text = sender.AzureEnvironment.ToString();
        }

        private async Task _AzureContext_UserAuthenticated(Microsoft.IdentityModel.Clients.ActiveDirectory.UserInfo userAuthenticated)
        {
            lblSourceUser.Text = userAuthenticated.DisplayableId;
        }

        private async Task _AzureContext_AfterAzureSubscriptionChange(AzureContext sender)
        {
            if (sender.AzureSubscription != null)
            {
                lblSourceSubscriptionName.Text = sender.AzureSubscription.Name;
                lblSourceSubscriptionId.Text = sender.AzureSubscription.SubscriptionId.ToString();
            }
            else
            {
                lblSourceSubscriptionName.Text = "-";
                lblSourceSubscriptionId.Text = "-";
            }
        }

        public string Title
        {
            get { return this.groupSubscription.Text; }
            set { this.groupSubscription.Text = value; }
        }

        public ChangeType ChangeType
        {
            get { return _ChangeType; }
            set { _ChangeType = value; }
        }

        private async void btnAzureContext_Click(object sender, EventArgs e)
        {
            if (_ChangeType == ChangeType.Full)
            {
                AzureLoginContextDialog azureLoginContextDialog = new AzureLoginContextDialog();
                await azureLoginContextDialog.InitializeDialog(_AzureContext);
                azureLoginContextDialog.ShowDialog();
                azureLoginContextDialog.Dispose();
            }
            else
            {
                AzureSubscriptionContextDialog azureSubscriptionContextDialog = new AzureSubscriptionContextDialog();
                await azureSubscriptionContextDialog.InitializeDialog(_AzureContext);
                azureSubscriptionContextDialog.ShowDialog();
                azureSubscriptionContextDialog.Dispose();
            }
        }

        private void AzureLoginContextViewer_EnabledChanged(object sender, EventArgs e)
        {
            btnAzureContext.Enabled = this.Enabled;
        }
    }
}
