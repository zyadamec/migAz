// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class DiskProperties
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblDiskName = new System.Windows.Forms.Label();
            this.lblHostCaching = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLUN = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rbExistingARMStorageAccount = new System.Windows.Forms.RadioButton();
            this.rbStorageAccountInMigration = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbTargetStorage = new System.Windows.Forms.ComboBox();
            this.lblAsmStorageAccount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTargetDiskName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rbManagedDisk = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBlobName = new System.Windows.Forms.TextBox();
            this.lblSourceSizeGb = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTargetSize = new System.Windows.Forms.TextBox();
            this.virtualMachineSummary = new MigAz.Azure.UserControls.ResourceSummary();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.upDownLUN = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.txtDiskEncryptionKeySourceVaultId = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtDiskEncryptionKeySecretUrl = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtKeyEncryptionKeySourceVaultId = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtKeyEncryptionKeyKeyUrl = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.upDownLUN)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Disk Name:";
            // 
            // lblDiskName
            // 
            this.lblDiskName.AutoSize = true;
            this.lblDiskName.Location = new System.Drawing.Point(137, 33);
            this.lblDiskName.Name = "lblDiskName";
            this.lblDiskName.Size = new System.Drawing.Size(99, 17);
            this.lblDiskName.TabIndex = 1;
            this.lblDiskName.Text = "DiskName.vhd";
            // 
            // lblHostCaching
            // 
            this.lblHostCaching.AutoSize = true;
            this.lblHostCaching.Location = new System.Drawing.Point(137, 57);
            this.lblHostCaching.Name = "lblHostCaching";
            this.lblHostCaching.Size = new System.Drawing.Size(42, 17);
            this.lblHostCaching.TabIndex = 3;
            this.lblHostCaching.Text = "None";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Host Caching:";
            // 
            // lblLUN
            // 
            this.lblLUN.AutoSize = true;
            this.lblLUN.Location = new System.Drawing.Point(137, 80);
            this.lblLUN.Name = "lblLUN";
            this.lblLUN.Size = new System.Drawing.Size(16, 17);
            this.lblLUN.TabIndex = 5;
            this.lblLUN.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "LUN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Storage Account:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "Migrate To:";
            // 
            // rbExistingARMStorageAccount
            // 
            this.rbExistingARMStorageAccount.AutoSize = true;
            this.rbExistingARMStorageAccount.Enabled = false;
            this.rbExistingARMStorageAccount.Location = new System.Drawing.Point(137, 271);
            this.rbExistingARMStorageAccount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbExistingARMStorageAccount.Name = "rbExistingARMStorageAccount";
            this.rbExistingARMStorageAccount.Size = new System.Drawing.Size(226, 21);
            this.rbExistingARMStorageAccount.TabIndex = 3;
            this.rbExistingARMStorageAccount.Text = "Existing ARM Storage in Target";
            this.rbExistingARMStorageAccount.UseVisualStyleBackColor = true;
            this.rbExistingARMStorageAccount.CheckedChanged += new System.EventHandler(this.rbExistingARMStorageAccount_CheckedChanged);
            // 
            // rbStorageAccountInMigration
            // 
            this.rbStorageAccountInMigration.AutoSize = true;
            this.rbStorageAccountInMigration.Location = new System.Drawing.Point(137, 244);
            this.rbStorageAccountInMigration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbStorageAccountInMigration.Name = "rbStorageAccountInMigration";
            this.rbStorageAccountInMigration.Size = new System.Drawing.Size(198, 21);
            this.rbStorageAccountInMigration.TabIndex = 2;
            this.rbStorageAccountInMigration.Text = "Storage in MigAz Migration";
            this.rbStorageAccountInMigration.UseVisualStyleBackColor = true;
            this.rbStorageAccountInMigration.CheckedChanged += new System.EventHandler(this.rbStorageAccountInMigration_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 300);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 17);
            this.label7.TabIndex = 19;
            this.label7.Text = "Target Storage:";
            // 
            // cmbTargetStorage
            // 
            this.cmbTargetStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetStorage.Enabled = false;
            this.cmbTargetStorage.FormattingEnabled = true;
            this.cmbTargetStorage.Location = new System.Drawing.Point(137, 298);
            this.cmbTargetStorage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTargetStorage.Name = "cmbTargetStorage";
            this.cmbTargetStorage.Size = new System.Drawing.Size(263, 24);
            this.cmbTargetStorage.Sorted = true;
            this.cmbTargetStorage.TabIndex = 4;
            this.cmbTargetStorage.SelectedIndexChanged += new System.EventHandler(this.cmbTargetStorage_SelectedIndexChanged);
            // 
            // lblAsmStorageAccount
            // 
            this.lblAsmStorageAccount.AutoSize = true;
            this.lblAsmStorageAccount.Location = new System.Drawing.Point(137, 124);
            this.lblAsmStorageAccount.Name = "lblAsmStorageAccount";
            this.lblAsmStorageAccount.Size = new System.Drawing.Size(55, 17);
            this.lblAsmStorageAccount.TabIndex = 20;
            this.lblAsmStorageAccount.Text = "abc123";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 17);
            this.label6.TabIndex = 21;
            this.label6.Text = "Source";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 17);
            this.label8.TabIndex = 22;
            this.label8.Text = "Target";
            // 
            // txtTargetDiskName
            // 
            this.txtTargetDiskName.Location = new System.Drawing.Point(137, 341);
            this.txtTargetDiskName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTargetDiskName.Name = "txtTargetDiskName";
            this.txtTargetDiskName.Size = new System.Drawing.Size(260, 22);
            this.txtTargetDiskName.TabIndex = 5;
            this.txtTargetDiskName.TextChanged += new System.EventHandler(this.txtTargetDiskName_TextChanged);
            this.txtTargetDiskName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetDiskName_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 342);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 17);
            this.label9.TabIndex = 24;
            this.label9.Text = "Disk Name:";
            // 
            // rbManagedDisk
            // 
            this.rbManagedDisk.AutoSize = true;
            this.rbManagedDisk.Location = new System.Drawing.Point(137, 217);
            this.rbManagedDisk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbManagedDisk.Name = "rbManagedDisk";
            this.rbManagedDisk.Size = new System.Drawing.Size(119, 21);
            this.rbManagedDisk.TabIndex = 1;
            this.rbManagedDisk.Text = "Managed Disk";
            this.rbManagedDisk.UseVisualStyleBackColor = true;
            this.rbManagedDisk.CheckedChanged += new System.EventHandler(this.rbManagedDIsk_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 370);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 17);
            this.label10.TabIndex = 27;
            this.label10.Text = "Blob Name:";
            // 
            // txtBlobName
            // 
            this.txtBlobName.Location = new System.Drawing.Point(137, 368);
            this.txtBlobName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBlobName.Name = "txtBlobName";
            this.txtBlobName.Size = new System.Drawing.Size(260, 22);
            this.txtBlobName.TabIndex = 6;
            this.txtBlobName.TextChanged += new System.EventHandler(this.txtBlobName_TextChanged);
            this.txtBlobName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetDiskName_KeyPress);
            // 
            // lblSourceSizeGb
            // 
            this.lblSourceSizeGb.AutoSize = true;
            this.lblSourceSizeGb.Location = new System.Drawing.Point(137, 102);
            this.lblSourceSizeGb.Name = "lblSourceSizeGb";
            this.lblSourceSizeGb.Size = new System.Drawing.Size(16, 17);
            this.lblSourceSizeGb.TabIndex = 29;
            this.lblSourceSizeGb.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(15, 102);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 17);
            this.label12.TabIndex = 28;
            this.label12.Text = "Size (GB):";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 400);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 17);
            this.label11.TabIndex = 31;
            this.label11.Text = "Size:";
            // 
            // txtTargetSize
            // 
            this.txtTargetSize.Location = new System.Drawing.Point(137, 398);
            this.txtTargetSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTargetSize.MaxLength = 4;
            this.txtTargetSize.Name = "txtTargetSize";
            this.txtTargetSize.Size = new System.Drawing.Size(69, 22);
            this.txtTargetSize.TabIndex = 7;
            this.txtTargetSize.TextChanged += new System.EventHandler(this.txtTargetSize_TextChanged);
            this.txtTargetSize.Validating += new System.ComponentModel.CancelEventHandler(this.txtTargetSize_Validating);
            // 
            // virtualMachineSummary
            // 
            this.virtualMachineSummary.AutoSize = true;
            this.virtualMachineSummary.Location = new System.Drawing.Point(137, 182);
            this.virtualMachineSummary.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.virtualMachineSummary.Name = "virtualMachineSummary";
            this.virtualMachineSummary.Size = new System.Drawing.Size(257, 34);
            this.virtualMachineSummary.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 182);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(109, 17);
            this.label13.TabIndex = 33;
            this.label13.Text = "Virtual Machine:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 428);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 17);
            this.label14.TabIndex = 34;
            this.label14.Text = "LUN:";
            // 
            // upDownLUN
            // 
            this.upDownLUN.Location = new System.Drawing.Point(137, 427);
            this.upDownLUN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.upDownLUN.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.upDownLUN.Name = "upDownLUN";
            this.upDownLUN.Size = new System.Drawing.Size(59, 22);
            this.upDownLUN.TabIndex = 8;
            this.upDownLUN.ValueChanged += new System.EventHandler(this.upDownLUN_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(15, 484);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(218, 17);
            this.label15.TabIndex = 38;
            this.label15.Text = "DiskEncryptionKeySourceVaultId:";
            // 
            // txtDiskEncryptionKeySourceVaultId
            // 
            this.txtDiskEncryptionKeySourceVaultId.Location = new System.Drawing.Point(137, 482);
            this.txtDiskEncryptionKeySourceVaultId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDiskEncryptionKeySourceVaultId.Name = "txtDiskEncryptionKeySourceVaultId";
            this.txtDiskEncryptionKeySourceVaultId.Size = new System.Drawing.Size(260, 22);
            this.txtDiskEncryptionKeySourceVaultId.TabIndex = 36;
            this.txtDiskEncryptionKeySourceVaultId.TextChanged += new System.EventHandler(this.txtDiskEncryptionKeySourceVaultId_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(15, 456);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(189, 17);
            this.label16.TabIndex = 37;
            this.label16.Text = "DiskEncryptionKeySecretUrl:";
            // 
            // txtDiskEncryptionKeySecretUrl
            // 
            this.txtDiskEncryptionKeySecretUrl.Location = new System.Drawing.Point(137, 455);
            this.txtDiskEncryptionKeySecretUrl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDiskEncryptionKeySecretUrl.Name = "txtDiskEncryptionKeySecretUrl";
            this.txtDiskEncryptionKeySecretUrl.Size = new System.Drawing.Size(260, 22);
            this.txtDiskEncryptionKeySecretUrl.TabIndex = 35;
            this.txtDiskEncryptionKeySecretUrl.TextChanged += new System.EventHandler(this.txtDiskEncryptionKeySecretUrl_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(15, 537);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(215, 17);
            this.label17.TabIndex = 42;
            this.label17.Text = "KeyEncryptionKeySourceVaultId:";
            // 
            // txtKeyEncryptionKeySourceVaultId
            // 
            this.txtKeyEncryptionKeySourceVaultId.Location = new System.Drawing.Point(137, 535);
            this.txtKeyEncryptionKeySourceVaultId.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKeyEncryptionKeySourceVaultId.Name = "txtKeyEncryptionKeySourceVaultId";
            this.txtKeyEncryptionKeySourceVaultId.Size = new System.Drawing.Size(260, 22);
            this.txtKeyEncryptionKeySourceVaultId.TabIndex = 40;
            this.txtKeyEncryptionKeySourceVaultId.TextChanged += new System.EventHandler(this.txtKeyEncryptionKeySourceVaultId_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 509);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(169, 17);
            this.label18.TabIndex = 41;
            this.label18.Text = "KeyEncryptionKeyKeyUrl:";
            // 
            // txtKeyEncryptionKeyKeyUrl
            // 
            this.txtKeyEncryptionKeyKeyUrl.Location = new System.Drawing.Point(137, 508);
            this.txtKeyEncryptionKeyKeyUrl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtKeyEncryptionKeyKeyUrl.Name = "txtKeyEncryptionKeyKeyUrl";
            this.txtKeyEncryptionKeyKeyUrl.Size = new System.Drawing.Size(260, 22);
            this.txtKeyEncryptionKeyKeyUrl.TabIndex = 39;
            this.txtKeyEncryptionKeyKeyUrl.TextChanged += new System.EventHandler(this.txtKeyEncryptionKeyKeyUrl_TextChanged);
            // 
            // DiskProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label17);
            this.Controls.Add(this.txtKeyEncryptionKeySourceVaultId);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.txtKeyEncryptionKeyKeyUrl);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtDiskEncryptionKeySourceVaultId);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtDiskEncryptionKeySecretUrl);
            this.Controls.Add(this.upDownLUN);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.virtualMachineSummary);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtTargetSize);
            this.Controls.Add(this.lblSourceSizeGb);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBlobName);
            this.Controls.Add(this.rbManagedDisk);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtTargetDiskName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblAsmStorageAccount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmbTargetStorage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.rbExistingARMStorageAccount);
            this.Controls.Add(this.rbStorageAccountInMigration);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblLUN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblHostCaching);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDiskName);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DiskProperties";
            this.Size = new System.Drawing.Size(427, 824);
            ((System.ComponentModel.ISupportInitialize)(this.upDownLUN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDiskName;
        private System.Windows.Forms.Label lblHostCaching;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblLUN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbExistingARMStorageAccount;
        private System.Windows.Forms.RadioButton rbStorageAccountInMigration;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbTargetStorage;
        private System.Windows.Forms.Label lblAsmStorageAccount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTargetDiskName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rbManagedDisk;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBlobName;
        private System.Windows.Forms.Label lblSourceSizeGb;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTargetSize;
        private ResourceSummary virtualMachineSummary;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown upDownLUN;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtDiskEncryptionKeySourceVaultId;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtDiskEncryptionKeySecretUrl;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtKeyEncryptionKeySourceVaultId;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtKeyEncryptionKeyKeyUrl;
    }
}

