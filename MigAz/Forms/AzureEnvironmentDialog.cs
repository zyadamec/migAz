using MigAz.Azure;
using MigAz.Azure.AzureStack;
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
        private bool _istxtNameTextChangedEvent = false;
        private AzureRetriever _AzureRetriever;
        private List<AzureEnvironment> _DefaultAzureEnvironments;
        private List<AzureEnvironment> _UserDefinedAzureEnvironments;

        public AzureEnvironmentDialog()
        {
            InitializeComponent();
        }

        public void Bind(AzureRetriever azureRetriever, List<AzureEnvironment> defaultAzureEnvironments, ref List<AzureEnvironment> userDefinedAzureEnvironments)
        {
            _AzureRetriever = azureRetriever;
            _DefaultAzureEnvironments = defaultAzureEnvironments;
            _UserDefinedAzureEnvironments = userDefinedAzureEnvironments;

            listBoxAzureEnvironments.Items.Clear();
            foreach (AzureEnvironment azureEnvironment in defaultAzureEnvironments)
            {
                listBoxAzureEnvironments.Items.Add(azureEnvironment);
            }

            foreach (AzureEnvironment azureEnvironment in userDefinedAzureEnvironments)
            {
                listBoxAzureEnvironments.Items.Add(azureEnvironment);
            }

            if (listBoxAzureEnvironments.SelectedIndex < 0 && listBoxAzureEnvironments.Items.Count > 0)
                listBoxAzureEnvironments.SelectedIndex = 0;
        }

        private void listBoxAzureEnvironments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_istxtNameTextChangedEvent)  // Rebind to refresh the name from the textbox causes double validation, only bind if not in the Name Text Changed event.
            {
                if (listBoxAzureEnvironments.SelectedItem == null)
                {
                    cmbAzureEnvironmentType.SelectedIndex = 0;
                    txtName.Text = String.Empty;
                    txtAzureStackAdminManagementUrl.Text = String.Empty;
                    txtLoginUrl.Text = String.Empty;
                    txtAdTenant.Text = String.Empty;
                    txtGraphApiUrl.Text = String.Empty;
                    txtASMManagementUrl.Text = String.Empty;
                    txtARMManagementUrl.Text = String.Empty;
                    txtStorageEndpoint.Text = String.Empty;
                    txtBlobEndpoint.Text = String.Empty;
                    txtSQLDatabaseDnsSuffix.Text = String.Empty;
                    txtTrafficManagerDnsSuffix.Text = String.Empty;
                    txtAzureKeyVaultDnsSuffix.Text = String.Empty;

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
                    txtAzureStackAdminManagementUrl.Text = azureEnvironment.AzureStackAdminManagementUrl;
                    txtLoginUrl.Text = azureEnvironment.ActiveDirectoryEndpoint;
                    txtAdTenant.Text = azureEnvironment.AdTenant;
                    txtGraphApiUrl.Text = azureEnvironment.GraphEndpoint;
                    txtASMManagementUrl.Text = azureEnvironment.ServiceManagementUrl;
                    txtARMManagementUrl.Text = azureEnvironment.ResourceManagerEndpoint;
                    txtStorageEndpoint.Text = azureEnvironment.StorageEndpointSuffix;
                    txtBlobEndpoint.Text = azureEnvironment.BlobEndpointUrl;
                    txtSQLDatabaseDnsSuffix.Text = azureEnvironment.SqlDatabaseDnsSuffix;
                    txtTrafficManagerDnsSuffix.Text = azureEnvironment.TrafficManagerDnsSuffix;
                    txtAzureKeyVaultDnsSuffix.Text = azureEnvironment.AzureKeyVaultDnsSuffix;

                    SetAzureEnvironmentFieldEnabled(azureEnvironment.IsUserDefined);
                    btnCloneAzureEnvironment.Enabled = true;
                    btnDeleteAzureEnvironment.Enabled = azureEnvironment.IsUserDefined;
                    txtName.Focus();
                }
            }
        }

        private void SetAzureEnvironmentFieldEnabled(bool enabled)
        {
            txtName.Enabled = enabled;
            cmbAzureEnvironmentType.Enabled = enabled;

            txtLoginUrl.Enabled = enabled;
            txtAdTenant.Enabled = enabled;
            txtGraphApiUrl.Enabled = enabled;
            txtASMManagementUrl.Enabled = enabled;
            txtARMManagementUrl.Enabled = enabled;
            txtStorageEndpoint.Enabled = enabled;
            txtBlobEndpoint.Enabled = enabled;
            txtSQLDatabaseDnsSuffix.Enabled = enabled;
            txtTrafficManagerDnsSuffix.Enabled = enabled;
            txtAzureKeyVaultDnsSuffix.Enabled = enabled;

            if (!enabled) // We only allow these to set upon true based on specific criteria
            {
                btnQueryAzureStackMetadata.Enabled = enabled;
                txtAzureStackAdminManagementUrl.Enabled = enabled;
            }
        }

        

        private void btnDeleteAzureEnvironment_Click(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                if (MessageBox.Show("Are you sure you want to delete Azure Environment '" + azureEnvironment.Name + "'?", "Delete Azure Environment", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    listBoxAzureEnvironments.Items.Remove(listBoxAzureEnvironments.SelectedItem);
                    _UserDefinedAzureEnvironments.Remove(azureEnvironment);
                }
            }
        }

        private void cmbAzureEnvironmentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.AzureEnvironmentType = (AzureEnvironmentType)Enum.Parse(typeof(AzureEnvironmentType), cmbAzureEnvironmentType.SelectedItem.ToString());
                txtAzureStackAdminManagementUrl.Text = azureEnvironment.AzureStackAdminManagementUrl;
                txtAzureStackAdminManagementUrl.Enabled = azureEnvironment.AzureEnvironmentType == AzureEnvironmentType.AzureStack;
                btnQueryAzureStackMetadata.Enabled = azureEnvironment.AzureEnvironmentType == AzureEnvironmentType.AzureStack;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            AzureEnvironment azureEnvironment = new AzureEnvironment();
            listBoxAzureEnvironments.Items.Add(azureEnvironment);
            listBoxAzureEnvironments.SelectedIndex = listBoxAzureEnvironments.Items.IndexOf(azureEnvironment);
            _UserDefinedAzureEnvironments.Add(azureEnvironment);
            txtName.Focus();
        }

        private void btnCloneAzureEnvironment_Click(object sender, EventArgs e)
        {
            AzureEnvironment selectedAzureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
            AzureEnvironment clonedAzureEnvironment = new AzureEnvironment(selectedAzureEnvironment);
            listBoxAzureEnvironments.Items.Add(clonedAzureEnvironment);
            listBoxAzureEnvironments.SelectedIndex = listBoxAzureEnvironments.Items.IndexOf(clonedAzureEnvironment);
            _UserDefinedAzureEnvironments.Add(clonedAzureEnvironment);
            txtName.Focus();
        }
   
        

        private int GetEnvironmentNameCount(string environmentName)
        {
            int nameCount = 0;
            foreach (object objAzureEnvironment in listBoxAzureEnvironments.Items)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)objAzureEnvironment;
                if (String.Compare(azureEnvironment.Name, environmentName.Trim(), true) == 0)
                    nameCount++;
            }

            return nameCount;
        }

        private async void btnQueryAzureStackMetadata_Click(object sender, EventArgs e)
        {
            // Obtain Azure Stack Envrionment Metadata
            try
            {
                AzureStackEndpoints azureStackEndpoints = await AzureStackContext.LoadMetadataEndpoints(_AzureRetriever, txtAzureStackAdminManagementUrl.Text);

                txtLoginUrl.Text = azureStackEndpoints.LoginEndpoint;
                txtGraphApiUrl.Text = azureStackEndpoints.GraphEndpoint;
            }
            catch (System.Net.WebException exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Field Events

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _istxtNameTextChangedEvent = true;

                if (listBoxAzureEnvironments.SelectedItem != null)
                {
                    AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                    azureEnvironment.Name = txtName.Text;
                    listBoxAzureEnvironments.Items[listBoxAzureEnvironments.SelectedIndex] = listBoxAzureEnvironments.Items[listBoxAzureEnvironments.SelectedIndex];
                }
            }
            finally
            {
                _istxtNameTextChangedEvent = false;
            }
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            if (sender.GetType() == typeof(TextBox))
            {
                TextBox senderTextBox = (TextBox)sender;

                if (senderTextBox.Text.Trim() == String.Empty)
                {
                    e.Cancel = true;
                    MessageBox.Show("Name is required.", "Azure Environment");
                }
                else
                {
                    if (GetEnvironmentNameCount(senderTextBox.Text) > 1)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Azure Anvironment Name must be unique.", "Azure Environment");
                    }
                }
            }
        }

        private void txtAzureStackAdminManagementUrl_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.AzureStackAdminManagementUrl = txtAzureStackAdminManagementUrl.Text;
            }
        }


        private void txtLoginUrl_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.ActiveDirectoryEndpoint = txtLoginUrl.Text;
            }
        }

        private void txtGraphApiUrl_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.GraphEndpoint = txtGraphApiUrl.Text;
            }
        }

        private void txtASMManagementUrl_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.ServiceManagementUrl = txtASMManagementUrl.Text;
            }
        }

        private void txtARMManagementUrl_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.ResourceManagerEndpoint = txtARMManagementUrl.Text;
            }
        }

        private void txtStorageEndpoint_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.StorageEndpointSuffix = txtStorageEndpoint.Text;
            }
        }

        private void txtBlobEndpoint_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.BlobEndpointUrl = txtBlobEndpoint.Text;
            }
        }

        private void txtAdTenant_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.AdTenant = txtAdTenant.Text;
            }
        }

        private void txtSQLDatabaseDnsSuffix_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.SqlDatabaseDnsSuffix = txtSQLDatabaseDnsSuffix.Text;
            }
        }

        private void txtTrafficManagerDnsSuffix_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.TrafficManagerDnsSuffix = txtTrafficManagerDnsSuffix.Text;
            }
        }

        private void txtAzureKeyVaultDnsSuffix_TextChanged(object sender, EventArgs e)
        {
            if (listBoxAzureEnvironments.SelectedItem != null)
            {
                AzureEnvironment azureEnvironment = (AzureEnvironment)listBoxAzureEnvironments.SelectedItem;
                azureEnvironment.AzureKeyVaultDnsSuffix = txtAzureKeyVaultDnsSuffix.Text;
            }
        }

        #endregion
    }
}
