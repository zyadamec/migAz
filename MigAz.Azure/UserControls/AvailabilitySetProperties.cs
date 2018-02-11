using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Asm;
using MigAz.Azure.Arm;

namespace MigAz.Azure.UserControls
{
    public partial class AvailabilitySetProperties : UserControl
    {
        TargetTreeView _TargetTreeView;
        TreeNode _armAvailabilitySetNode;
        bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public AvailabilitySetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TargetTreeView targetTreeView, TreeNode availabilitySetNode)
        {
            try
            {
                _IsBinding = true;

                _TargetTreeView = targetTreeView;
                _armAvailabilitySetNode = availabilitySetNode;

                Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;
                txtTargetName.Text = targetAvailabilitySet.TargetName;
                upDownFaultDomains.Value = targetAvailabilitySet.PlatformFaultDomainCount;
                upDownUpdateDomains.Value = targetAvailabilitySet.PlatformUpdateDomainCount;

                foreach (Azure.MigrationTarget.VirtualMachine virtualMachine in targetAvailabilitySet.TargetVirtualMachines)
                {
                    AddResourceSummary(new ResourceSummary(virtualMachine, _TargetTreeView));
                }
            }
            finally
            {
                _IsBinding = false;
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

            Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;

            targetAvailabilitySet.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);
            _armAvailabilitySetNode.Text = targetAvailabilitySet.ToString();

            if (!_IsBinding)
                PropertyChanged();
        }

        private void upDownFaultDomains_ValueChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;
            targetAvailabilitySet.PlatformFaultDomainCount = Convert.ToInt32(upDownFaultDomains.Value);

            if (!_IsBinding)
                PropertyChanged();
        }

        private void upDownUpdateDomains_ValueChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.AvailabilitySet targetAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;
            targetAvailabilitySet.PlatformUpdateDomainCount = Convert.ToInt32(upDownUpdateDomains.Value);

            if (!_IsBinding)
                PropertyChanged();
        }
    }
}
