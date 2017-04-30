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
        private TreeNode _ARMDataDiskNode;
        private Azure.Asm.Disk _AsmDataDisk;
        private Azure.Arm.Disk _ArmDataDisk;
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

        internal void Bind(AsmToArm asmToArmForm, Azure.Asm.Disk asmDisk)
        {
            _AsmToArmForm = asmToArmForm;
            _AsmDataDisk = asmDisk;

            BindCommon();
        }

        internal void Bind(AsmToArm asmToArmForm, Azure.Arm.Disk armDisk)
        {
            _AsmToArmForm = asmToArmForm;
            _ArmDataDisk = armDisk;

            BindCommon();
        }

        internal void Bind(AsmToArm asmToArmForm, TreeNode armDataDiskNode)
        {
            _AsmToArmForm = asmToArmForm;
            _ARMDataDiskNode = armDataDiskNode;
            _AsmDataDisk = (Azure.Asm.Disk)_ARMDataDiskNode.Tag;

            BindCommon();
        }

        private void BindCommon()
        {
            if (AllowManangedDisk)
                rbManagedDisk.Checked = true;
            else
            {
                if (_AsmDataDisk != null)
                {
                    if (_AsmDataDisk.TargetStorageAccount != null && _AsmDataDisk.TargetStorageAccount.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                        rbStorageAccountInMigration.Checked = true;
                    else
                        rbExistingARMStorageAccount.Checked = true;
                }
                else if (_ArmDataDisk != null)
                {
                    if (_ArmDataDisk.TargetStorageAccount != null && _ArmDataDisk.TargetStorageAccount.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                        rbStorageAccountInMigration.Checked = true;
                    else
                        rbExistingARMStorageAccount.Checked = true;
                }
            }

            if (_AsmDataDisk != null)
            {
                lblAsmStorageAccount.Text = _AsmDataDisk.StorageAccountName;
                lblDiskName.Text = _AsmDataDisk.DiskName;
                lblHostCaching.Text = _AsmDataDisk.HostCaching;
                lblLUN.Text = _AsmDataDisk.Lun.ToString();
                txtTargetDiskName.Text = _AsmDataDisk.TargetName;
            }
            else if (_ArmDataDisk != null)
            {
                lblAsmStorageAccount.Text = _ArmDataDisk.StorageAccountName;
                lblDiskName.Text = _ArmDataDisk.Name;
                lblHostCaching.Text = _ArmDataDisk.Caching;
                lblLUN.Text = _ArmDataDisk.Lun.ToString();
                txtTargetDiskName.Text = _ArmDataDisk.TargetName;
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

                TreeNode targetResourceGroupNode = _AsmToArmForm.SeekARMChildTreeNode(_AsmToArmForm.TargetResourceGroup.TargetName, _AsmToArmForm.TargetResourceGroup.ToString(), _AsmToArmForm.TargetResourceGroup, false);
                TreeNode storageAccountsNode = _AsmToArmForm.SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Storage Accounts", "Storage Accounts", "Storage Accounts", false);

                if (storageAccountsNode != null)
                {
                    foreach (TreeNode armStorageAccountNode in storageAccountsNode.Nodes)
                    {
                        Azure.MigrationTarget.StorageAccount storageAccountTarget = (Azure.MigrationTarget.StorageAccount)armStorageAccountNode.Tag;
                        cmbTargetStorage.Items.Add(storageAccountTarget);
                    }
                }

                if (_AsmDataDisk != null)
                {
                    if (_AsmDataDisk.TargetStorageAccount != null)
                    {
                        for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                        {
                            Azure.MigrationTarget.StorageAccount cmbStorageAccount = (Azure.MigrationTarget.StorageAccount)cmbTargetStorage.Items[i];
                            if (cmbStorageAccount.ToString() == _AsmDataDisk.TargetStorageAccount.ToString())
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

                if (_AsmDataDisk != null)
                {
                    if ((_AsmDataDisk.TargetStorageAccount == null) || (_AsmDataDisk.TargetStorageAccount.GetType() == typeof(Azure.Asm.StorageAccount)))
                    {

                    }
                    else
                    {
                        for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                        {
                            Azure.Arm.StorageAccount cmbStorageAccount = (Azure.Arm.StorageAccount)cmbTargetStorage.Items[i];
                            if (cmbStorageAccount.ToString() == _AsmDataDisk.TargetStorageAccount.ToString())
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

            if (_AsmDataDisk != null)
                _AsmDataDisk.TargetStorageAccount = null;
            if (_ArmDataDisk != null)
                _ArmDataDisk.TargetStorageAccount = null;

            if (cmbSender.SelectedItem != null)
            {
                IStorageTarget targetStorageAccount = (IStorageTarget)cmbSender.SelectedItem;
                if (_AsmDataDisk != null)
                    _AsmDataDisk.TargetStorageAccount = targetStorageAccount;
                if (_ArmDataDisk != null)
                    _ArmDataDisk.TargetStorageAccount = targetStorageAccount;
            }

            UpdateParentNode();
            PropertyChanged();
            this._AsmToArmForm.StatusProvider.UpdateStatus("Ready");
        }

        private void UpdateParentNode()
        {
            if (_ARMDataDiskNode != null)
            {
                TreeNode parentNode = (TreeNode)_ARMDataDiskNode.Parent.Tag;
                Azure.Asm.VirtualMachine parentVirtualMachine = (Azure.Asm.VirtualMachine)parentNode.Tag;
                foreach (Azure.Asm.Disk parentDisk in parentVirtualMachine.DataDisks)
                {
                    if (parentDisk.DiskName == _AsmDataDisk.DiskName)
                    {
                        parentDisk.TargetStorageAccount = _AsmDataDisk.TargetStorageAccount;
                        parentDisk.TargetName = _AsmDataDisk.TargetName;
                    }
                }
            }
        }

        private void txtTargetDiskName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            if (_AsmDataDisk != null)
            {
                _AsmDataDisk.TargetName = txtSender.Text.Trim();

                if (_ARMDataDiskNode != null)
                {
                    _ARMDataDiskNode.Text = _AsmDataDisk.TargetName;
                    UpdateParentNode();
                }
            }
            else if (_ArmDataDisk != null)
            {
                _ArmDataDisk.TargetName = txtSender.Text.Trim();

                if (_ARMDataDiskNode != null)
                {
                    _ARMDataDiskNode.Text = _ArmDataDisk.TargetName;
                    UpdateParentNode();
                }
            }

            PropertyChanged();
        }


    }
}
