using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Forms
{
    public partial class NewVersionAvailableDialog : Form
    {
        public NewVersionAvailableDialog()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://aka.ms/MigAz/Release");
        }

        public void Bind(string currentVersion, string newVersion)
        {
            lblCurrentVersion.Text = currentVersion;
            lblNewVersion.Text = newVersion;
        }
    }
}
