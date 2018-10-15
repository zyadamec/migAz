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
using MigAz.Azure.Core;
using MigAz.Azure.Core.Interface;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class ResourceSummary : UserControl
    {
        private bool _IsBinding = false;
        private Core.MigrationTarget _MigrationTarget;
        private TargetTreeView _TargetTreeView;

        public delegate Task AfterMigrationTargetChangedHandler(ResourceSummary sender, Core.MigrationTarget selectedResource);
        public event AfterMigrationTargetChangedHandler AfterMigrationTargetChanged;


        public ResourceSummary()
        {
            InitializeComponent();
        }

        public ResourceSummary(Core.MigrationTarget migrationTarget, TargetTreeView targetTreeView)
        {
            InitializeComponent();
            Bind(migrationTarget, targetTreeView);
        }


        public void Bind(Core.MigrationTarget migrationTarget, TargetTreeView targetTreeView, bool allowSelection = false, bool allowNone = false, List<Core.MigrationTarget> allowedResources = null)
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
                        foreach (Core.MigrationTarget allowedMigrationTargetResource in allowedResources)
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

                if (migrationTarget == null)
                    pictureBox1.Image = null;
                else
                    pictureBox1.Image = _TargetTreeView.ImageList.Images[Core.MigrationTarget.GetImageKey(migrationTarget.GetType())];
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
            Core.MigrationTarget selectedMigrationTargetResource = null; 

            if (cmbResources.SelectedItem != null && cmbResources.SelectedItem.GetType().BaseType == typeof(Core.MigrationTarget))
                selectedMigrationTargetResource = (Core.MigrationTarget)cmbResources.SelectedItem;

            if (!_IsBinding)
                AfterMigrationTargetChanged?.Invoke(this, selectedMigrationTargetResource);
        }
    }
}

