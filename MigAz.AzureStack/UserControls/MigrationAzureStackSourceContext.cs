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

namespace MigAz.AzureStack.UserControls
{
    public partial class MigrationAzureStackSourceContext : UserControl, IMigrationSourceUserControl
    {
        private bool _IsAuthenticated = false;
        private ILogProvider _LogProvider;

        public MigrationAzureStackSourceContext()
        {
            InitializeComponent();
        }

        public void Bind(ILogProvider logProvider)
        {
            _LogProvider = logProvider;
        }


        public bool IsSourceContextAuthenticated
        {
            get { return _IsAuthenticated; }
            set { _IsAuthenticated = value; }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            AzureStackContext azureStackContext = new AzureStackContext(_LogProvider);
            await azureStackContext.Login();
        }

        public Task UncheckMigrationTarget(MigrationTarget migrationTarget)
        {
            throw new NotImplementedException();
        }
    }
}
