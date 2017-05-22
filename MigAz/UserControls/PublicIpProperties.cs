using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.UserControls
{
    public partial class PublicIpProperties : UserControl
    {
        TreeNode _PublicIpNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public PublicIpProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TreeNode publicIpNode)
        {
            _PublicIpNode = publicIpNode;

            Azure.MigrationTarget.PublicIp targetPublicIp = (Azure.MigrationTarget.PublicIp)_PublicIpNode.Tag;
            txtTargetName.Text = targetPublicIp.Name;
            txtDomainNameLabel.Text = targetPublicIp.DomainNameLabel;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.PublicIp targetPublicIp = (Azure.MigrationTarget.PublicIp)_PublicIpNode.Tag;

            targetPublicIp.Name = txtSender.Text;
            _PublicIpNode.Text = targetPublicIp.ToString();

            PropertyChanged();
        }

        private void txtDomainNameLabel_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.PublicIp targetPublicIp = (Azure.MigrationTarget.PublicIp)_PublicIpNode.Tag;

            targetPublicIp.DomainNameLabel = txtSender.Text;

            PropertyChanged();
        }
    }
}
