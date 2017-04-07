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

        public void Bind(AzureContext azureContext, TreeNode storageAccountNode)
        {
            _AzureContext = azureContext;
            txtTargetName.MaxLength = 24 - azureContext.SettingsProvider.StorageAccountSuffix.Length;

            _ArmStorageAccountNode = storageAccountNode;
            TreeNode asmStorageAccountNode = (TreeNode)_ArmStorageAccountNode.Tag;
            StorageAccount asmStorageAccount = (StorageAccount)asmStorageAccountNode.Tag;

            lblAccountType.Text = asmStorageAccount.AccountType;
            lblSourceASMName.Text = asmStorageAccount.Name;

            if (asmStorageAccount.TargetName.Length > txtTargetName.MaxLength)
                txtTargetName.Text = asmStorageAccount.TargetName.Substring(0, txtTargetName.MaxLength);
            else
                txtTargetName.Text = asmStorageAccount.TargetName;
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            TreeNode asmStorageAccountNode = (TreeNode)_ArmStorageAccountNode.Tag;
            StorageAccount asmStorageAccount = (StorageAccount)asmStorageAccountNode.Tag;

            asmStorageAccount.TargetName = txtSender.Text;
            _ArmStorageAccountNode.Text = asmStorageAccount.GetFinalTargetName();

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
