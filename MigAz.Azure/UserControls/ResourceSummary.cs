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
using MigAz.Core.Interface;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class ResourceSummary<T> : UserControl
    {
        private bool _IsBinding = false;
        private T _MigrationTarget;
        private TargetTreeView _TargetTreeView;

        public delegate Task AfterMigrationTargetChangedHandler<T>(ResourceSummary<T> sender, T selectedResource);
        public event AfterMigrationTargetChangedHandler<T> AfterMigrationTargetChanged;


        public ResourceSummary()
        {
            InitializeComponent();
        }

        public ResourceSummary(T migrationTarget, TargetTreeView targetTreeView)
        {
            InitializeComponent();
            Bind(migrationTarget, targetTreeView);
        }


        public void Bind(T migrationTarget, TargetTreeView targetTreeView, bool allowSelection = false, bool allowNone = false, List<T> allowedResources = null)
        {
            _IsBinding = true;

            try
            {
                _MigrationTarget = migrationTarget;
                _TargetTreeView = targetTreeView;

                if (allowSelection)
                {
                    cmbResources.Enabled = allowSelection;
                    cmbResources.Visible = allowSelection;
                    lblResourceText.Enabled = !cmbResources.Enabled;
                    lblResourceText.Visible = !cmbResources.Visible;

                    cmbResources.Items.Clear();
                    if (allowNone)
                        cmbResources.Items.Add("(None)");

                    if (allowedResources != null)
                    {
                        foreach (T allowedMigrationTargetResource in allowedResources)
                        {
                            cmbResources.Items.Add(allowedMigrationTargetResource);
                        }
                    }

                    if (migrationTarget == null)
                    {
                        if (cmbResources.Items.Count > 0)
                            cmbResources.SelectedIndex = 0;
                    }
                    else
                    {
                        int itemIndex = -1;
                        if (cmbResources.Items.Contains(migrationTarget))
                            itemIndex = cmbResources.Items.IndexOf(migrationTarget);
                        else
                            itemIndex = cmbResources.Items.Add(migrationTarget);

                        if (itemIndex > -1)
                            cmbResources.SelectedIndex = itemIndex;
                    }
                }
                else
                {
                    cmbResources.Visible = false;

                    pictureBox1.Visible = migrationTarget != null;
                    lblResourceText.Visible = migrationTarget != null;

                    if (migrationTarget == null)
                        lblResourceText.Text = "(None)";
                    else
                        lblResourceText.Text = migrationTarget.ToString();
                }

                pictureBox1.Image = _TargetTreeView.ImageList.Images[Core.MigrationTarget.GetImageKey(typeof(T))];
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void ResourceSummary_Click(object sender, EventArgs e)
        {
            if (_TargetTreeView != null && _MigrationTarget != null)
                _TargetTreeView.SeekAlertSource(_MigrationTarget);
        }

        private void cmbResources_Click(object sender, EventArgs e)
        {
            if (!cmbResources.Enabled)
                ResourceSummary_Click(this, e);
        }

        private void cmbResources_SelectedIndexChanged(object sender, EventArgs e)
        {
            T selectedMigrationTargetResource = default(T);

            if (cmbResources.SelectedItem != null && cmbResources.SelectedItem.GetType().BaseType == typeof(Core.MigrationTarget))
                selectedMigrationTargetResource = (T)cmbResources.SelectedItem;

            if (!_IsBinding)
                AfterMigrationTargetChanged?.Invoke(this, selectedMigrationTargetResource);
        }

        internal class AfterMigrationTargetChangedHandler
        {
            private Func<ResourceSummary<AvailabilitySet>, AvailabilitySet, Task> availabilitySetSummary_AfterMigrationTargetChanged;

            public AfterMigrationTargetChangedHandler(Func<ResourceSummary<AvailabilitySet>, AvailabilitySet, Task> availabilitySetSummary_AfterMigrationTargetChanged)
            {
                this.availabilitySetSummary_AfterMigrationTargetChanged = availabilitySetSummary_AfterMigrationTargetChanged;
            }
        }
    }
}

