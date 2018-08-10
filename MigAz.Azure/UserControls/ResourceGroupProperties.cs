// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
    public partial class ResourceGroupProperties : TargetPropertyControl
    {
        private ResourceGroup _ResourceGroup;

        public ResourceGroupProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(ResourceGroup resourceGroup, TargetTreeView targetTreeView)
        {
            try
            {
                this.IsBinding = true;
                _ResourceGroup = resourceGroup;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = resourceGroup.TargetName;

                cboTargetLocation.Items.Clear();
                if (targetTreeView.TargetSubscription != null && targetTreeView.TargetSubscription.Locations != null && targetTreeView.TargetSubscription.Locations.Count() > 0)
                {
                    foreach (Arm.Location armLocation in targetTreeView.TargetSubscription.Locations.OrderBy(a => a.DisplayName))
                    {
                        cboTargetLocation.Items.Add(armLocation);
                    }
                }
                else
                {
                    cboTargetLocation.Visible = false;
                    lblTargetContext.Visible = true;
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
                this.IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _ResourceGroup.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            this.RaisePropertyChangedEvent(_ResourceGroup);
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

            this.RaisePropertyChangedEvent(_ResourceGroup);
        }

    }
}

