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
using MigAz.Core.Interface;

namespace MigAz.UserControls
{
    public partial class NetworkInterfaceProperties : UserControl
    {

        private AsmToArm _AsmToArmForm;
        private TreeNode _NetworkInterfaceNode;
        private Azure.MigrationTarget.NetworkInterface _TargetNetworkInterface;
        private ILogProvider _LogProvider;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public NetworkInterfaceProperties()
        {
            InitializeComponent();
        }

        internal void Bind(AsmToArm asmToArmForm, Azure.MigrationTarget.NetworkInterface targetNetworkInterface)
        {
            _AsmToArmForm = asmToArmForm;
            _TargetNetworkInterface = targetNetworkInterface;

        }
        internal void Bind(AsmToArm asmToArmForm, TreeNode networkInterfaceNode)
        {
            _AsmToArmForm = asmToArmForm;
            _NetworkInterfaceNode = networkInterfaceNode;
            _TargetNetworkInterface = (Azure.MigrationTarget.NetworkInterface)_NetworkInterfaceNode.Tag;

        }


    }
}
