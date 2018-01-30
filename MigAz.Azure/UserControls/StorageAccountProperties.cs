using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure;
using MigAz.Core.Interface;
using MigAz.Core;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class StorageAccountProperties : UserControl
    {
        private StorageAccount _StorageAccount;
        private AzureContext _AzureContext;
        private TargetTreeView _TargetTreeView;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public StorageAccountProperties()
        {
            InitializeComponent();
        }

        public void Bind(AzureContext azureContext, StorageAccount storageAccount, TargetTreeView targetTreeView)
        {
            _AzureContext = azureContext;
            _TargetTreeView = targetTreeView;
            txtTargetName.MaxLength = StorageAccount.MaximumTargetNameLength(targetTreeView.TargetSettings);

            _StorageAccount = storageAccount;
            lblAccountType.Text = storageAccount.SourceAccount.AccountType;
            lblSourceASMName.Text = storageAccount.SourceAccount.Name;

            if (storageAccount.TargetName != null)
            {
                if (storageAccount.TargetName.Length > txtTargetName.MaxLength)
                    txtTargetName.Text = storageAccount.TargetName.Substring(0, txtTargetName.MaxLength);
                else
                    txtTargetName.Text = storageAccount.TargetName;
            }

            cmbAccountType.SelectedIndex = cmbAccountType.FindString(storageAccount.StorageAccountType.ToString());
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;
            _StorageAccount.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cmbAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccountType.SelectedItem.ToString() == "Premium_LRS")
                _StorageAccount.StorageAccountType = StorageAccountType.Premium_LRS;
            else
                _StorageAccount.StorageAccountType = StorageAccountType.Standard_LRS;

            PropertyChanged();
        }
    }
}
