using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAzAWS.Forms
{
    public partial class AuthenticationForm : Form
    {
        private string accessKeyID;
        private string secretKeyID;
        public AuthenticationForm()
        {
            InitializeComponent();
        }
        public string GetAWSAccessKeyID()
        {
            return accessKeyID;
        }
        public string GetAWSSecretKeyID()
        {
            return secretKeyID;
        }
        private void continueButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            accessKeyID = accessKeyTextBox.Text.ToString().Trim(); 
            secretKeyID = secretKeyTextBox.Text.ToString().Trim();


            app.Default.AWSKey = accessKeyID;
            app.Default.AWSSecret = secretKeyID;
            app.Default.Save();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            accessKeyTextBox.Text = app.Default.AWSKey;
            secretKeyTextBox.Text = app.Default.AWSSecret;

        }
    }
}
