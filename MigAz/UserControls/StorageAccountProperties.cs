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
using MigAz.Azure;

namespace MigAz.UserControls
{
    public partial class StorageAccountProperties : UserControl
    {
        private TreeNode _ArmStorageAccountNode;
        private AzureContext _AzureContext;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public StorageAccountProperties()
        {
            InitializeComponent();
        }

        public void Bind(AzureContext azureContext, TreeNode sourceStorageAccountNode)
        {
            _AzureContext = azureContext;
            txtTargetName.MaxLength = 24 - azureContext.SettingsProvider.StorageAccountSuffix.Length;

            _ArmStorageAccountNode = sourceStorageAccountNode;
            TreeNode storageAccountNode = (TreeNode)_ArmStorageAccountNode.Tag;

            if (storageAccountNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount))
            {
                StorageAccount asmStorageAccount = (StorageAccount)storageAccountNode.Tag;

                lblAccountType.Text = asmStorageAccount.AccountType;
                lblSourceASMName.Text = asmStorageAccount.Name;

                if (asmStorageAccount.TargetName.Length > txtTargetName.MaxLength)
                    txtTargetName.Text = asmStorageAccount.TargetName.Substring(0, txtTargetName.MaxLength);
                else
                    txtTargetName.Text = asmStorageAccount.TargetName;
            }
            else if (storageAccountNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount))
            {
                StorageAccount armStorageAccount = (StorageAccount)storageAccountNode.Tag;

                lblAccountType.Text = armStorageAccount.AccountType;
                lblSourceASMName.Text = armStorageAccount.Name;

                if (armStorageAccount.TargetName.Length > txtTargetName.MaxLength)
                    txtTargetName.Text = armStorageAccount.TargetName.Substring(0, txtTargetName.MaxLength);
                else
                    txtTargetName.Text = armStorageAccount.TargetName;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            TreeNode asmStorageAccountNode = (TreeNode)_ArmStorageAccountNode.Tag;

            if (asmStorageAccountNode.Tag.GetType() == typeof(Azure.Asm.StorageAccount))
            {
                Azure.Asm.StorageAccount asmStorageAccount = (Azure.Asm.StorageAccount)asmStorageAccountNode.Tag;

                asmStorageAccount.TargetName = txtSender.Text;
                _ArmStorageAccountNode.Text = asmStorageAccount.GetFinalTargetName();
            }
            else if (asmStorageAccountNode.Tag.GetType() == typeof(Azure.Arm.StorageAccount))
            {
                Azure.Arm.StorageAccount armStorageAccount = (Azure.Arm.StorageAccount)asmStorageAccountNode.Tag;

                armStorageAccount.TargetName = txtSender.Text;
                _ArmStorageAccountNode.Text = armStorageAccount.GetFinalTargetName();
            }

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
