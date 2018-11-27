// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Azure.Core.Interface;
using MigAz.Azure.Interface;
using MigAz.Azure.MigrationTarget;
using MigAz.Azure.Core;

namespace MigAz.Azure.UserControls
{
    public partial class PublicIpSelectionControl : UserControl
    {
        private IMigrationPublicIp _PublicIpTarget;
        private Azure.UserControls.TargetTreeView _TargetTreeView;
        private bool _IsBinding = false;

        public delegate void AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public PublicIpSelectionControl()
        {
            InitializeComponent();
        }

        public async Task Bind(TargetTreeView targetTreeView)
        {
            _TargetTreeView = targetTreeView;

            try
            {
                _IsBinding = true;

                if (_TargetTreeView.TargetResourceGroup != null && _TargetTreeView.TargetResourceGroup.TargetLocation != null)
                {
                    int existingPublicIpCount = targetTreeView.GetExistingPublicIpsInTargetLocation().Count();

                    rbExistingARMPublicIp.Text = "Existing Public IP(s) in " + _TargetTreeView.TargetResourceGroup.TargetLocation.DisplayName;
                    if (existingPublicIpCount == 0)
                        rbExistingARMPublicIp.Text = "No " + rbExistingARMPublicIp.Text;

                    this.ExistingARMPublicIpEnabled = existingPublicIpCount > 0;
                }
                else
                {
                    // Cannot use existing ARM Public IP without Target Location
                    rbExistingARMPublicIp.Enabled = false;
                    rbExistingARMPublicIp.Text = "<Set Target Resource Group Location>";
                }
            }
            catch (Exception exc)
            {
                targetTreeView.LogProvider.WriteLog("VirtualMachineProperties.Bind", exc.Message);
                this.ExistingARMPublicIpEnabled = false;
            }
            finally
            {
                _IsBinding = false;
            }
        }

        public IMigrationPublicIp PublicIp
        {
            get { return _PublicIpTarget; }
            set
            {
                _PublicIpTarget = value;

                try
                {
                    _IsBinding = true;

                    if (_PublicIpTarget != null)
                    {
                        if (this.ExistingARMPublicIpEnabled == false ||
                            _PublicIpTarget == null ||
                            _PublicIpTarget.GetType() == typeof(Azure.MigrationTarget.PublicIp)
                            )
                        {
                            this.SelectPublicIpInMigration();
                        }
                        else
                        {
                            this.SelectExistingARMPublicIp();
                        }
                    }
                }
                finally
                {
                    _IsBinding = false;
                }
            }
        }

        public bool ExistingARMPublicIpEnabled
        {
            get { return rbExistingARMPublicIp.Enabled; }
            set { rbExistingARMPublicIp.Enabled = value; }
        }

        public void SelectPublicIpInMigration()
        {
            rbPublicIPInMigration.Checked = true;
        }

        public void SelectExistingARMPublicIp()
        {
            rbExistingARMPublicIp.Checked = true;
        }

        private async void rbPublicIPInMigration_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                #region Add "In MigAz Migration" Public Ips to cmbPublicIp

                cmbPublicIp.Items.Clear();

                foreach (Azure.MigrationTarget.PublicIp targetPublicIp in _TargetTreeView.GetPublicIPsInMigration())
                {
                    cmbPublicIp.Items.Add(targetPublicIp);
                }

                #endregion

                #region Seek Target Public and Subnet as ComboBox SelectedItems

                if (_PublicIpTarget != null)
                {
                    if (_PublicIpTarget.GetType() == typeof(PublicIp))
                    {
                        PublicIp targetPublicIp = (PublicIp)_PublicIpTarget;

                        // Attempt to match target to list items
                        foreach (Azure.MigrationTarget.PublicIp listPublicIp in cmbPublicIp.Items)
                        {
                            if (listPublicIp == targetPublicIp)
                            {
                                cmbPublicIp.SelectedItem = listPublicIp;
                                break;
                            }
                        }

                    }
                }

                #endregion

                if (cmbPublicIp.SelectedIndex < 0 && cmbPublicIp.Items.Count > 0)
                    cmbPublicIp.SelectedIndex = 0;

                if (!_IsBinding)
                    PropertyChanged?.Invoke();
            }
        }

        private async void rbExistingARMPublicIP_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {

                #region Add "Existing in Subscription / Location" ARM Public IPs to cmbPublicIp

                cmbPublicIp.Items.Clear();

                foreach (Arm.PublicIP armPublicIp in _TargetTreeView.GetExistingPublicIpsInTargetLocation())
                {
                    cmbPublicIp.Items.Add(armPublicIp);
                    if (_PublicIpTarget == armPublicIp)
                        cmbPublicIp.SelectedIndex = cmbPublicIp.Items.IndexOf(armPublicIp);
                }

                #endregion

                if (cmbPublicIp.SelectedIndex < 0 && cmbPublicIp.Items.Count > 0)
                    cmbPublicIp.SelectedIndex = 0;

                if (!_IsBinding)
                    PropertyChanged?.Invoke();
            }
        }

        private void cmbPublicIp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_IsBinding)
            {
                if (cmbPublicIp.SelectedItem == null)
                    _PublicIpTarget = null;
                else
                    _PublicIpTarget = (IMigrationPublicIp)cmbPublicIp.SelectedItem;

                PropertyChanged?.Invoke();
            }
        }

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                _IsBinding = true;

                try
                {
                    if (value == false)
                    {
                        rbExistingARMPublicIp.Checked = false;
                        rbPublicIPInMigration.Checked = false;
                        cmbPublicIp.Items.Clear();

                        PropertyChanged?.Invoke();
                    }
                }
                finally
                {
                    base.Enabled = value;
                    _IsBinding = false;
                }
            }
        }
    }
}

