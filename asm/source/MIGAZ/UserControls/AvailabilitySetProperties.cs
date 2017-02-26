using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Asm;
using MigAz.Azure.Arm;

namespace MIGAZ.UserControls
{
    public partial class AvailabilitySetProperties : UserControl
    {
        TreeNode _armAvailabilitySetNode;

        public AvailabilitySetProperties()
        {
            InitializeComponent();
        }

        internal void Bind(TreeNode availabilitySetNode)
        {
            _armAvailabilitySetNode = availabilitySetNode;

            ArmAvailabilitySet armAvailabilitySet = (ArmAvailabilitySet)_armAvailabilitySetNode.Tag;

            //lblAccountType.Text = asmCloudService.AccountType;
            txtTargetName.Text = armAvailabilitySet.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            ArmAvailabilitySet armAvailabilitySet = (ArmAvailabilitySet)_armAvailabilitySetNode.Tag;

            armAvailabilitySet.TargetName = txtSender.Text;
            _armAvailabilitySetNode.Text = armAvailabilitySet.GetFinalTargetName();
        }
    }
}
