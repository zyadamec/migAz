using MigAz.Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIGAZ.Forms
{
    public partial class AzureContextASMDialog : Form
    {
        private AsmToArmForm _ParentForm;

        public AzureContextASMDialog()
        {
            InitializeComponent();
        }

        public async Task InitializeDialog(AsmToArmForm parentForm)
        {
            _ParentForm = parentForm;
            await this.azureLoginControl1.BindContext(parentForm.AzureContextSourceASM);
            parentForm.AzureContextSourceASM.AfterAzureSubscriptionChange += AzureContextSourceASM_AfterAzureSubscriptionChange;
        }

        private async Task AzureContextSourceASM_AfterAzureSubscriptionChange(AzureContext sender)
        {
            this.Close();
        }

        private void btnCloseDialog_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}




























//        private void AzureLoginControl_EnabledChanged(object sender, EventArgs e)
//        {
//            cboAzureEnvironment.Enabled = this.Enabled;
//            btnAuthenticate.Enabled = this.Enabled;
//            cmbSubscriptions.Enabled = this.Enabled && cmbSubscriptions.Items.Count > 0;
//        }
//    }
//}
