namespace MIGAZ
{
    partial class Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.btnGetToken = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnOptions = new System.Windows.Forms.Button();
            this.lvwVirtualNetworks = new System.Windows.Forms.ListView();
            this.colVpcId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVpcName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lvwVirtualMachines = new System.Windows.Forms.ListView();
            this.colInstanceId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInstanceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSignInText = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetToken
            // 
            this.btnGetToken.Location = new System.Drawing.Point(32, 16);
            this.btnGetToken.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetToken.Name = "btnGetToken";
            this.btnGetToken.Size = new System.Drawing.Size(172, 28);
            this.btnGetToken.TabIndex = 0;
            this.btnGetToken.Text = "Sign In";
            this.btnGetToken.UseVisualStyleBackColor = true;
            this.btnGetToken.Click += new System.EventHandler(this.btnGetToken_Click);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(676, 635);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(241, 28);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Enabled = false;
            this.btnChoosePath.Location = new System.Drawing.Point(588, 632);
            this.btnChoosePath.Margin = new System.Windows.Forms.Padding(4);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(39, 28);
            this.btnChoosePath.TabIndex = 6;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Enabled = false;
            this.txtDestinationFolder.Location = new System.Drawing.Point(135, 635);
            this.txtDestinationFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(445, 22);
            this.txtDestinationFolder.TabIndex = 5;
            this.txtDestinationFolder.TextChanged += new System.EventHandler(this.txtDestinationFolder_TextChanged);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(28, 638);
            this.lblOutputFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(99, 17);
            this.lblOutputFolder.TabIndex = 26;
            this.lblOutputFolder.Text = "Output Folder:";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 677);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(942, 25);
            this.statusStrip1.TabIndex = 29;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.Text = "Ready";
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(32, 62);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(4);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(172, 28);
            this.btnOptions.TabIndex = 34;
            this.btnOptions.Text = "Options...";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // lvwVirtualNetworks
            // 
            this.lvwVirtualNetworks.CheckBoxes = true;
            this.lvwVirtualNetworks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colVpcId,
            this.colVpcName});
            this.lvwVirtualNetworks.Location = new System.Drawing.Point(30, 126);
            this.lvwVirtualNetworks.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvwVirtualNetworks.Name = "lvwVirtualNetworks";
            this.lvwVirtualNetworks.Size = new System.Drawing.Size(390, 489);
            this.lvwVirtualNetworks.TabIndex = 2;
            this.lvwVirtualNetworks.UseCompatibleStateImageBehavior = false;
            this.lvwVirtualNetworks.View = System.Windows.Forms.View.Details;
            this.lvwVirtualNetworks.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwVirtualNetworks_ItemChecked);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 36;
            this.label1.Text = "VPCs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(427, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 36;
            this.label3.Text = "EC2 Instances";
            // 
            // lvwVirtualMachines
            // 
            this.lvwVirtualMachines.CheckBoxes = true;
            this.lvwVirtualMachines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInstanceId,
            this.colInstanceName});
            this.lvwVirtualMachines.FullRowSelect = true;
            this.lvwVirtualMachines.Location = new System.Drawing.Point(430, 126);
            this.lvwVirtualMachines.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lvwVirtualMachines.Name = "lvwVirtualMachines";
            this.lvwVirtualMachines.Size = new System.Drawing.Size(487, 489);
            this.lvwVirtualMachines.TabIndex = 4;
            this.lvwVirtualMachines.UseCompatibleStateImageBehavior = false;
            this.lvwVirtualMachines.View = System.Windows.Forms.View.Details;
            this.lvwVirtualMachines.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvwVirtualMachines_ItemChecked);
            this.lvwVirtualMachines.SelectedIndexChanged += new System.EventHandler(this.lvwVirtualMachines_SelectedIndexChanged);
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
            // lblSignInText
            // 
            this.lblSignInText.AutoSize = true;
            this.lblSignInText.Location = new System.Drawing.Point(231, 22);
            this.lblSignInText.Name = "lblSignInText";
            this.lblSignInText.Size = new System.Drawing.Size(93, 17);
            this.lblSignInText.TabIndex = 37;
            this.lblSignInText.Text = "Not Signed In";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(426, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 38;
            this.label4.Text = "Region";
            // 
            // cmbRegion
            // 
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.Enabled = false;
            this.cmbRegion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbRegion.Location = new System.Drawing.Point(495, 65);
            this.cmbRegion.Margin = new System.Windows.Forms.Padding(4);
            this.cmbRegion.MaxDropDownItems = 15;
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(422, 25);
            this.cmbRegion.TabIndex = 39;
            this.cmbRegion.SelectedIndexChanged += new System.EventHandler(this.cmbRegion_SelectedIndexChanged);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(942, 702);
            this.Controls.Add(this.cmbRegion);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSignInText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwVirtualMachines);
            this.Controls.Add(this.lvwVirtualNetworks);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.lblOutputFolder);
            this.Controls.Add(this.btnGetToken);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "migAz";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
            this.Load += new System.EventHandler(this.Window_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGetToken;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.ListView lvwVirtualNetworks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView lvwVirtualMachines;
        private System.Windows.Forms.Label lblSignInText;
        private System.Windows.Forms.ColumnHeader colInstanceId;
        private System.Windows.Forms.ColumnHeader colInstanceName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRegion;
        private System.Windows.Forms.ColumnHeader colVpcId;
        private System.Windows.Forms.ColumnHeader colVpcName;
    }
}

