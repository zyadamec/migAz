using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class NetworkSecurityGroupProperties : UserControl
    {

        private TargetTreeView _TargetTreeView;
        private NetworkSecurityGroup _NetworkSecurityGroup;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkSecurityGroupProperties()
        {
            InitializeComponent();
        }

        internal void Bind(NetworkSecurityGroup networkSecurityGroup, TargetTreeView targetTreeView)
        {
            _NetworkSecurityGroup = networkSecurityGroup;
            _TargetTreeView = targetTreeView;

            if (_NetworkSecurityGroup.SourceNetworkSecurityGroup != null)
            {
                if (_NetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Asm.NetworkSecurityGroup))
                {
                    Azure.Asm.NetworkSecurityGroup asmNetworkSecurityGroup = (Azure.Asm.NetworkSecurityGroup)_NetworkSecurityGroup.SourceNetworkSecurityGroup;
                    lblSourceName.Text = asmNetworkSecurityGroup.Name;
                }
                else if (_NetworkSecurityGroup.SourceNetworkSecurityGroup.GetType() == typeof(Azure.Arm.NetworkSecurityGroup))
                {
                    Azure.Arm.NetworkSecurityGroup armNetworkSecurityGroup = (Azure.Arm.NetworkSecurityGroup)_NetworkSecurityGroup.SourceNetworkSecurityGroup;
                    lblSourceName.Text = armNetworkSecurityGroup.Name;
                }
            }

            txtTargetName.Text = _NetworkSecurityGroup.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _NetworkSecurityGroup.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

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
