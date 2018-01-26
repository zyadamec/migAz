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
using MigAz.Azure.UserControls;

namespace MigAz.UserControls
{
    public partial class MigAzMigrationTargetSelection : UserControl
    {
        public delegate void AfterMigrationTargetSelectedHandler(IMigrationTargetUserControl migrationTargetUserControl);
        public event AfterMigrationTargetSelectedHandler AfterMigrationTargetSelected;

        public MigAzMigrationTargetSelection()
        {
            InitializeComponent();
        }

        private void btnAzure_Click(object sender, EventArgs e)
        {
            AfterMigrationTargetSelected?.Invoke(new MigrationAzureTargetContext());
        }

        private void btnAzureStack_Click(object sender, EventArgs e)
        {
            AfterMigrationTargetSelected?.Invoke(null);
        }
    }
}
