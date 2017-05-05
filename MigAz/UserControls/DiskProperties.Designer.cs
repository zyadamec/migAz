namespace MigAz.UserControls
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Disk Name:";
            // 
            // lblDiskName
            // 
            this.lblDiskName.AutoSize = true;
            this.lblDiskName.Location = new System.Drawing.Point(200, 45);
            this.lblDiskName.Name = "lblDiskName";
            this.lblDiskName.Size = new System.Drawing.Size(151, 25);
            this.lblDiskName.TabIndex = 1;
            this.lblDiskName.Text = "DiskName.vhd";
            // 
            // lblHostCaching
            // 
            this.lblHostCaching.AutoSize = true;
            this.lblHostCaching.Location = new System.Drawing.Point(200, 80);
            this.lblHostCaching.Name = "lblHostCaching";
            this.lblHostCaching.Size = new System.Drawing.Size(63, 25);
            this.lblHostCaching.TabIndex = 3;
            this.lblHostCaching.Text = "None";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Host Caching:";
            // 
            // lblLUN
            // 
            this.lblLUN.AutoSize = true;
            this.lblLUN.Location = new System.Drawing.Point(200, 115);
            this.lblLUN.Name = "lblLUN";
            this.lblLUN.Size = new System.Drawing.Size(24, 25);
            this.lblLUN.TabIndex = 5;
            this.lblLUN.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "LUN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(177, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Storage Account:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 349);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 25);
            this.label5.TabIndex = 17;
            this.label5.Text = "Migrate To:";
            // 
            // rbExistingARMStorageAccount
            // 
            this.rbExistingARMStorageAccount.AutoSize = true;
            this.rbExistingARMStorageAccount.Location = new System.Drawing.Point(200, 431);
            this.rbExistingARMStorageAccount.Name = "rbExistingARMStorageAccount";
            this.rbExistingARMStorageAccount.Size = new System.Drawing.Size(344, 29);
            this.rbExistingARMStorageAccount.TabIndex = 2;
            this.rbExistingARMStorageAccount.Text = "Existing ARM Storage in Target";
            this.rbExistingARMStorageAccount.UseVisualStyleBackColor = true;
            this.rbExistingARMStorageAccount.CheckedChanged += new System.EventHandler(this.rbExistingARMStorageAccount_CheckedChanged);
            // 
            // rbStorageAccountInMigration
            // 
            this.rbStorageAccountInMigration.AutoSize = true;
            this.rbStorageAccountInMigration.Location = new System.Drawing.Point(200, 389);
            this.rbStorageAccountInMigration.Name = "rbStorageAccountInMigration";
            this.rbStorageAccountInMigration.Size = new System.Drawing.Size(302, 29);
            this.rbStorageAccountInMigration.TabIndex = 1;
            this.rbStorageAccountInMigration.Text = "Storage in MigAz Migration";
            this.rbStorageAccountInMigration.UseVisualStyleBackColor = true;
            this.rbStorageAccountInMigration.CheckedChanged += new System.EventHandler(this.rbStorageAccountInMigration_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 477);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 25);
            this.label7.TabIndex = 19;
            this.label7.Text = "Target Storage:";
            // 
            // cmbTargetStorage
            // 
            this.cmbTargetStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetStorage.Enabled = false;
            this.cmbTargetStorage.FormattingEnabled = true;
            this.cmbTargetStorage.Location = new System.Drawing.Point(200, 474);
            this.cmbTargetStorage.Name = "cmbTargetStorage";
            this.cmbTargetStorage.Size = new System.Drawing.Size(392, 33);
            this.cmbTargetStorage.TabIndex = 3;
            this.cmbTargetStorage.SelectedIndexChanged += new System.EventHandler(this.cmbTargetStorage_SelectedIndexChanged);
            // 
            // lblAsmStorageAccount
            // 
            this.lblAsmStorageAccount.AutoSize = true;
            this.lblAsmStorageAccount.Location = new System.Drawing.Point(200, 150);
            this.lblAsmStorageAccount.Name = "lblAsmStorageAccount";
            this.lblAsmStorageAccount.Size = new System.Drawing.Size(83, 25);
            this.lblAsmStorageAccount.TabIndex = 20;
            this.lblAsmStorageAccount.Text = "abc123";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(326, 25);
            this.label6.TabIndex = 21;
            this.label6.Text = "ASM (Source) Disk Properties";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 218);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(321, 25);
            this.label8.TabIndex = 22;
            this.label8.Text = "ARM (Target) Disk Properties";
            // 
            // txtTargetDiskName
            // 
            this.txtTargetDiskName.Location = new System.Drawing.Point(200, 261);
            this.txtTargetDiskName.Name = "txtTargetDiskName";
            this.txtTargetDiskName.Size = new System.Drawing.Size(387, 31);
            this.txtTargetDiskName.TabIndex = 0;
            this.txtTargetDiskName.TextChanged += new System.EventHandler(this.txtTargetDiskName_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 264);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(122, 25);
            this.label9.TabIndex = 24;
            this.label9.Text = "Disk Name:";
            // 
            // rbManagedDisk
            // 
            this.rbManagedDisk.AutoSize = true;
            this.rbManagedDisk.Enabled = false;
            this.rbManagedDisk.Location = new System.Drawing.Point(200, 347);
            this.rbManagedDisk.Name = "rbManagedDisk";
            this.rbManagedDisk.Size = new System.Drawing.Size(383, 29);
            this.rbManagedDisk.TabIndex = 25;
            this.rbManagedDisk.Text = "ARM Managed Disk (Coming Soon)";
            this.rbManagedDisk.UseVisualStyleBackColor = true;
            this.rbManagedDisk.CheckedChanged += new System.EventHandler(this.rbManagedDIsk_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 307);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 25);
            this.label10.TabIndex = 27;
            this.label10.Text = "Blob Name:";
            // 
            // txtBlobName
            // 
            this.txtBlobName.Location = new System.Drawing.Point(200, 304);
            this.txtBlobName.Name = "txtBlobName";
            this.txtBlobName.Size = new System.Drawing.Size(387, 31);
            this.txtBlobName.TabIndex = 26;
            this.txtBlobName.TextChanged += new System.EventHandler(this.txtBlobName_TextChanged);
            // 
            // DiskProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Name = "DiskProperties";
            this.Size = new System.Drawing.Size(640, 533);
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
    }
}
