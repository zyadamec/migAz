using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Azure.UserControls
{
    public partial class VirtualMachineImageProperties : UserControl
    {
        TargetTreeView _TargetTreeView;
        //MigrationTarget.VirtualMachineImage _VirtualMachineImage;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public VirtualMachineImageProperties()
        {
            InitializeComponent();
        }

        //public void Bind(TargetTreeView targetTreeView, MigrationTarget.VirtualMachineImage targetVirtualMachineImage)
        //{
        //    _TargetTreeView = targetTreeView;
        //    _VirtualMachineImage = targetVirtualMachineImage;

        //}

    }
}
