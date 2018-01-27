using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.UserControls;
using MigAz.Azure;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Generator;
using MigAz.Core.Interface;
using MigAz.Core;
using MigAz.Azure.Interface;

namespace MigAz.Azure.UserControls
{
    public partial class MigrationAzureTargetContext : UserControl, IMigrationTargetUserControl
    {
        private AzureContext _AzureContextTarget;
        private AzureGenerator _AzureGenerator;

        public MigrationAzureTargetContext()
        {
            InitializeComponent();
        }

        #region Matching Events from AzureContext

        public delegate Task BeforeAzureTenantChangedHandler(AzureContext sender);
        public event BeforeAzureTenantChangedHandler BeforeAzureTenantChange;

        public delegate Task AfterAzureTenantChangedHandler(AzureContext sender);
        public event AfterAzureTenantChangedHandler AfterAzureTenantChange;

        public delegate Task BeforeAzureSubscriptionChangedHandler(AzureContext sender);
        public event BeforeAzureSubscriptionChangedHandler BeforeAzureSubscriptionChange;

        public delegate Task AfterAzureSubscriptionChangedHandler(AzureContext sender);
        public event AfterAzureSubscriptionChangedHandler AfterAzureSubscriptionChange;

        public delegate Task AzureEnvironmentChangedHandler(AzureContext sender);
        public event AzureEnvironmentChangedHandler AzureEnvironmentChanged;

        public delegate Task UserAuthenticatedHandler(AzureContext sender);
        public event UserAuthenticatedHandler UserAuthenticated;

        public delegate Task BeforeUserSignOutHandler();
        public event BeforeUserSignOutHandler BeforeUserSignOut;

        public delegate Task AfterUserSignOutHandler();
        public event AfterUserSignOutHandler AfterUserSignOut;

        #endregion

        #region Properties

        public AzureGenerator TemplateGenerator
        {
            get
            {
                return _AzureGenerator;
            }
        }

        public AzureContext AzureContext
        {
            get { return azureLoginContextViewerTarget.SelectedAzureContext; }
        }

        public AzureContext ExistingContext
        {
            get { return azureLoginContextViewerTarget.ExistingContext; }
            set { azureLoginContextViewerTarget.ExistingContext = value; }
        }

        #endregion

        #region Methods

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _AzureContextTarget = new AzureContext(logProvider, statusProvider);
            _AzureContextTarget.AzureEnvironmentChanged += _AzureContextTarget_AzureEnvironmentChanged;
            _AzureContextTarget.UserAuthenticated += _AzureContextTarget_UserAuthenticated;
            _AzureContextTarget.BeforeAzureSubscriptionChange += _AzureContextTarget_BeforeAzureSubscriptionChange;
            _AzureContextTarget.AfterAzureSubscriptionChange += _AzureContextTarget_AfterAzureSubscriptionChange;
            _AzureContextTarget.BeforeUserSignOut += _AzureContextTarget_BeforeUserSignOut;
            _AzureContextTarget.AfterUserSignOut += _AzureContextTarget_AfterUserSignOut;
            _AzureContextTarget.BeforeAzureTenantChange += _AzureContextTarget_BeforeAzureTenantChange;
            _AzureContextTarget.AfterAzureTenantChange += _AzureContextTarget_AfterAzureTenantChange;

            await azureLoginContextViewerTarget.Bind(_AzureContextTarget);

            this._AzureGenerator = new AzureGenerator(_AzureContextTarget.AzureSubscription, _AzureContextTarget.AzureSubscription, logProvider, statusProvider);
        }

        #endregion

        #region AzureContext Event Handlers (re-raised as Target Context Events)

        private async Task _AzureContextTarget_BeforeAzureTenantChange(AzureContext sender)
        {
            this.BeforeAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContextTarget_AfterAzureTenantChange(AzureContext sender)
        {
            this.AfterAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContextTarget_AfterUserSignOut()
        {
            this.AfterUserSignOut?.Invoke();
        }

        private async Task _AzureContextTarget_BeforeUserSignOut()
        {
            this.BeforeUserSignOut?.Invoke();
        }

        private async Task _AzureContextTarget_AfterAzureSubscriptionChange(AzureContext sender)
        {
            this.AfterAzureSubscriptionChange?.Invoke(sender);
        }

        private async Task _AzureContextTarget_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            this.BeforeAzureSubscriptionChange?.Invoke(sender);
        }

        private async Task _AzureContextTarget_UserAuthenticated(AzureContext sender)
        {
            this.UserAuthenticated?.Invoke(sender);
        }

        private async Task _AzureContextTarget_AzureEnvironmentChanged(AzureContext sender)
        {
            this.AzureEnvironmentChanged?.Invoke(sender);
        }

        #endregion

        private void MigrationTargetAzure_Resize(object sender, EventArgs e)
        {
            azureLoginContextViewerTarget.Width = this.Width - 10;
        }
    }
}
