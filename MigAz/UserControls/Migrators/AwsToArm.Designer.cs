namespace MigAz.UserControls.Migrators
{
    partial class AwsToArm
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
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSignInText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lvwVirtualMachines = new System.Windows.Forms.ListView();
            this.colInstanceId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInstanceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvwVirtualNetworks = new System.Windows.Forms.ListView();
            this.colVpcId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVpcName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.btnGetToken = new System.Windows.Forms.Button();
            this.azureLoginContextViewer21 = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.SuspendLayout();
            // 
            // cmbRegion
            // 
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.Enabled = false;
            this.cmbRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbRegion.Location = new System.Drawing.Point(957, 258);
            this.cmbRegion.Margin = new System.Windows.Forms.Padding(6);
            this.cmbRegion.MaxDropDownItems = 15;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(631, 34);
            this.cmbRegion.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(854, 264);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 25);
            this.label4.TabIndex = 50;
            this.label4.Text = "Region";
            // 
            // lblSignInText
            // 
            this.lblSignInText.AutoSize = true;
            this.lblSignInText.Location = new System.Drawing.Point(663, 202);
            this.lblSignInText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSignInText.Name = "lblSignInText";
            this.lblSignInText.Size = new System.Drawing.Size(141, 25);
            this.lblSignInText.TabIndex = 49;
            this.lblSignInText.Text = "Not Signed In";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(917, 309);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 25);
            this.label3.TabIndex = 47;
            this.label3.Text = "EC2 Instances";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 295);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 25);
            this.label1.TabIndex = 48;
            this.label1.Text = "VPCs";
            // 
            // lvwVirtualMachines
            // 
            this.lvwVirtualMachines.CheckBoxes = true;
            this.lvwVirtualMachines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInstanceId,
            this.colInstanceName});
            this.lvwVirtualMachines.FullRowSelect = true;
            this.lvwVirtualMachines.Location = new System.Drawing.Point(907, 370);
            this.lvwVirtualMachines.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lvwVirtualMachines.Name = "lvwVirtualMachines";
            this.lvwVirtualMachines.Size = new System.Drawing.Size(728, 456);
            this.lvwVirtualMachines.TabIndex = 41;
            this.lvwVirtualMachines.UseCompatibleStateImageBehavior = false;
            this.lvwVirtualMachines.View = System.Windows.Forms.View.Details;
            // 
            // colInstanceId
            // 
            this.colInstanceId.Text = "Instance Id";
            this.colInstanceId.Width = 113;
            // 
            // colInstanceName
            // 
            this.colInstanceName.Text = "Intance Name";
            this.colInstanceName.Width = 163;
            // 
            // lvwVirtualNetworks
            // 
            this.lvwVirtualNetworks.CheckBoxes = true;
            this.lvwVirtualNetworks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colVpcId,
            this.colVpcName});
            this.lvwVirtualNetworks.Location = new System.Drawing.Point(307, 338);
            this.lvwVirtualNetworks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lvwVirtualNetworks.Name = "lvwVirtualNetworks";
            this.lvwVirtualNetworks.Size = new System.Drawing.Size(583, 488);
            this.lvwVirtualNetworks.TabIndex = 40;
            this.lvwVirtualNetworks.UseCompatibleStateImageBehavior = false;
            this.lvwVirtualNetworks.View = System.Windows.Forms.View.Details;
            // 
            // colVpcId
            // 
            this.colVpcId.Text = "Vpc Id";
            this.colVpcId.Width = 136;
            // 
            // colVpcName
            // 
            this.colVpcName.Text = "Vpc Name";
            this.colVpcName.Width = 146;
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(1286, 202);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(6);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(258, 44);
            this.btnOptions.TabIndex = 46;
            this.btnOptions.Text = "Options...";
            this.btnOptions.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(1276, 859);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(362, 44);
            this.btnExport.TabIndex = 44;
            this.btnExport.Text = "Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Enabled = false;
            this.btnChoosePath.Location = new System.Drawing.Point(1144, 855);
            this.btnChoosePath.Margin = new System.Windows.Forms.Padding(6);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(58, 44);
            this.btnChoosePath.TabIndex = 43;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Enabled = false;
            this.txtDestinationFolder.Location = new System.Drawing.Point(464, 859);
            this.txtDestinationFolder.Margin = new System.Windows.Forms.Padding(6);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(666, 31);
            this.txtDestinationFolder.TabIndex = 42;
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(304, 864);
            this.lblOutputFolder.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(149, 25);
            this.lblOutputFolder.TabIndex = 45;
            this.lblOutputFolder.Text = "Output Folder:";
            // 
            // btnGetToken
            // 
            this.btnGetToken.Location = new System.Drawing.Point(67, 85);
            this.btnGetToken.Margin = new System.Windows.Forms.Padding(6);
            this.btnGetToken.Name = "btnGetToken";
            this.btnGetToken.Size = new System.Drawing.Size(258, 44);
            this.btnGetToken.TabIndex = 52;
            this.btnGetToken.Text = "Sign In";
            this.btnGetToken.UseVisualStyleBackColor = true;
            // 
            // azureLoginContextViewer21
            // 
            this.azureLoginContextViewer21.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewer21.Location = new System.Drawing.Point(741, 16);
            this.azureLoginContextViewer21.Name = "azureLoginContextViewer21";
            this.azureLoginContextViewer21.Size = new System.Drawing.Size(894, 211);
            this.azureLoginContextViewer21.TabIndex = 53;
            this.azureLoginContextViewer21.Title = "Azure Subscription";
            // 
            // AwsToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.azureLoginContextViewer21);
            this.Controls.Add(this.btnGetToken);
            this.Controls.Add(this.cmbRegion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSignInText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwVirtualMachines);
            this.Controls.Add(this.lvwVirtualNetworks);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.lblOutputFolder);
            this.Name = "AwsToArm";
            this.Size = new System.Drawing.Size(1818, 1032);
            this.Load += new System.EventHandler(this.AwsToArm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRegion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSignInText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvwVirtualMachines;
        private System.Windows.Forms.ColumnHeader colInstanceId;
        private System.Windows.Forms.ColumnHeader colInstanceName;
        private System.Windows.Forms.ListView lvwVirtualNetworks;
        private System.Windows.Forms.ColumnHeader colVpcId;
        private System.Windows.Forms.ColumnHeader colVpcName;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewer21;
        private System.Windows.Forms.Button btnGetToken;
    }
}
