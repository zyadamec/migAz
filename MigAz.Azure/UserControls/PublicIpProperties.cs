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
        private TargetTreeView _TargetTreeView;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public PublicIpProperties()
        {
            InitializeComponent();
        }

        internal void Bind(PublicIp publicIp, TargetTreeView targetTreeView)
        {
            _PublicIp = publicIp;
            _TargetTreeView = targetTreeView;

            txtTargetName.Text = _PublicIp.TargetName;
            txtDomainNameLabel.Text = _PublicIp.DomainNameLabel;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _PublicIp.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            PropertyChanged();
        }

        private void txtDomainNameLabel_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _PublicIp.DomainNameLabel = txtSender.Text;

            PropertyChanged();
        }
    }
}
