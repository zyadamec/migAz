using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using MigAz.Azure.Interface;
using MigAz.UserControls.Migrators;
using MigAz.Azure;

namespace MigAz.UserControls
{
    public partial class ResourceGroupProperties : UserControl
    {
        private AzureContext _AzureContext;
        private TreeNode _ResourceGroupNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public ResourceGroupProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AzureContext azureContext, TreeNode resourceGroupNode)
        {
            _AzureContext = azureContext;
            _ResourceGroupNode = resourceGroupNode;

            Azure.MigrationTarget.ResourceGroup armResourceGroup = (Azure.MigrationTarget.ResourceGroup) _ResourceGroupNode.Tag;

            txtTargetName.Text = armResourceGroup.TargetName;

            try
            {
                cboTargetLocation.Items.Clear();
                foreach (Azure.Arm.Location armLocation in await azureContext.AzureRetriever.GetAzureARMLocations())
                {
                    cboTargetLocation.Items.Add(armLocation);
                }
            }
            catch (WebException)
            {
                // We are trying to load the ARM defined subscription locations above first; however, this as of Feb 24 2017, this ARM query
                // does not succeed (503 Forbidden) across all Azure Environments.  For example, it works in Azure Commercial, but Azure US Gov
                // is not yet update to support this call.  In the event the ARM location query fails, we will default to using ASM Location query.

                cboTargetLocation.Items.Clear();
                foreach (Azure.Asm.Location asmLocation in await azureContext.AzureRetriever.GetAzureASMLocations())
                {
                    cboTargetLocation.Items.Add(asmLocation);
                }
            }

            if (armResourceGroup.TargetLocation != null)
            {
                foreach (Azure.Arm.Location armLocation in cboTargetLocation.Items)
                {
                    if (armLocation.Name == armResourceGroup.TargetLocation.Name)
                        cboTargetLocation.SelectedItem = armLocation;
                }
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.ResourceGroup armResourceGroup = (Azure.MigrationTarget.ResourceGroup)_ResourceGroupNode.Tag;

            armResourceGroup.TargetName = txtSender.Text;
            _ResourceGroupNode.Text = armResourceGroup.ToString();
            _ResourceGroupNode.Name = armResourceGroup.ToString();

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cboTargetLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;

            Azure.MigrationTarget.ResourceGroup armResourceGroup = (Azure.MigrationTarget.ResourceGroup)_ResourceGroupNode.Tag;
            armResourceGroup.TargetLocation = (ILocation) cmbSender.SelectedItem;

            PropertyChanged();
        }
    }
}
