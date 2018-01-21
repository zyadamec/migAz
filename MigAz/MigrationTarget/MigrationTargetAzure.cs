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
using MigAz.Providers;

namespace MigAz.MigrationTarget
{
    public partial class MigrationTargetAzure : UserControl
    {
        private AzureContext _AzureContextTarget;
        private AzureGenerator _AzureGenerator;

        public delegate Task AfterTargetSelectedHandler(TreeNode sender);
        public event AfterTargetSelectedHandler AfterTargetSelected;

        public delegate Task AfterResourceValidationHandler();
        public event AfterResourceValidationHandler AfterResourceValidation;

        public MigrationTargetAzure()
        {
            InitializeComponent();

            treeTargetARM.AfterTargetSelected += TreeTargetARM_AfterTargetSelected;
            treeTargetARM.AfterResourceValidation += TreeTargetARM_AfterResourceValidation;
        }

        public TemplateGenerator TemplateGenerator
        {
            get
            {
                return _AzureGenerator;
            }
        }

        private async Task TreeTargetARM_AfterResourceValidation()
        {
            await AfterResourceValidation?.Invoke();
        }

        public ImageList ImageList
        {
            get { return treeTargetARM.ImageList; }
            set { treeTargetARM.ImageList = value; }
        }

        private void TreeTargetARM_AfterTargetSelected()
        {
            AfterTargetSelected?.Invoke(this.TargetTreeView.SelectedNode);
        }

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider, AppSettingsProvider appSettingsProvider, ITelemetryProvider telemetryProvider, PropertyPanel propertyPanel)
        {
            _AzureContextTarget = new AzureContext(logProvider, statusProvider, appSettingsProvider);
            //_AzureContextTarget.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            //_AzureContextTarget.UserAuthenticated += _AzureContext_UserAuthenticated;
            //_AzureContextTarget.BeforeAzureSubscriptionChange += _AzureContext_BeforeAzureSubscriptionChange;
            //_AzureContextTarget.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
            //_AzureContextTarget.BeforeUserSignOut += _AzureContext_BeforeUserSignOut;
            //_AzureContextTarget.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            //_AzureContextTarget.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
            //_AzureContextTarget.BeforeAzureTenantChange += _AzureContextSource_BeforeAzureTenantChange;

            await azureLoginContextViewerTarget.Bind(_AzureContextTarget);

            this.treeTargetARM.PropertyPanel = propertyPanel;
            this._AzureGenerator = new AzureGenerator(_AzureContextTarget.AzureSubscription, _AzureContextTarget.AzureSubscription, logProvider, statusProvider, telemetryProvider, appSettingsProvider);
        }

        public void Clear()
        {
            this.treeTargetARM.Clear();
        }

        public TargetTreeView TargetTreeView
        {
            get { return this.treeTargetARM; }
        }

        private void MigrationTargetAzure_Resize(object sender, EventArgs e)
        {
            this.treeTargetARM.Width = this.Width;
            this.treeTargetARM.Height = this.Height - 125;
            azureLoginContextViewerTarget.Width = this.Width;
        }
    }
}
