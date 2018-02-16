using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure;
using MigAz.Azure.Forms;
using MigAz.AzureStack.Forms;

namespace MigAz.AzureStack.UserControls
{
    public enum AzureLoginChangeType
    {
        NewOrExistingContext,
        NewContext,
        SubscriptionChangeOnly
    }

    public enum AzureContextSelectedType
    {
        ExistingContext,
        SameUserDifferentSubscription,
        NewContext
    }

    public partial class AzureStackLoginContextViewer : UserControl
    {
        private AzureStackContext _AzureStackContext;
        private AzureContext _ExistingContext;
        private AzureLoginChangeType _ChangeType = AzureLoginChangeType.NewContext;
        private AzureContextSelectedType _AzureContextSelectedType = AzureContextSelectedType.ExistingContext;

        public delegate Task AfterContextChangedHandler(AzureStackLoginContextViewer sender);
        public event AfterContextChangedHandler AfterContextChanged;

        public AzureStackLoginContextViewer()
        {
            InitializeComponent();
        }

        public async Task Bind(AzureStackContext azureStackContext)
        {
            _AzureStackContext = azureStackContext;
            _AzureStackContext.AzureContext.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            _AzureStackContext.AzureContext.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
            _AzureStackContext.AzureContext.UserAuthenticated += _AzureContext_UserAuthenticated;
            _AzureStackContext.AzureContext.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            _AzureStackContext.AzureContext.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
        }

        public AzureContextSelectedType AzureContextSelectedType
        {
            get { return _AzureContextSelectedType; }
            set
            {
                _AzureContextSelectedType = value;
            }
        }
        public AzureContext ExistingContext
        {
            get { return _ExistingContext; }
            set
            {
                _ExistingContext = value;
                UpdateLabels();
            }
        }

        private async Task _AzureContext_AfterAzureTenantChange(AzureContext sender)
        {
            UpdateLabels();
        }

        private async Task _AzureContext_AfterUserSignOut()
        {
            UpdateLabels();
        }

        private async Task _AzureContext_AzureEnvironmentChanged(AzureContext sender)
        {
            UpdateLabels();
        }

        private async Task _AzureContext_UserAuthenticated(AzureContext sender)
        {
            UpdateLabels();
        }

        private async Task _AzureContext_AfterAzureSubscriptionChange(AzureContext sender)
        {
            UpdateLabels();
        }

        public void UpdateLabels()
        {
            lblSourceUser.Text = "-";
            lblSourceSubscriptionName.Text = "-";
            lblSourceSubscriptionId.Text = "-";
            lblTenantName.Text = "-";

            AzureContext selectedContext = this.SelectedAzureContext;
            if (selectedContext != null)
            {
                lblSourceEnvironment.Text = selectedContext.AzureEnvironment.ToString();

                if (selectedContext.AzureTenant != null)
                    lblTenantName.Text = selectedContext.AzureTenant.ToString();

                if (selectedContext.TokenProvider != null &&
                    selectedContext.TokenProvider.LastUserInfo != null)
                {
                    lblSourceUser.Text = selectedContext.TokenProvider.LastUserInfo.DisplayableId;
                }

                if (selectedContext.AzureSubscription != null)
                {
                    lblSourceSubscriptionName.Text = selectedContext.AzureSubscription.Name;
                    lblSourceSubscriptionId.Text = selectedContext.AzureSubscription.SubscriptionId.ToString();
                }
            }
        }

        public string Title
        {
            get { return this.groupSubscription.Text; }
            set { this.groupSubscription.Text = value; }
        }

        public AzureLoginChangeType ChangeType
        {
            get { return _ChangeType; }
            set { _ChangeType = value; }
        }

        public AzureContext SelectedAzureContext
        {
            get
            {
                if (this.ChangeType == AzureLoginChangeType.NewOrExistingContext)
                {
                    if (_ExistingContext != null && _AzureContextSelectedType == AzureContextSelectedType.ExistingContext)
                        return _ExistingContext;
                    else
                    {
                        if (_AzureStackContext == null)
                            return null;
                        else
                            return _AzureStackContext.AzureContext;
                    }
                }
                else
                {
                    if (_AzureStackContext == null)
                        return null;
                    else
                        return _AzureStackContext.AzureContext;
                }
            }
        }

        public AzureContext AzureContext
        {
            get { return _AzureStackContext.AzureContext; }
        }

        private async void btnAzureContext_Click(object sender, EventArgs e)
        {
            if (_AzureStackContext.AzureContext == null)
                throw new ArgumentException("Azure Context not set.  You must initiate the AzureLoginContextViewer control with the Bind Method.");

            if (_ChangeType == AzureLoginChangeType.NewOrExistingContext)
            {
                if (_ExistingContext == null)
                {
                    //AzureLoginContextDialog azureLoginContextDialog = new AzureLoginContextDialog();
                    //await azureLoginContextDialog.InitializeDialog(_AzureContext);
                    //azureLoginContextDialog.ShowDialog();
                    //azureLoginContextDialog.Dispose();
                }
                else
                {
                    //AzureNewOrExistingLoginContextDialog azureLoginContextDialog = new AzureNewOrExistingLoginContextDialog();
                    //await azureLoginContextDialog.InitializeDialog(this);
                    //azureLoginContextDialog.ShowDialog();
                    //azureLoginContextDialog.Dispose();
                }
            }
            else if (_ChangeType == AzureLoginChangeType.NewContext)
            {
                AzureStackLoginContextDialog azureStackLoginContextDialog = new AzureStackLoginContextDialog();
                await azureStackLoginContextDialog.InitializeDialog(_AzureStackContext);
                azureStackLoginContextDialog.ShowDialog();
                azureStackLoginContextDialog.Dispose();
            }
            else
            {
                //AzureSubscriptionContextDialog azureSubscriptionContextDialog = new AzureSubscriptionContextDialog();
                //await azureSubscriptionContextDialog.InitializeDialog(_AzureContext);
                //azureSubscriptionContextDialog.ShowDialog();
                //azureSubscriptionContextDialog.Dispose();
            }

            AfterContextChanged?.Invoke(this);
        }

        private void AzureLoginContextViewer_EnabledChanged(object sender, EventArgs e)
        {
            btnAzureContext.Enabled = this.Enabled;
        }

        public void ChangeAzureContext()
        {
            btnAzureContext_Click(this, null);
        }

        private void AzureLoginContextViewer_Resize(object sender, EventArgs e)
        {
            groupSubscription.Width = this.Width - 5;
            btnAzureContext.Left = this.Width - btnAzureContext.Width - 10;
        }
    }
}
