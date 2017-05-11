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
using MigAz.UserControls.Migrators;
using MigAz.Core.Interface;

namespace MigAz.UserControls
{
    public partial class DiskProperties : UserControl
    {
        private AsmToArm _AsmToArmForm;
        private TreeNode _DiskTreeNode;
        private Azure.MigrationTarget.Disk _TargetDisk;
        private ILogProvider _LogProvider;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public DiskProperties()
        {
            InitializeComponent();
        }

        public bool AllowManangedDisk
        {
            get { return rbManagedDisk.Enabled; }
            set
            {
                //if (!value)
                //{
                //    if (rbManagedDisk.Checked)
                //    {
                //        rbManagedDisk.Checked = false;
                //    }
                //}

                //rbManagedDisk.Enabled = value;
            }
        }

        public ILogProvider LogProvider
        {
            get { return _LogProvider; }
            set { _LogProvider = value; }
        }

        public IStorageAccount TargetStorageAccount
        {
            get
            {
                if (cmbTargetStorage.SelectedItem == null)
                    return null;
                else
                    return (IStorageAccount)cmbTargetStorage.SelectedItem;
            }
        }

        internal void Bind(AsmToArm asmToArmForm, Azure.MigrationTarget.Disk targetDisk)
        {
            _AsmToArmForm = asmToArmForm;
            _TargetDisk = targetDisk;

            BindCommon();
        }

        internal void Bind(AsmToArm asmToArmForm, TreeNode armDataDiskNode)
        {
            _AsmToArmForm = asmToArmForm;
            _DiskTreeNode = armDataDiskNode;
            _TargetDisk = (Azure.MigrationTarget.Disk)_DiskTreeNode.Tag;

            BindCommon();
        }

        private void BindCommon()
        {
            if (AllowManangedDisk)
                rbManagedDisk.Checked = true;
            else
            {
                if (_TargetDisk != null)
                {
                    if (_TargetDisk.TargetStorageAccount == null || (_TargetDisk.TargetStorageAccount != null && _TargetDisk.TargetStorageAccount.GetType() == typeof(Azure.MigrationTarget.StorageAccount)))
                        rbStorageAccountInMigration.Checked = true;
                    else if (_TargetDisk.TargetStorageAccount != null && _TargetDisk.TargetStorageAccount.GetType() == typeof(Azure.Arm.StorageAccount))
                        rbExistingARMStorageAccount.Checked = true;
                }
            }

            if (_TargetDisk != null)
            {
                lblAsmStorageAccount.Text = _TargetDisk.StorageAccountName;
                lblDiskName.Text = _TargetDisk.TargetName;
                lblHostCaching.Text = _TargetDisk.HostCaching;
                lblLUN.Text = _TargetDisk.Lun.ToString();
                txtTargetDiskName.Text = _TargetDisk.TargetName;
                txtBlobName.Text = _TargetDisk.StorageAccountBlob;
            }
        }

        private void rbManagedDIsk_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton senderButton = (RadioButton)sender;
            if (senderButton.Checked)
            {
                cmbTargetStorage.Items.Clear();
                cmbTargetStorage.Enabled = false;
            }
        }

        private void rbStorageAccountInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                cmbTargetStorage.Items.Clear();
                cmbTargetStorage.Enabled = true;

                TreeNode targetResourceGroupNode = _AsmToArmForm.SeekARMChildTreeNode(_AsmToArmForm.TargetResourceGroup.ToString(), _AsmToArmForm.TargetResourceGroup.ToString(), _AsmToArmForm.TargetResourceGroup, false);

                foreach (TreeNode treeNode in targetResourceGroupNode.Nodes)
                {
                    if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        Azure.MigrationTarget.StorageAccount storageAccountTarget = (Azure.MigrationTarget.StorageAccount)treeNode.Tag;
                        cmbTargetStorage.Items.Add(storageAccountTarget);
                    }
                }

                if (_TargetDisk != null)
                {
                    if (_TargetDisk.TargetStorageAccount != null)
                    {
                        for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                        {
                            Azure.MigrationTarget.StorageAccount cmbStorageAccount = (Azure.MigrationTarget.StorageAccount)cmbTargetStorage.Items[i];
                            if (cmbStorageAccount.ToString() == _TargetDisk.TargetStorageAccount.ToString())
                            {
                                cmbTargetStorage.SelectedIndex = i;
                                break;
                            }
                        }

                        // Using a for loop above, because this was always selecting Index 0, even when matched on a higher ( > 0) indexed item
                        //foreach (Azure.Asm.StorageAccount asmStorageAccount in cmbTargetStorage.Items)
                        //{
                        //    if (asmStorageAccount.Id == _AsmDataDisk.TargetStorageAccount.Id)
                        //    {
                        //        cmbTargetStorage.SelectedItem = asmStorageAccount;
                        //        break;
                        //    }
                        //}
                    }
                }
            }
        }

        private async void rbExistingARMStorageAccount_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                cmbTargetStorage.Items.Clear();
                cmbTargetStorage.Enabled = true;

                foreach (Azure.Arm.StorageAccount armStorageAccount in await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMStorageAccounts())
                {
                    cmbTargetStorage.Items.Add(armStorageAccount);
                }

                if (_TargetDisk != null)
                {
                    if ((_TargetDisk.TargetStorageAccount == null) || (_TargetDisk.TargetStorageAccount.GetType() == typeof(Azure.Asm.StorageAccount)))
                    {

                    }
                    else
                    {
                        for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                        {
                            Azure.Arm.StorageAccount cmbStorageAccount = (Azure.Arm.StorageAccount)cmbTargetStorage.Items[i];
                            if (cmbStorageAccount.ToString() == _TargetDisk.TargetStorageAccount.ToString())
                            {
                                cmbTargetStorage.SelectedIndex = i;
                                break;
                            }
                        }

                        // Using a for loop above, because this was always selecting Index 0, even when matched on a higher ( > 0) indexed item
                        //foreach (Azure.Arm.StorageAccount armStorageAccount in cmbTargetStorage.Items)
                        //{
                        //    if (armStorageAccount.Id == _AsmDataDisk.TargetStorageAccount.Id)
                        //        cmbTargetStorage.SelectedIndex = cmbTargetStorage.Items.IndexOf(armStorageAccount);
                        //}
                    }
                }
            }

            _AsmToArmForm.AzureContextTargetARM.StatusProvider.UpdateStatus("Ready");
        }

        private void cmbTargetStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;

            if (_TargetDisk != null)
                _TargetDisk.TargetStorageAccount = null;

            if (cmbSender.SelectedItem != null)
            {
                IStorageTarget targetStorageAccount = (IStorageTarget)cmbSender.SelectedItem;
                if (_TargetDisk != null)
                    _TargetDisk.TargetStorageAccount = targetStorageAccount;
            }

            PropertyChanged();
            this._AsmToArmForm.StatusProvider.UpdateStatus("Ready");
        }

        private void txtTargetDiskName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetDisk.TargetName = txtSender.Text.Trim();
            if (_DiskTreeNode != null)
                _DiskTreeNode.Text = _TargetDisk.ToString();

            PropertyChanged();
            this._AsmToArmForm.StatusProvider.UpdateStatus("Ready");
        }

        private void txtBlobName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetDisk.StorageAccountBlob = txtSender.Text.Trim();

            PropertyChanged();
            this._AsmToArmForm.StatusProvider.UpdateStatus("Ready");
        }
    }
}
