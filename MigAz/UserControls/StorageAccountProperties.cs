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
        private TreeNode _StorageAccountNode;
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

            _StorageAccountNode = sourceStorageAccountNode;
            Azure.MigrationTarget.StorageAccount storageAccount = (Azure.MigrationTarget.StorageAccount)_StorageAccountNode.Tag;
            lblAccountType.Text = storageAccount.SourceAccount.AccountType;
            lblSourceASMName.Text = storageAccount.SourceAccount.Name;

            if (storageAccount.TargetName != null)
            {
                if (storageAccount.TargetName.Length > txtTargetName.MaxLength)
                    txtTargetName.Text = storageAccount.TargetName.Substring(0, txtTargetName.MaxLength);
                else
                    txtTargetName.Text = storageAccount.TargetName;
            }
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            Azure.MigrationTarget.StorageAccount storageAccount = (Azure.MigrationTarget.StorageAccount)_StorageAccountNode.Tag;
            storageAccount.TargetName = txtSender.Text;
            _StorageAccountNode.Text = storageAccount.ToString();

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
