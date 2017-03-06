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
using MigAz.Forms.ASM;

namespace MigAz.UserControls
{
    public partial class DiskProperties : UserControl
    {
        private AsmToArm _AsmToArmForm;
        private TreeNode _ARMDataDiskNode;
        private AsmDisk _AsmDataDisk;

        public DiskProperties()
        {
            InitializeComponent();
        }

        internal void Bind(AsmToArm asmToArmForm, AsmDisk asmDisk)
        {
            _AsmToArmForm = asmToArmForm;
            _AsmDataDisk = asmDisk;

            BindCommon();
        }

        internal void Bind(AsmToArm asmToArmForm, TreeNode armDataDiskNode)
        {
            _AsmToArmForm = asmToArmForm;
            _ARMDataDiskNode = armDataDiskNode;
            _AsmDataDisk = (AsmDisk)_ARMDataDiskNode.Tag;

            BindCommon();
        }

        private void BindCommon()
        {
            lblAsmStorageAccount.Text = _AsmDataDisk.StorageAccountName;
            lblDiskName.Text = _AsmDataDisk.DiskName;
            lblHostCaching.Text = _AsmDataDisk.HostCaching;
            lblLUN.Text = _AsmDataDisk.Lun.ToString();
            txtTargetDiskName.Text = _AsmDataDisk.TargetName;

            if (_AsmDataDisk.TargetStorageAccount == null || _AsmDataDisk.TargetStorageAccount.GetType() == typeof(AsmStorageAccount))
                rbStorageAccountInMigration.Checked = true;
            else
                rbExistingARMStorageAccount.Checked = true;
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

                if ((_AsmDataDisk.TargetStorageAccount == null) || (_AsmDataDisk.TargetStorageAccount.GetType() == typeof(ArmStorageAccount)))
                {
                    foreach (AsmStorageAccount asmStorageAccount in cmbTargetStorage.Items)
                    {
                        if (asmStorageAccount.Name == _AsmDataDisk.StorageAccountName)
                        {
                            cmbTargetStorage.SelectedItem = asmStorageAccount;
                            break;
                        }
                    }

                    // In the event we couldn't find the target default storage account, alert user and select default
                    if (_AsmDataDisk.TargetStorageAccount == null)
                    {
                        MessageBox.Show("Unable to location originating ASM Storage Account '" + _AsmDataDisk.StorageAccountName + "' as an object included for ASM to ARM migration.  Please select a target storage account for the Azure Disk.");
                    }
                }
                else
                {
                    foreach (AsmStorageAccount asmStorageAccount in cmbTargetStorage.Items)
                    {
                        if (asmStorageAccount.Id == _AsmDataDisk.TargetStorageAccount.Id)
                        {
                            cmbTargetStorage.SelectedItem = asmStorageAccount;
                            break;
                        }
                    }

                    if (cmbTargetStorage.SelectedItem == null)
                        MessageBox.Show("Unable to location previously selected ASM Storage Account '" + _AsmDataDisk.TargetStorageAccount.Id + "' as an object included for ASM to ARM migration.  Please select a target storage account for the Azure Disk.");
                }
            }
        }

        private async void rbExistingARMStorageAccount_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                cmbTargetStorage.Items.Clear();
                _AsmDataDisk.TargetStorageAccount = null;

                foreach (ArmStorageAccount armStorageAccount in await _AsmToArmForm.AzureContextTargetARM.AzureRetriever.GetAzureARMStorageAccounts())
                {
                    cmbTargetStorage.Items.Add(armStorageAccount);
                }

                if ((_AsmDataDisk.TargetStorageAccount == null) || (_AsmDataDisk.TargetStorageAccount.GetType() == typeof(AsmStorageAccount)))
                {
                    if (cmbTargetStorage.Items.Count > 0)
                        cmbTargetStorage.SelectedIndex = 0;
                }
                else
                {
                    foreach (ArmStorageAccount armStorageAccount in cmbTargetStorage.Items)
                    {
                        if (armStorageAccount.Id == _AsmDataDisk.TargetStorageAccount.Id)
                            cmbTargetStorage.SelectedItem = armStorageAccount;
                    }
                }
            }
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
                if (cmbSender.SelectedItem.GetType() == typeof(AsmStorageAccount))
                {
                    AsmStorageAccount asmStorageAccount = (AsmStorageAccount)cmbSender.SelectedItem;
                    _AsmDataDisk.TargetStorageAccount = asmStorageAccount;
                }
                else if (cmbSender.SelectedItem.GetType() == typeof(ArmStorageAccount))
                {
                    ArmStorageAccount armStorageAccount = (ArmStorageAccount)cmbSender.SelectedItem;
                    _AsmDataDisk.TargetStorageAccount = armStorageAccount;
                }
                else
                    _AsmDataDisk.TargetStorageAccount = null;
            }

            UpdateParentNode();
        }

        private void UpdateParentNode()
        {
            if (_ARMDataDiskNode != null)
            {
                TreeNode parentNode = (TreeNode)_ARMDataDiskNode.Parent.Tag;
                AsmVirtualMachine parentVirtualMachine = (AsmVirtualMachine)parentNode.Tag;
                foreach (AsmDisk parentDisk in parentVirtualMachine.DataDisks)
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
        }

    }
}
