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
        private IMigrationTarget _MigrationTarget;
        private TargetTreeView _TargetTreeView;

        private ResourceSummary()
        {
            InitializeComponent();
        }

        public ResourceSummary(IMigrationTarget migrationTarget, TargetTreeView targetTreeView)
        {
            InitializeComponent();
            _MigrationTarget = migrationTarget;
            _TargetTreeView = targetTreeView;

            if (_MigrationTarget.GetType() == typeof(Azure.MigrationTarget.Disk))
                pictureBox1.Image = _TargetTreeView.ImageList.Images["Disk"];
            else if (_MigrationTarget.GetType() == typeof(Azure.MigrationTarget.NetworkInterface))
                pictureBox1.Image = _TargetTreeView.ImageList.Images["NetworkInterface"];

            label1.Text = _MigrationTarget.ToString();
        }

        private void ResourceSummary_Click(object sender, EventArgs e)
        {
            _TargetTreeView.SeekAlertSource(_MigrationTarget);
        }
    }
}
