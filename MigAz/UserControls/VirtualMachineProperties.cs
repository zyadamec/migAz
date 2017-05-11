using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.UserControls.Migrators;
using MigAz.Core.Interface;

namespace MigAz.UserControls
{
    public partial class VirtualMachineProperties : UserControl
    {
        private AsmToArm _AsmToArmForm;
        private TreeNode _VirtualMachineNode;
        private ILogProvider _LogProvider;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public bool AllowManangedDisk
        {
            get { return this.diskProperties1.AllowManangedDisk; }
            set { this.diskProperties1.AllowManangedDisk = value; }
        }

        public VirtualMachineProperties()
        {
            InitializeComponent();
            this.networkInterfaceProperties1.PropertyChanged += Properties1_PropertyChanged;
            this.diskProperties1.PropertyChanged += Properties1_PropertyChanged;
        }

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
            set
            {
                _LogProvider = value;
                this.diskProperties1.LogProvider = value;
            }
        }

        public async Task Bind(TreeNode armVirtualMachineNode, AsmToArm asmToArmForm)
        {
            _VirtualMachineNode = armVirtualMachineNode;
            _AsmToArmForm = asmToArmForm;

            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;
            txtTargetName.Text = targetVirtualMachine.TargetName;

            if (targetVirtualMachine.Source.GetType() == typeof(Azure.Asm.VirtualMachine))
            {
                Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)targetVirtualMachine.Source;

                lblRoleSize.Text = asmVirtualMachine.RoleSize;
                lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
            }
            else if (targetVirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
            {
                Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)targetVirtualMachine.Source;

                lblRoleSize.Text = armVirtualMachine.VmSize;
                lblOS.Text = armVirtualMachine.OSVirtualHardDiskOS;
            }

            if (targetVirtualMachine.PrimaryNetworkInterface != null)
                await this.networkInterfaceProperties1.Bind(asmToArmForm, targetVirtualMachine.PrimaryNetworkInterface);
            else
                this.networkInterfaceProperties1.Visible = false;

            if (targetVirtualMachine.OSVirtualHardDisk != null)
                this.diskProperties1.Bind(asmToArmForm, targetVirtualMachine.OSVirtualHardDisk);
            else
                this.diskProperties1.Visible = false;
        }

        private async Task Properties1_PropertyChanged()
        {
            await PropertyChanged();
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            Azure.MigrationTarget.VirtualMachine targetVirtualMachine = (Azure.MigrationTarget.VirtualMachine)_VirtualMachineNode.Tag;

            targetVirtualMachine.TargetName = txtTargetName.Text;
            _VirtualMachineNode.Text = targetVirtualMachine.ToString();

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
