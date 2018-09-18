// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class AvailabilitySetProperties : TargetPropertyControl
    {
        AvailabilitySet _AvailabilitySet;

        public AvailabilitySetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(AvailabilitySet availabilitySet, TargetTreeView targetTreeView)
        {
            try
            {
                this.IsBinding = true;

                _TargetTreeView = targetTreeView;
                _AvailabilitySet = availabilitySet;

                txtTargetName.Text = _AvailabilitySet.TargetName;
                upDownFaultDomains.Value = _AvailabilitySet.PlatformFaultDomainCount;
                upDownUpdateDomains.Value = _AvailabilitySet.PlatformUpdateDomainCount;

                foreach (VirtualMachine virtualMachine in _AvailabilitySet.TargetVirtualMachines)
                {
                    AddResourceSummary(new ResourceSummary(virtualMachine, _TargetTreeView));
                }
            }
            finally
            {
                this.IsBinding = false;
            }
        }

        private void AddResourceSummary(ResourceSummary resourceSummary)
        {
            if (pictureBox1.Controls.Count > 0)
            {
                resourceSummary.Top = pictureBox1.Controls[pictureBox1.Controls.Count - 1].Top + pictureBox1.Controls[pictureBox1.Controls.Count - 1].Height;
            }

            pictureBox1.Controls.Add(resourceSummary);
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _AvailabilitySet.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            this.RaisePropertyChangedEvent(_AvailabilitySet);
        }

        private void upDownFaultDomains_ValueChanged(object sender, EventArgs e)
        {
            _AvailabilitySet.PlatformFaultDomainCount = Convert.ToInt32(upDownFaultDomains.Value);

            this.RaisePropertyChangedEvent(_AvailabilitySet);
        }

        private void upDownUpdateDomains_ValueChanged(object sender, EventArgs e)
        {
            _AvailabilitySet.PlatformUpdateDomainCount = Convert.ToInt32(upDownUpdateDomains.Value);

            this.RaisePropertyChangedEvent(_AvailabilitySet);
        }
    }
}

