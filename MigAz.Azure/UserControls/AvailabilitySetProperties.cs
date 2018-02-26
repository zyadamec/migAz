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
    public partial class AvailabilitySetProperties : UserControl
    {
        TargetTreeView _TargetTreeView;
        AvailabilitySet _AvailabilitySet;
        bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public AvailabilitySetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(AvailabilitySet availabilitySet, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;

                _TargetTreeView = targetTreeView;
                _AvailabilitySet = availabilitySet;

                txtTargetName.Text = _AvailabilitySet.TargetName;
                upDownFaultDomains.Value = _AvailabilitySet.PlatformFaultDomainCount;
                upDownUpdateDomains.Value = _AvailabilitySet.PlatformUpdateDomainCount;

                foreach (Azure.MigrationTarget.VirtualMachine virtualMachine in _AvailabilitySet.TargetVirtualMachines)
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

            _AvailabilitySet.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged();
        }

        private void upDownFaultDomains_ValueChanged(object sender, EventArgs e)
        {
            _AvailabilitySet.PlatformFaultDomainCount = Convert.ToInt32(upDownFaultDomains.Value);

            if (!_IsBinding)
                PropertyChanged();
        }

        private void upDownUpdateDomains_ValueChanged(object sender, EventArgs e)
        {
            _AvailabilitySet.PlatformUpdateDomainCount = Convert.ToInt32(upDownUpdateDomains.Value);

            if (!_IsBinding)
                PropertyChanged();
        }
    }
}
