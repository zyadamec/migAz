using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using MigAz.Azure.Interface;
using System.Linq;
using System.Collections.Generic;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class ResourceGroupProperties : UserControl
    {
        private AzureContext _AzureContext;
        private ResourceGroup _ResourceGroup;
        private TargetTreeView _TargetTreeView;
        private bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public ResourceGroupProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AzureContext azureContext, ResourceGroup resourceGroup, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                _AzureContext = azureContext;
                _ResourceGroup = resourceGroup;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = resourceGroup.TargetName;

                try
                {
                    cboTargetLocation.Items.Clear();
                    if (azureContext != null && azureContext.AzureRetriever != null && azureContext.AzureRetriever.SubscriptionContext != null)
                    {
                        List<Arm.Location> armLocations = await azureContext.AzureSubscription.GetAzureARMLocations(_AzureContext);

                        foreach (Azure.Arm.Location armLocation in armLocations.OrderBy(a => a.DisplayName))
                        {
                            cboTargetLocation.Items.Add(armLocation);
                        }
                    }
                    else
                    {
                        cboTargetLocation.Visible = false;
                        lblTargetContext.Visible = true;
                    }
                }
                catch (WebException)
                {
                    // We are trying to load the ARM defined subscription locations above first; however, this as of Feb 24 2017, this ARM query
                    // does not succeed (503 Forbidden) across all Azure Environments.  For example, it works in Azure Commercial, but Azure US Gov
                    // is not yet update to support this call.  In the event the ARM location query fails, we will default to using ASM Location query.

                    cboTargetLocation.Items.Clear();

                    if (azureContext.AzureRetriever.SubscriptionContext != null)
                    {
                        foreach (Azure.Asm.Location asmLocation in await azureContext.AzureSubscription.GetAzureASMLocations(_AzureContext))
                        {
                            cboTargetLocation.Items.Add(asmLocation);
                        }
                    }
                }

                if (resourceGroup.TargetLocation != null)
                {
                    foreach (Azure.Arm.Location armLocation in cboTargetLocation.Items)
                    {
                        if (armLocation.Name == resourceGroup.TargetLocation.Name)
                            cboTargetLocation.SelectedItem = armLocation;
                    }
                }
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _ResourceGroup.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged?.Invoke();
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

            _ResourceGroup.TargetLocation = (Arm.Location) cmbSender.SelectedItem;

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

    }
}
