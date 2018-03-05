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

namespace MigAz.Forms
{
    public partial class AzureEnvironmentDialog : Form
    {
        private List<AzureEnvironment> _DefaultAzureEnvironments;

        public AzureEnvironmentDialog()
        {
            InitializeComponent();
        }

        public void Bind(List<AzureEnvironment> defaultAzureEnvironments)
        {
            _DefaultAzureEnvironments = defaultAzureEnvironments;

            listBoxAzureEnvironments.Items.Clear();
            foreach (AzureEnvironment azureEnvironment in defaultAzureEnvironments)
            {
                listBoxAzureEnvironments.Items.Add(azureEnvironment);
            }

            if (listBoxAzureEnvironments.SelectedIndex < 0 && listBoxAzureEnvironments.Items.Count > 0)
                listBoxAzureEnvironments.SelectedIndex = 0;
        }

        private void listBoxAzureEnvironments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem == null)
            {
                cmbAzureEnvironmentType.SelectedIndex = 0;
                txtName.Text = String.Empty;
                txtLoginUrl.Text = String.Empty;
                txtGraphApiUrl.Text = String.Empty;
                txtASMManagementUrl.Text = String.Empty;
                txtARMManagementUrl.Text = String.Empty;
                txtStorageEndpoint.Text = String.Empty;
                txtBlobEndpoint.Text = String.Empty;
            }
            else
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;

                int azureEnvironmentTypeIndex = cmbAzureEnvironmentType.FindStringExact(azureEnvironment.AzureEnvironmentType.ToString());
                if (azureEnvironmentTypeIndex >= 0)
                    cmbAzureEnvironmentType.SelectedIndex = azureEnvironmentTypeIndex;
                else
                    cmbAzureEnvironmentType.SelectedIndex = 0;

                txtName.Text = azureEnvironment.Name;
                txtLoginUrl.Text = azureEnvironment.AzureLoginUrl;
                txtGraphApiUrl.Text = azureEnvironment.GraphApiUrl;
                txtASMManagementUrl.Text = azureEnvironment.ASMServiceManagementUrl;
                txtARMManagementUrl.Text = azureEnvironment.ARMServiceManagementUrl;
                txtStorageEndpoint.Text = azureEnvironment.StorageEndpointUrl;
                txtBlobEndpoint.Text = azureEnvironment.BlobEndpointUrl;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.Name = txtName.Text.Trim();
                listBoxAzureEnvironments.Items[listBoxAzureEnvironments.SelectedIndex] = listBoxAzureEnvironments.Items[listBoxAzureEnvironments.SelectedIndex];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AzureEnvironment azureEnvironment = new AzureEnvironment();
            listBoxAzureEnvironments.Items.Add(azureEnvironment);
        }

        private void btnDeleteAzureEnvironment_Click(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                listBoxAzureEnvironments.Items.Remove(listBoxAzureEnvironments.SelectedItem);
            }
        }

        private void cmbAzureEnvironmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
            azureEnvironment.AzureEnvironmentType = (AzureEnvironmentType)Enum.Parse(typeof(AzureEnvironmentType), cmbAzureEnvironmentType.SelectedItem.ToString());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
