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

        public void Bind(Core.MigrationTarget migrationTarget, TargetTreeView targetTreeView)
        {
            _MigrationTarget = migrationTarget;
            _TargetTreeView = targetTreeView;

            if (_MigrationTarget != null && _TargetTreeView != null)
            {
                pictureBox1.Image = _TargetTreeView.ImageList.Images[_MigrationTarget.ImageKey];
                label1.Text = _MigrationTarget.ToString();
            }
            else
            {
                pictureBox1.Visible = false;
                label1.Visible = false;
            }
        }

        private void ResourceSummary_Click(object sender, EventArgs e)
        {
            if (_TargetTreeView != null && _MigrationTarget != null)
                _TargetTreeView.SeekAlertSource(_MigrationTarget);
        }
    }
}
