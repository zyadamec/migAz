using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core.Interface;
using MigAz.Azure.MigrationTarget;

namespace MigAz.Azure.UserControls
{
    public partial class DiskProperties : UserControl
    {
        TargetTreeView _TargetTreeView;
        private AzureContext _AzureContext;
        private Azure.MigrationTarget.Disk _TargetDisk;
        private TreeNode _TargetTreeNode;
        private bool _ShowSizeInGb = true;
        private bool _IsBinding = false;
        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public DiskProperties()
        {
            InitializeComponent();
        }

        public bool ShowSizeInGb
        {
            get
            {
                return _ShowSizeInGb;
            }
            set
            {
                _ShowSizeInGb = value;

                lblSourceSizeGb.Visible = _ShowSizeInGb;
                txtTargetSize.Visible = _ShowSizeInGb;
            }
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

        internal void Bind(AzureContext azureContext, TargetTreeView targetTreeView, TreeNode targetDiskTreeNode, Azure.MigrationTarget.Disk targetDisk)
        {
            _AzureContext = azureContext;
            _TargetTreeView = targetTreeView;
            _TargetDisk = targetDisk;
            _TargetTreeNode = targetDiskTreeNode;

            BindCommon();
        }

        private async Task BindCommon()
        {
            if (_TargetDisk == null)
                throw new ArgumentException("MigrationTarget Disk object cannot be null.");

            _IsBinding = true;

            if (_TargetDisk.TargetStorage != null && _TargetDisk.TargetStorage.GetType() == typeof(Azure.MigrationTarget.ManagedDiskStorage))
                rbManagedDisk.Checked = true;
            if (_TargetDisk.TargetStorage != null && _TargetDisk.TargetStorage.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                rbStorageAccountInMigration.Checked = true;
            else if (_TargetDisk.TargetStorage != null && _TargetDisk.TargetStorage.GetType() == typeof(Azure.Arm.StorageAccount))
                rbExistingARMStorageAccount.Checked = true;

            lblAsmStorageAccount.Text = String.Empty;
            lblDiskName.Text = _TargetDisk.TargetName;
            lblHostCaching.Text = _TargetDisk.HostCaching;
            lblLUN.Text = _TargetDisk.Lun.ToString();
            txtTargetDiskName.Text = _TargetDisk.TargetName;
            txtBlobName.Text = _TargetDisk.TargetStorageAccountBlob;

            txtTargetSize.Text = _TargetDisk.DiskSizeInGB.ToString();

            if (_TargetDisk.SourceDisk != null)
            {
                lblSourceSizeGb.Text = _TargetDisk.SourceDisk.DiskSizeGb.ToString();

                if (_TargetDisk.SourceDisk.GetType() == typeof(Azure.Asm.Disk))
                {
                    Azure.Asm.Disk asmDisk = (Azure.Asm.Disk)_TargetDisk.SourceDisk;
                    if (asmDisk.SourceStorageAccount != null)
                        lblAsmStorageAccount.Text = asmDisk.SourceStorageAccount.Name;
                }
                else if (_TargetDisk.SourceDisk.GetType() == typeof(Azure.Arm.ClassicDisk))
                {
                    Azure.Arm.ClassicDisk armDisk = (Azure.Arm.ClassicDisk)_TargetDisk.SourceDisk;
                    if (armDisk.SourceStorageAccount != null)
                        lblAsmStorageAccount.Text = armDisk.SourceStorageAccount.Name;
                }
            }

            if (_TargetTreeView.TargetResourceGroup != null && _TargetTreeView.TargetResourceGroup.TargetLocation != null)
            {
                rbExistingARMStorageAccount.Text = "Existing Storage in " + _TargetTreeView.TargetResourceGroup.TargetLocation.DisplayName;
                List<Azure.Arm.StorageAccount> a = await _AzureContext.AzureSubscription.GetAzureARMStorageAccounts(_AzureContext, _TargetTreeView.TargetResourceGroup.TargetLocation);
                rbExistingARMStorageAccount.Enabled = a.Count() > 0;
            }
            else
            {
                // Cannot use existing ARM Storage without Target Location
                rbExistingARMStorageAccount.Enabled = false;
                rbExistingARMStorageAccount.Text = "<Set Resource Group Location>";
            }

            if (!_TargetTreeView.HasStorageAccount)
            {
                rbStorageAccountInMigration.Enabled = false;
            }

            virtualMachineSummary.Bind(_TargetDisk.ParentVirtualMachine, _TargetTreeView);

            _IsBinding = false;
        }

        private void rbManagedDIsk_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton senderButton = (RadioButton)sender;
            if (senderButton.Checked)
            {
                cmbTargetStorage.Items.Clear();
                cmbTargetStorage.Enabled = true;
                cmbTargetStorage.Items.Add("Standard_LRS");
                cmbTargetStorage.Items.Add("Premium_LRS");

                txtBlobName.Enabled = false;
                txtBlobName.Text = String.Empty;

                if (_IsBinding)
                {
                    int comboBoxIndex = cmbTargetStorage.Items.IndexOf(_TargetDisk.TargetStorage.StorageAccountType.ToString());
                    if (comboBoxIndex >= 0)
                        cmbTargetStorage.SelectedIndex = comboBoxIndex;
                }
                else
                {
                    _TargetDisk.TargetStorage = new ManagedDiskStorage(_TargetDisk.SourceDisk);
                    _TargetTreeView.TransitionToManagedDisk(_TargetTreeNode);

                    int comboBoxIndex = cmbTargetStorage.Items.IndexOf(_TargetDisk.TargetStorage.StorageAccountType.ToString());
                    if (comboBoxIndex >= 0)
                        cmbTargetStorage.SelectedIndex = comboBoxIndex;
                }

                if (!_IsBinding)
                {
                    PropertyChanged?.Invoke();
                }
            }
        }

        private void rbStorageAccountInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                if (_TargetDisk.TargetStorage != null && _TargetDisk.TargetStorage.GetType() != typeof(Azure.MigrationTarget.StorageAccount))
                    _TargetDisk.TargetStorage = null;

                cmbTargetStorage.Items.Clear();
                cmbTargetStorage.Enabled = true;
                txtBlobName.Enabled = true;
                txtBlobName.Text = _TargetDisk.TargetStorageAccountBlob;

                TreeNode targetResourceGroupNode = _TargetTreeView.ResourceGroupNode;

                foreach (TreeNode treeNode in targetResourceGroupNode.Nodes)
                {
                    if (treeNode.Tag != null && treeNode.Tag.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        Azure.MigrationTarget.StorageAccount storageAccountTarget = (Azure.MigrationTarget.StorageAccount)treeNode.Tag;
                        cmbTargetStorage.Items.Add(storageAccountTarget);
                    }
                }

                if (_IsBinding)
                {
                    if (_TargetDisk.TargetStorage != null)
                    {
                        for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                        {
                            Azure.MigrationTarget.StorageAccount cmbStorageAccount = (Azure.MigrationTarget.StorageAccount)cmbTargetStorage.Items[i];
                            if (cmbStorageAccount.ToString() == _TargetDisk.TargetStorage.ToString())
                            {
                                cmbTargetStorage.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                else if (_TargetDisk != null)
                {
                    if (_TargetDisk.TargetStorage != null)
                    {
                        for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                        {
                            Azure.MigrationTarget.StorageAccount cmbStorageAccount = (Azure.MigrationTarget.StorageAccount)cmbTargetStorage.Items[i];
                            if (cmbStorageAccount.ToString() == _TargetDisk.TargetStorage.ToString())
                            {
                                cmbTargetStorage.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Default
                        if (_TargetDisk.SourceStorageAccount != null)
                        {
                            foreach (Azure.MigrationTarget.StorageAccount targetStorageAccount in cmbTargetStorage.Items)
                            {
                                if (targetStorageAccount.SourceName == _TargetDisk.SourceStorageAccount.Name)
                                {
                                    cmbTargetStorage.SelectedIndex = cmbTargetStorage.Items.IndexOf(targetStorageAccount);
                                    break;
                                }
                            }
                        }

                        if (cmbTargetStorage.SelectedIndex == -1 && cmbTargetStorage.Items.Count > 0)
                            cmbTargetStorage.SelectedIndex = 0;
                    }

                    _TargetTreeView.TransitionToClassicDisk(_TargetTreeNode);

                    if (!_IsBinding)
                    {
                        PropertyChanged?.Invoke();
                    }
                }
            }
        }

        private async void rbExistingARMStorageAccount_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbSender = (RadioButton)sender;

            if (rbSender.Checked)
            {
                if (_TargetDisk.TargetStorage != null && _TargetDisk.TargetStorage.GetType() != typeof(Azure.Arm.StorageAccount))
                    _TargetDisk.TargetStorage = null;

                cmbTargetStorage.Items.Clear();
                cmbTargetStorage.Enabled = true;
                txtBlobName.Enabled = true;
                txtBlobName.Text = _TargetDisk.TargetStorageAccountBlob;

                foreach (Azure.Arm.StorageAccount armStorageAccount in await _AzureContext.AzureSubscription.GetAzureARMStorageAccounts(_AzureContext))
                {
                    cmbTargetStorage.Items.Add(armStorageAccount);
                }

                if (_IsBinding)
                {
                    if (_TargetDisk.TargetStorage != null)
                    {
                        if (_TargetDisk.TargetStorage.GetType() == typeof(Azure.Arm.StorageAccount))
                        {
                            for (int i = 0; i < cmbTargetStorage.Items.Count; i++)
                            {
                                if (cmbTargetStorage.Items[i].GetType() == typeof(Azure.Arm.StorageAccount))
                                {
                                    Azure.Arm.StorageAccount cmbStorageAccount = (Azure.Arm.StorageAccount)cmbTargetStorage.Items[i];
                                    if (cmbStorageAccount.ToString() == _TargetDisk.TargetStorage.ToString())
                                    {
                                        cmbTargetStorage.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                _TargetTreeView.TransitionToClassicDisk(_TargetTreeNode);

                if (!_IsBinding)
                {
                    PropertyChanged?.Invoke();
                }
            }

            _AzureContext.StatusProvider.UpdateStatus("Ready");
        }

        private void cmbTargetStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbSender = (ComboBox)sender;

            if (_TargetDisk != null)
            {
                if (_TargetDisk.TargetStorage == null)
                {
                    if (cmbSender.SelectedItem.GetType() == typeof(Azure.MigrationTarget.StorageAccount))
                    {
                        _TargetDisk.TargetStorage = (Azure.MigrationTarget.StorageAccount)cmbSender.SelectedItem;
                    }
                    else if (cmbSender.SelectedItem.GetType() == typeof(Azure.Arm.StorageAccount))
                    {
                        _TargetDisk.TargetStorage = (Azure.Arm.StorageAccount)cmbSender.SelectedItem;
                    }
                }


                if (_TargetDisk.TargetStorage.GetType() == typeof(Azure.MigrationTarget.ManagedDiskStorage))
                {
                    ManagedDiskStorage managedDiskStorage = (ManagedDiskStorage)_TargetDisk.TargetStorage;
                    if (cmbSender.SelectedItem.ToString() == "Premium_LRS")
                        managedDiskStorage.StorageAccountType = StorageAccountType.Premium_LRS;
                    else
                        managedDiskStorage.StorageAccountType = StorageAccountType.Standard_LRS;
                }
                else
                {
                    if (cmbSender.SelectedItem != null)
                    {
                        IStorageTarget targetStorageAccount = (IStorageTarget)cmbSender.SelectedItem;
                        _TargetDisk.TargetStorage = targetStorageAccount;
                    }
                    else
                        _TargetDisk.TargetStorage = null;
                }

                if (!_IsBinding)
                {
                    PropertyChanged?.Invoke();
                }
            }
        }

        private void txtTargetDiskName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            _TargetDisk.SetTargetName(txtSender.Text, _TargetTreeView.TargetSettings);

            if (!_IsBinding)
            {
                PropertyChanged?.Invoke();
            }
        }

        private void txtBlobName_TextChanged(object sender, EventArgs e)
        {
            TextBox txtSender = (TextBox)sender;

            if (rbExistingARMStorageAccount.Checked || rbStorageAccountInMigration.Checked) // We don't want to call / save text change on a Managed Disk, specifically when set to String.Empty would loose text value
            {
                _TargetDisk.TargetStorageAccountBlob = txtSender.Text.Trim();
                if (!_IsBinding)
                {
                    PropertyChanged?.Invoke();
                }
            }
        }

        private void txtTargetDiskName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTargetSize_TextChanged(object sender, EventArgs e)
        {
            if (_TargetDisk != null)
            {
                Int32 targetSizeInGb = -1;
                Int32.TryParse(txtTargetSize.Text, out targetSizeInGb);

                try
                {
                    _TargetDisk.DiskSizeInGB = targetSizeInGb;
                }
                catch (Exception exc)
                {
                    _TargetDisk.DiskSizeInGB = 0;
                    _AzureContext.LogProvider.WriteLog("txtTargetSize_TextChanged", exc.Message);
                }

                if (!_IsBinding)
                {
                    PropertyChanged?.Invoke();
                }
            }
        }

        private void txtTargetSize_Validating(object sender, CancelEventArgs e)
        {
            Int32 targetSizeInGb = -1;
            Int32.TryParse(txtTargetSize.Text, out targetSizeInGb);

            if (targetSizeInGb <= 0)
            {
                MessageBox.Show("Invalid Target Size in GB.");
                e.Cancel = true;
                return;
            }

            if (targetSizeInGb > 4095)
            {
                MessageBox.Show("Target Size in GB cannot exceed 4,095.");
                e.Cancel = true;
                return;
            }
        }

    }
}
