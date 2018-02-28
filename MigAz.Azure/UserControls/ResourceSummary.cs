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

namespace MigAz.Azure.UserControls
{
    public partial class ResourceSummary : UserControl
    {
        private Core.MigrationTarget _MigrationTarget;
        private TargetTreeView _TargetTreeView;

        public ResourceSummary()
        {
            InitializeComponent();
        }

        public ResourceSummary(Core.MigrationTarget migrationTarget, TargetTreeView targetTreeView)
        {
            InitializeComponent();
            Bind(migrationTarget, targetTreeView);
        }

        public void Bind(Core.MigrationTarget migrationTarget, TargetTreeView targetTreeView, bool allowSelection = true, bool allowNone = true)
        {
            _MigrationTarget = migrationTarget;
            _TargetTreeView = targetTreeView;

            if (_MigrationTarget != null && _TargetTreeView != null)
            {
                pictureBox1.Image = _TargetTreeView.ImageList.Images[_MigrationTarget.ImageKey];
            }
            else
            {
                pictureBox1.Visible = false;
            }

            cmbResources.Items.Clear();
            if (allowNone)
                cmbResources.Items.Add("(None)");

            if (migrationTarget == null)
                cmbResources.SelectedIndex = 0;
            else
            {
                int itemIndex = cmbResources.Items.Add(migrationTarget);
                cmbResources.SelectedIndex = itemIndex;
            }

            cmbResources.Enabled = allowSelection;
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

        }
    }
}

