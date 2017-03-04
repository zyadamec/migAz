namespace MigAz.Forms
{
    partial class PreExportDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cboTenants = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRGName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboRGLocation = new System.Windows.Forms.ComboBox();
            this.cboSubscription = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblExportFolder = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboTenants
            // 
            this.cboTenants.DisplayMember = "SubscriptionName";
            this.cboTenants.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenants.FormattingEnabled = true;
            this.cboTenants.Location = new System.Drawing.Point(274, 68);
            this.cboTenants.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboTenants.Name = "cboTenants";
            this.cboTenants.Size = new System.Drawing.Size(583, 33);
            this.cboTenants.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(39, 79);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 25);
            this.label8.TabIndex = 17;
            this.label8.Text = "Tenants:";
            // 
            // txtRGName
            // 
            this.txtRGName.Location = new System.Drawing.Point(274, 190);
            this.txtRGName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtRGName.Name = "txtRGName";
            this.txtRGName.Size = new System.Drawing.Size(390, 31);
            this.txtRGName.TabIndex = 14;
            this.txtRGName.Text = "MigratedResources";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(39, 257);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 25);
            this.label6.TabIndex = 15;
            this.label6.Text = "Location:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 198);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(223, 25);
            this.label5.TabIndex = 13;
            this.label5.Text = "New Resource Group:";
            // 
            // cboRGLocation
            // 
            this.cboRGLocation.FormattingEnabled = true;
            this.cboRGLocation.Items.AddRange(new object[] {
            "East US",
            "East US 2",
            "West US",
            "Central US",
            "North Central US",
            "South Central US",
            "North Europe",
            "West Europe",
            "East Asia",
            "Southeast Asia",
            "Japan East",
            "Japan West",
            "Australia East",
            "Australia Southeast",
            "Brazil South",
            "South India",
            "Central India",
            "West India",
            "Canada Central",
            "Canada East",
            "West US 2",
            "West Central US",
            "UK South",
            "UK West"});
            this.cboRGLocation.Location = new System.Drawing.Point(274, 252);
            this.cboRGLocation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboRGLocation.Name = "cboRGLocation";
            this.cboRGLocation.Size = new System.Drawing.Size(390, 33);
            this.cboRGLocation.TabIndex = 16;
            // 
            // cboSubscription
            // 
            this.cboSubscription.DisplayMember = "SubscriptionName";
            this.cboSubscription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubscription.FormattingEnabled = true;
            this.cboSubscription.Location = new System.Drawing.Point(274, 130);
            this.cboSubscription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cboSubscription.Name = "cboSubscription";
            this.cboSubscription.Size = new System.Drawing.Size(583, 33);
            this.cboSubscription.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 134);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Subscription:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(638, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "Now let’s get this template deployed to Azure Resource Manager!\r\n";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(441, 387);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(282, 44);
            this.btnExport.TabIndex = 19;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(952, 301);
            this.btnChoosePath.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(59, 44);
            this.btnChoosePath.TabIndex = 21;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(274, 308);
            this.txtDestinationFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(668, 31);
            this.txtDestinationFolder.TabIndex = 20;
            // 
            // lblExportFolder
            // 
            this.lblExportFolder.AutoSize = true;
            this.lblExportFolder.Location = new System.Drawing.Point(38, 314);
            this.lblExportFolder.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblExportFolder.Name = "lblExportFolder";
            this.lblExportFolder.Size = new System.Drawing.Size(147, 25);
            this.lblExportFolder.TabIndex = 22;
            this.lblExportFolder.Text = "Export Folder:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(746, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(282, 44);
            this.btnCancel.TabIndex = 23;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PreExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 465);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.lblExportFolder);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cboTenants);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtRGName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboRGLocation);
            this.Controls.Add(this.cboSubscription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "PreExportDialog";
            this.Text = "PreExportDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboTenants;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRGName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboRGLocation;
        private System.Windows.Forms.ComboBox cboSubscription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblExportFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnCancel;
    }
}