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

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public AvailabilitySetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TargetTreeView targetTreeView, TreeNode availabilitySetNode)
        {
            _TargetTreeView = targetTreeView;
            _armAvailabilitySetNode = availabilitySetNode;
            
            Azure.MigrationTarget.AvailabilitySet armAvailabilitySet = (Azure.MigrationTarget.AvailabilitySet)_armAvailabilitySetNode.Tag;
            txtTargetName.Text = armAvailabilitySet.TargetName;

            foreach (Azure.MigrationTarget.VirtualMachine virtualMachine in armAvailabilitySet.TargetVirtualMachines)
            {
                AddResourceSummary(new ResourceSummary(virtualMachine, _TargetTreeView));
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

            targetAvailabilitySet.TargetName = txtSender.Text;
            _armAvailabilitySetNode.Text = targetAvailabilitySet.ToString();

            PropertyChanged();
        }
    }
}
