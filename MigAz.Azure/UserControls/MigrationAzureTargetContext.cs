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

namespace MigAz.Azure.UserControls
{
    public partial class MigrationAzureTargetContext : UserControl
    {
        private AzureContext _AzureContextTarget;
        private AzureGenerator _AzureGenerator;

        public delegate Task AfterTargetSelectedHandler(TreeNode sender);
        public event AfterTargetSelectedHandler AfterTargetSelected;

        public delegate void AfterExportArtifactRefreshHandler(TargetTreeView sender);
        public event AfterExportArtifactRefreshHandler AfterExportArtifactRefresh;

        public MigrationAzureTargetContext()
        {
            InitializeComponent();

            treeTargetARM.AfterTargetSelected += TreeTargetARM_AfterTargetSelected;
            treeTargetARM.AfterExportArtifactRefresh += TreeTargetARM_AfterExportArtifactRefresh;
        }

        public TemplateGenerator TemplateGenerator
        {
            get
            {
                return _AzureGenerator;
            }
        }

        private async Task TreeTargetARM_AfterExportArtifactRefresh()
        {
            AfterExportArtifactRefresh?.Invoke(this.treeTargetARM);
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

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider, ITelemetryProvider telemetryProvider, PropertyPanel propertyPanel)
        {
            _AzureContextTarget = new AzureContext(logProvider, statusProvider, null);
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
            this._AzureGenerator = new AzureGenerator(_AzureContextTarget.AzureSubscription, _AzureContextTarget.AzureSubscription, logProvider, statusProvider, telemetryProvider);
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
