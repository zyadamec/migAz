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
using MigAz.Azure.Interface;
using MigAz.AzureStack.UserControls;

namespace MigAz.UserControls
{
    public partial class MigAzMigrationTargetSelection : UserControl
    {
        private UserControl _IMigrationSource;

        public UserControl MigrationSource
        {
            get
            {
                return _IMigrationSource;
            }
            internal set
            {
                if (value == null)
                {
                    _IMigrationSource = value;
                    return;
                }

                if (!value.GetType().GetInterfaces().Contains(typeof(IMigrationSourceUserControl)))
                    throw new ArgumentException("Must implement IMigrationSourceUserControl.");

                _IMigrationSource = value;

                IMigrationSourceUserControl migrationSourceUserControl = (IMigrationSourceUserControl)_IMigrationSource;

                bool isMigrationSourceReady = (_IMigrationSource != null && migrationSourceUserControl.IsSourceContextAuthenticated);

                if (_IMigrationSource == null)
                {
                    lblMigrationSourceStatus.Text = "Select Migration Source prior to setting Migration Target.";
                }
                else if (_IMigrationSource != null && !migrationSourceUserControl.IsSourceContextAuthenticated)
                {
                    lblMigrationSourceStatus.Text = "Authenticate to Migration Source prior to setting Migration Target.";
                }

                lblMigrationSourceStatus.Visible = !(isMigrationSourceReady);
                btnAzure.Enabled = (isMigrationSourceReady);
                btnAzureStack.Enabled = (isMigrationSourceReady);
            }
        }

        public delegate Task AfterMigrationTargetSelectedHandler(UserControl migrationTargetUserControl);
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
            AfterMigrationTargetSelected?.Invoke(new MigrationAzureStackTargetContext());
        }

        private void MigAzMigrationTargetSelection_Resize(object sender, EventArgs e)
        {
            groupBox1.Width = this.Width - 10;
        }
    }
}
