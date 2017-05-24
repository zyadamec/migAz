using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class NetworkSecurityGroupProperties : UserControl
    {
        private TreeNode _NetworkSecurityGroupNode;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkSecurityGroupProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TreeNode networkSecurityGroupNode)
        {
            _NetworkSecurityGroupNode = networkSecurityGroupNode;

            NetworkSecurityGroup targetNetworkSecurityGroup = (NetworkSecurityGroup)_NetworkSecurityGroupNode.Tag;
            if (targetNetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
            {
                Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup = (Azure.Asm.NetworkSecurityGroup)targetNetworkSecurityGroup.SourceNetworkSecurityGroup;
                lblSourceName.Text = asmNetworkSecurityGroup.Name;
            }
            else if (targetNetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
            {
                Azure.Arm.NetworkSecurityGroup armNetworkSecurityGroup = (Azure.Arm.NetworkSecurityGroup)targetNetworkSecurityGroup.SourceNetworkSecurityGroup;
                lblSourceName.Text = armNetworkSecurityGroup.Name;
            }

            txtTargetName.Text = targetNetworkSecurityGroup.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;
            NetworkSecurityGroup targetNetworkSecurityGroup = (NetworkSecurityGroup)_NetworkSecurityGroupNode.Tag;

            targetNetworkSecurityGroup.TargetName = txtSender.Text;
            _NetworkSecurityGroupNode.Text = targetNetworkSecurityGroup.ToString();

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
