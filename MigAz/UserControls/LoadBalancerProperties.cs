using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.UserControls.Migrators;

namespace MigAz.UserControls
{
    public partial class LoadBalancerProperties : UserControl
    {
        private AsmToArm _ParentForm;
        private TreeNode _TargetLoadBalancerNode;
        private Azure.MigrationTarget.LoadBalancer _TargetLoadBalancer;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public LoadBalancerProperties()
        {
            InitializeComponent();
        }

        internal async Task Bind(AsmToArm parentForm, TreeNode treeNode)
        {
            _ParentForm = parentForm;
            _TargetLoadBalancerNode = treeNode;
            _TargetLoadBalancer = (Azure.MigrationTarget.LoadBalancer)treeNode.Tag;
            txtTargetName.Text = _TargetLoadBalancer.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetLoadBalancer.TargetName = txtSender.Text;
            _TargetLoadBalancerNode.Text = _TargetLoadBalancer.ToString();

            PropertyChanged();
        }
    }
}
