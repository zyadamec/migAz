using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core.Interface;
using MigAz.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Forms;

namespace MigAz.AzureStack.UserControls
{
    public partial class MigrationAzureStackSourceContext : UserControl, IMigrationSourceUserControl
    {
        bool _IsAuthenticated = false;
        ILogProvider _LogProvider;
        IStatusProvider _StatusProvider;
        ImageList _ImageList;
        Core.TargetSettings _TargetSettings;
        AzureStackContext _AzureStackContextSource;

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

        #region New Events from MigrationAzureStackSource

        public delegate Task AfterNodeCheckedHandler(Core.MigrationTarget sender);
        public event AfterNodeCheckedHandler AfterNodeChecked;

        public delegate Task AfterNodeUncheckedHandler(Core.MigrationTarget sender);
        public event AfterNodeUncheckedHandler AfterNodeUnchecked;

        public delegate Task AfterNodeChangedHandler(Core.MigrationTarget sender);
        public event AfterNodeChangedHandler AfterNodeChanged;

        public delegate void ClearContextHandler();
        public event ClearContextHandler ClearContext;

        public delegate void AfterContextChangedHandler(MigrationAzureStackSourceContext sender);
        public event AfterContextChangedHandler AfterContextChanged;

        #endregion

        public MigrationAzureStackSourceContext()
        {
            InitializeComponent();
        }

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider, Core.TargetSettings targetSettings, ImageList imageList, PromptBehavior promptBehavior)
        {
            _TargetSettings = targetSettings;
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;
            _ImageList = imageList;

            _AzureStackContextSource = new AzureStackContext(_LogProvider, _StatusProvider, promptBehavior);
            _AzureStackContextSource.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            _AzureStackContextSource.UserAuthenticated += _AzureContext_UserAuthenticated;
            _AzureStackContextSource.BeforeAzureSubscriptionChange += _AzureContext_BeforeAzureSubscriptionChange;
            _AzureStackContextSource.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
            _AzureStackContextSource.BeforeUserSignOut += _AzureContext_BeforeUserSignOut;
            _AzureStackContextSource.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            _AzureStackContextSource.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
            _AzureStackContextSource.BeforeAzureTenantChange += _AzureStackContext_BeforeAzureTenantChange;
            azureStackLoginContextViewer1.AfterContextChanged += AzureStackLoginContextViewerSource_AfterContextChanged;

            treeAzureARM.ImageList = _ImageList;

            await this.azureStackLoginContextViewer1.Bind(_AzureStackContextSource);
        }


        #region Properties

        public bool IsSourceContextAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }

        public AzureStackContext AzureStackContext
        {
            get { return _AzureStackContextSource; }
        }

        #endregion

        public async Task UncheckMigrationTarget(Core.MigrationTarget migrationTarget)
        {
            throw new Exception("Not Here");
        }

        private void MigrationAzureStackSourceContext_Resize(object sender, EventArgs e)
        {
            azureStackLoginContextViewer1.Width = this.Width;
            treeViewSourceResourceManager1.Width = this.Width - 5;
            treeViewSourceResourceManager1.Height = this.Height - 110;
        }

        #region Event Handlers

        private async Task AzureStackLoginContextViewerSource_AfterContextChanged(AzureStackLoginContextViewer sender)
        {
            AfterContextChanged?.Invoke(this);
        }


        private async Task _AzureStackContext_BeforeAzureTenantChange(AzureContext sender)
        {
            BeforeAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContext_AfterAzureTenantChange(AzureContext sender)
        {
            //await _AzureContextTargetARM.CopyContext(_AzureStackContext);

            AfterAzureTenantChange?.Invoke(sender);
        }

        private async Task _AzureContext_BeforeAzureSubscriptionChange(AzureContext sender)
        {
            //await SaveSubscriptionSettings(sender.AzureSubscription);
            //await _AzureContextTargetARM.SetSubscriptionContext(null);

            BeforeAzureSubscriptionChange?.Invoke(sender);
        }

        private async Task _AzureContext_AzureEnvironmentChanged(AzureContext sender)
        {
            AzureEnvironmentChanged?.Invoke(sender);
        }


        private async Task _AzureContext_UserAuthenticated(AzureContext sender)
        {
            this.IsSourceContextAuthenticated = true;
            UserAuthenticated?.Invoke(sender);
        }

        private async Task _AzureContext_BeforeUserSignOut()
        {
            //await SaveSubscriptionSettings(this._AzureStackContext.AzureSubscription);

            BeforeUserSignOut?.Invoke();
        }

        private async Task _AzureContext_AfterUserSignOut()
        {
            treeViewSourceResourceManager1.ResetForm();
            this.IsSourceContextAuthenticated = false;

            //if (_AzureContextTargetARM != null)
            //    await _AzureContextTargetARM.SetSubscriptionContext(null);

            //if (_AzureContextTargetARM != null)
            //    await _AzureContextTargetARM.Logout();

            //AzureStackLoginContextViewerARM.Enabled = false;
            //AzureStackLoginContextViewerARM.Refresh();

            AfterUserSignOut?.Invoke();
        }

        private async Task _AzureContext_AfterAzureSubscriptionChange(AzureContext sender)
        {
            try
            {
                treeViewSourceResourceManager1.ResetForm();

                if (sender.AzureSubscription != null)
                {
                    await sender.AzureSubscription.InitializeChildrenAsync();
                    await treeViewSourceResourceManager1.BindArmResources(sender.AzureSubscription, _TargetSettings);

                    //_AzureStackContext.AzureRetriever.SaveRestCache();
                    //        await ReadSubscriptionSettings(sender.AzureSubscription);
                }
            }
            catch (Exception exc)
            {
                UnhandledExceptionDialog unhandledException = new UnhandledExceptionDialog(_AzureStackContextSource.LogProvider, exc);
                unhandledException.ShowDialog();
            }

            _AzureStackContextSource.StatusProvider.UpdateStatus("Ready");

            AfterAzureSubscriptionChange?.Invoke(sender);
        }

        #endregion

    }
}
