using MigAz.Azure;
using MigAz.AzureStack;
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
        private AzureRetriever _AzureRetriever;
        private List<AzureEnvironment> _DefaultAzureEnvironments;

        public AzureEnvironmentDialog()
        {
            InitializeComponent();
        }

        public void Bind(AzureRetriever azureRetriever, List<AzureEnvironment> defaultAzureEnvironments)
        {
            _AzureRetriever = azureRetriever;
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

                SetAzureEnvironmentFieldEnabled(false);
                btnCloneAzureEnvironment.Enabled = false;
                btnDeleteAzureEnvironment.Enabled = false;
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

                SetAzureEnvironmentFieldEnabled(azureEnvironment.IsUserDefined);
                btnCloneAzureEnvironment.Enabled = true;
                btnDeleteAzureEnvironment.Enabled = azureEnvironment.IsUserDefined;
            }
        }

        private void SetAzureEnvironmentFieldEnabled(bool enabled)
        {
            txtName.Enabled = enabled;
            cmbAzureEnvironmentType.Enabled = enabled;
            txtLoginUrl.Enabled = enabled;
            txtGraphApiUrl.Enabled = enabled;
            txtASMManagementUrl.Enabled = enabled;
            txtARMManagementUrl.Enabled = enabled;
            txtStorageEndpoint.Enabled = enabled;
            txtBlobEndpoint.Enabled = enabled;
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
            txtAzureStackAdminManagementUrl.Text = azureEnvironment.AzureStackAdminManagementUrl;
            txtAzureStackAdminManagementUrl.Enabled = azureEnvironment.AzureEnvironmentType == AzureEnvironmentType.AzureStack;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AzureEnvironment azureEnvironment = new AzureEnvironment();
            listBoxAzureEnvironments.Items.Add(azureEnvironment);
            listBoxAzureEnvironments.SelectedIndex = listBoxAzureEnvironments.Items.IndexOf(azureEnvironment);
        }

        private void btnCloneAzureEnvironment_Click(object sender, EventArgs e)
        {
            AzureEnvironment selectedAzureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
            AzureEnvironment clonedAzureEnvironment = new AzureEnvironment(selectedAzureEnvironment);
            listBoxAzureEnvironments.Items.Add(clonedAzureEnvironment);
            listBoxAzureEnvironments.SelectedIndex = listBoxAzureEnvironments.Items.IndexOf(clonedAzureEnvironment);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private async void btnQueryAzureStackMetadata_Click(object sender, EventArgs e)
        {
            // Obtain Azure Stack Envrionment Metadata
            AzureStackEndpoints azureStackEndpoints = await AzureStackContext.LoadMetadataEndpoints(_AzureRetriever, txtAzureStackAdminManagementUrl.Text);
            txtLoginUrl.Text = azureStackEndpoints.LoginEndpoint;
            txtGraphApiUrl.Text = azureStackEndpoints.GraphEndpoint;
        }
    }
}
