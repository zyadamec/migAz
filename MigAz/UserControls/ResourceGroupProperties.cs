using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure;
using MigAz.Azure.Arm;
using System.Net;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.UserControls.Migrators;
using MigAz.Core.Interface;

namespace MigAz.UserControls
{
    public partial class ResourceGroupProperties : UserControl
    {
        private AsmToArm _ParentForm;
        private TreeNode _ResourceGroupNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public ResourceGroupProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AsmToArm parentForm, TreeNode resourceGroupNode)
        {
            _ParentForm = parentForm;
            _ResourceGroupNode = resourceGroupNode;

            ResourceGroup armResourceGroup = (ResourceGroup) _ResourceGroupNode.Tag;

            txtName.Text = armResourceGroup.TargetName;

            try
            {
                cboTargetLocation.Items.Clear();
                foreach (Azure.Arm.Location armLocation in await _ParentForm.AzureContextTargetARM.AzureRetriever.GetAzureARMLocations())
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
                foreach (Azure.Asm.Location asmLocation in await _ParentForm.AzureContextTargetARM.AzureRetriever.GetAzureASMLocations())
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

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            ResourceGroup armResourceGroup = (ResourceGroup)_ResourceGroupNode.Tag;

            armResourceGroup.TargetName = txtSender.Text;
            _ResourceGroupNode.Text = armResourceGroup.GetFinalTargetName();
            _ResourceGroupNode.Name = armResourceGroup.TargetName;

            PropertyChanged();
        }

        private void cboTargetLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;

            ResourceGroup armResourceGroup = (ResourceGroup)_ResourceGroupNode.Tag;
            armResourceGroup.TargetLocation = (ILocation) cmbSender.SelectedItem;

            PropertyChanged();
        }
    }
}
