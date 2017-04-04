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
        private ILogProvider _LogProvider;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public DiskProperties(ILogProvider logProvider)
        {
            InitializeComponent();

            _LogProvider = logProvider;
        }

        private DiskProperties()
        {

        }

        public bool AllowManangedDisk
        {
            get { return rbManagedDIsk.Enabled; }
            set
            {
                if (!value)
                {
                    if (rbManagedDIsk.Checked)
                    {
                        rbManagedDIsk.Checked = false;
                    }
                }

                rbManagedDIsk.Enabled = value;
                cmbTargetStorage.Enabled = !value;
            }
        }

        internal void Bind(AsmToArm asmToArmForm, Azure.Asm.Disk asmDisk)
        {
            _AsmToArmForm = asmToArmForm;
            _AsmDataDisk = asmDisk;

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
            lblAsmStorageAccount.Text = _AsmDataDisk.StorageAccountName;
            lblDiskName.Text = _AsmDataDisk.DiskName;
            lblHostCaching.Text = _AsmDataDisk.HostCaching;
            lblLUN.Text = _AsmDataDisk.Lun.ToString();
            txtTargetDiskName.Text = _AsmDataDisk.TargetName;

            if (AllowManangedDisk)
                rbManagedDIsk.Checked = true;
            else
            {
                rbManagedDIsk.Enabled = false;

                if (_AsmDataDisk.TargetStorageAccount == null || _AsmDataDisk.TargetStorageAccount.GetType() == typeof(Azure.Asm.StorageAccount))
                    rbStorageAccountInMigration.Checked = true;
                else
                    rbExistingARMStorageAccount.Checked = true;
            }
        }

        private void rbManagedDIsk_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbStorageAccountInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                cmbTargetStorage.Items.Clear();

                TreeNode targetResourceGroupNode = _AsmToArmForm.SeekARMChildTreeNode(_AsmToArmForm.TargetResourceGroup.Name, _AsmToArmForm.TargetResourceGroup.GetFinalTargetName(), _AsmToArmForm.TargetResourceGroup, false);
                TreeNode storageAccountsNode = _AsmToArmForm.SeekARMChildTreeNode(targetResourceGroupNode.Nodes, "Storage Accounts", "Storage Accounts", "Storage Accounts", false);

                if (storageAccountsNode != null)
                {
                    foreach (TreeNode armStorageAccountNode in storageAccountsNode.Nodes)
                    {
                        TreeNode asmStorageAccountNode = (TreeNode)armStorageAccountNode.Tag;
                        cmbTargetStorage.Items.Add(asmStorageAccountNode.Tag);
                    }
                }

                if ((_AsmDataDisk.TargetStorageAccount == null) || (_AsmDataDisk.TargetStorageAccount.GetType() == typeof(Azure.Arm.StorageAccount)))
                {
                    foreach (Azure.Asm.StorageAccount asmStorageAccount in cmbTargetStorage.Items)
                    {
                        if (asmStorageAccount.Name == _AsmDataDisk.StorageAccountName)
                        {
                            cmbTargetStorage.SelectedItem = asmStorageAccount;
                            break;
                        }
                    }

                    if (_AsmDataDisk.TargetStorageAccount == null)
                    {
                        _LogProvider.WriteLog("rbStorageAccountInMigration_CheckedChanged", "Unable to location originating ASM Storage Account '" + _AsmDataDisk.StorageAccountName + "' as an object included for ASM to ARM migration.  Please select a target storage account for the Azure Disk.");
                    }
                }
                else
                {
                    foreach (Azure.Asm.StorageAccount asmStorageAccount in cmbTargetStorage.Items)
                    {
                        if (asmStorageAccount.Id == _AsmDataDisk.TargetStorageAccount.Id)
                        {
                            cmbTargetStorage.SelectedItem = asmStorageAccount;
                            break;
                        }
                    }

                    if (cmbTargetStorage.SelectedItem == null)
                        _LogProvider.WriteLog("rbStorageAccountInMigration_CheckedChanged", "Unable to location previously selected ASM Storage Account '" + _AsmDataDisk.TargetStorageAccount.Id + "' as an object included for ASM to ARM migration.  Please select a target storage account for the Azure Disk.");
                }
            }

            PropertyChanged();
        }

        private async void rbExistingARMStorageAccount_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                cmbTargetStorage.Items.Clear();
                _AsmDataDisk.TargetStorageAccount = null;

                foreach (Azure.Arm.StorageAccount armStorageAccount in await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMStorageAccounts())
                {
                    cmbTargetStorage.Items.Add(armStorageAccount);
                }

                if ((_AsmDataDisk.TargetStorageAccount == null) || (_AsmDataDisk.TargetStorageAccount.GetType() == typeof(Azure.Asm.StorageAccount)))
                {
                    if (cmbTargetStorage.Items.Count > 0)
                        cmbTargetStorage.SelectedIndex = 0;
                }
                else
                {
                    foreach (Azure.Arm.StorageAccount armStorageAccount in cmbTargetStorage.Items)
                    {
                        if (armStorageAccount.Id == _AsmDataDisk.TargetStorageAccount.Id)
                            cmbTargetStorage.SelectedItem = armStorageAccount;
                    }
                }
            }

            PropertyChanged();
        }

        private void cmbTargetStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;

            if (cmbSender.SelectedItem == null)
            {
                _AsmDataDisk.TargetStorageAccount = null;
            }
            else
            {
                if (cmbSender.SelectedItem.GetType() == typeof(Azure.Asm.StorageAccount))
                {
                    Azure.Asm.StorageAccount asmStorageAccount = (Azure.Asm.StorageAccount)cmbSender.SelectedItem;
                    _AsmDataDisk.TargetStorageAccount = asmStorageAccount;
                }
                else if (cmbSender.SelectedItem.GetType() == typeof(Azure.Arm.StorageAccount))
                {
                    Azure.Arm.StorageAccount armStorageAccount = (Azure.Arm.StorageAccount)cmbSender.SelectedItem;
                    _AsmDataDisk.TargetStorageAccount = armStorageAccount;
                }
                else
                    _AsmDataDisk.TargetStorageAccount = null;
            }

            UpdateParentNode();
            PropertyChanged();
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

            _AsmDataDisk.TargetName = txtSender.Text.Trim();

            if (_ARMDataDiskNode != null)
            {
                _ARMDataDiskNode.Text = _AsmDataDisk.TargetName;
                UpdateParentNode();
            }

            PropertyChanged();
        }


    }
}
