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
    public partial class PublicIpProperties : UserControl
    {
        PublicIp _PublicIp;
        TargetTreeView _TargetTreeView;
        bool _IsBinding = false;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public PublicIpProperties()
        {
            InitializeComponent();
        }

        internal void Bind(PublicIp publicIp, TargetTreeView targetTreeView)
        {
            try
            {
                _IsBinding = true;
                _PublicIp = publicIp;
                _TargetTreeView = targetTreeView;

                txtTargetName.Text = _PublicIp.TargetName;
                txtDomainNameLabel.Text = _PublicIp.DomainNameLabel;

                int allocationMethodIndex = cmbPublicIpAllocation.FindString(_PublicIp.IPAllocationMethod.ToString());
                if (allocationMethodIndex >= 0)
                    cmbPublicIpAllocation.SelectedIndex = allocationMethodIndex;
                else
                    cmbPublicIpAllocation.SelectedIndex = 0;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _PublicIp.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private void txtDomainNameLabel_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _PublicIp.DomainNameLabel = txtSender.Text;

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }

        private void cmbPublicIpAllocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPublicIpAllocation.SelectedItem.ToString() == "Static")
                _PublicIp.IPAllocationMethod = IPAllocationMethodEnum.Static;
            else
                _PublicIp.IPAllocationMethod = IPAllocationMethodEnum.Dynamic;

            if (!_IsBinding)
                PropertyChanged?.Invoke();
        }
    }
}
