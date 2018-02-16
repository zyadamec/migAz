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
using MigAz.Core;
using MigAz.Azure;

namespace MigAz.AzureStack.UserControls
{
    public partial class MigrationAzureStackSourceContext : UserControl, IMigrationSourceUserControl
    {
        bool _IsAuthenticated = false;
        ILogProvider _LogProvider;
        IStatusProvider _StatusProvider;

        public MigrationAzureStackSourceContext()
        {
            InitializeComponent();
        }

        public async Task Bind(ILogProvider logProvider, IStatusProvider statusProvider)
        {
            _LogProvider = logProvider;
            _StatusProvider = statusProvider;

            AzureContext azureContext = new AzureContext(_LogProvider, _StatusProvider);
            AzureStackContext azureStackcontext = new AzureStackContext(azureContext);
            await this.azureStackLoginContextViewer1.Bind(azureStackcontext);

        }


        public bool IsSourceContextAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }

        public Task UncheckMigrationTarget(MigrationTarget migrationTarget)
        {
            throw new NotImplementedException();
        }

        private void MigrationAzureStackSourceContext_Resize(object sender, EventArgs e)
        {
            this.azureStackLoginContextViewer1.Width = this.Width;
        }
    }
}
