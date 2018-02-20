using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Interface;
using MigAz.AzureStack.UserControls;
using MigAz.Azure.UserControls;
using MigAz.AWS.UserControls;

namespace MigAz.UserControls
{
    public partial class MigAzMigrationSourceSelection : UserControl
    {
        public delegate void AfterMigrationSourceSelectedHandler(UserControl migrationSourceUserControl);
        public event AfterMigrationSourceSelectedHandler AfterMigrationSourceSelected;

        public MigAzMigrationSourceSelection()
        {
            InitializeComponent();
        }

        private void MigAzMigrationSourceSelection_Resize(object sender, EventArgs e)
        {
            groupBox1.Width = this.Width - 5;
            groupBox1.Height = this.Height - 5;
        }

        private void btnAzure_Click(object sender, EventArgs e)
        {
            AfterMigrationSourceSelected?.Invoke(new MigrationAzureSourceContext());
        }

        private void btnAzureStack_Click(object sender, EventArgs e)
        {
            AfterMigrationSourceSelected?.Invoke(new MigrationAzureStackSourceContext());
        }

        private void btnAmazonWebServices_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AWS Source work in progress from old version, not yet functional.");
            AfterMigrationSourceSelected?.Invoke(new MigrationAWSSourceContext());
        }
        
    }
}
