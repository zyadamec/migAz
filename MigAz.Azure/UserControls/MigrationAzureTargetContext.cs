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
    public partial class MigrationAzureTargetContext : IMigrationTargetUserControl
    {
        private AzureContext _AzureContextTarget;
        private AzureGenerator _AzureGenerator;

        public MigrationAzureTargetContext()
        {
            InitializeComponent();
        }

        public TemplateGenerator TemplateGenerator
        {
            get
            {
                return _AzureGenerator;
            }
        }

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider, ITelemetryProvider telemetryProvider, TargetSettings targetSettings, PropertyPanel propertyPanel)
        {
            _AzureContextTarget = new AzureContext(logProvider, statusProvider, targetSettings);
            //_AzureContextTarget.AzureEnvironmentChanged += _AzureContext_AzureEnvironmentChanged;
            //_AzureContextTarget.UserAuthenticated += _AzureContext_UserAuthenticated;
            //_AzureContextTarget.BeforeAzureSubscriptionChange += _AzureContext_BeforeAzureSubscriptionChange;
            //_AzureContextTarget.AfterAzureSubscriptionChange += _AzureContext_AfterAzureSubscriptionChange;
            //_AzureContextTarget.BeforeUserSignOut += _AzureContext_BeforeUserSignOut;
            //_AzureContextTarget.AfterUserSignOut += _AzureContext_AfterUserSignOut;
            //_AzureContextTarget.AfterAzureTenantChange += _AzureContext_AfterAzureTenantChange;
            //_AzureContextTarget.BeforeAzureTenantChange += _AzureContextSource_BeforeAzureTenantChange;

            await azureLoginContextViewerTarget.Bind(_AzureContextTarget);

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

        private void MigrationTargetAzure_Resize(object sender, EventArgs e)
        {
            azureLoginContextViewerTarget.Width = this.Width - 10;
        }
    }
}
