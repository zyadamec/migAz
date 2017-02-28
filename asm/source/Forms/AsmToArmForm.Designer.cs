namespace MigAzASM
{
    partial class AsmToArmForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmToArmForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnOptions = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAzureObjectName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.treeASM = new System.Windows.Forms.TreeView();
            this.treeARM = new System.Windows.Forms.TreeView();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.azureLoginContextViewer1 = new MigAz.Azure.Arm.UserControls.AzureLoginContextViewer();
            this.azureLoginContextViewer2 = new MigAz.Azure.Arm.UserControls.AzureLoginContextViewer();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 892);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 28, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1974, 37);
            this.statusStrip1.TabIndex = 29;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(1712, 32);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "Ready";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(231, 32);
            this.toolStripStatusLabel1.Text = "http://aka.ms/MigAz";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(1220, 755);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(723, 44);
            this.btnOptions.TabIndex = 6;
            this.btnOptions.Text = "Options...";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ResourceGroup");
            this.imageList1.Images.SetKeyName(1, "Disk");
            this.imageList1.Images.SetKeyName(2, "LoadBalancer");
            this.imageList1.Images.SetKeyName(3, "NetworkInterface.png");
            this.imageList1.Images.SetKeyName(4, "NetworkSecurityGroup");
            this.imageList1.Images.SetKeyName(5, "PublicIPAddress.png");
            this.imageList1.Images.SetKeyName(6, "StorageAccount");
            this.imageList1.Images.SetKeyName(7, "VirtualMachine");
            this.imageList1.Images.SetKeyName(8, "AvailabilitySet");
            this.imageList1.Images.SetKeyName(9, "VirtualNetwork");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.lblAzureObjectName);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(1220, 207);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(723, 473);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Azure Resource Manager Properties";
            this.groupBox1.Resize += new System.EventHandler(this.groupBox1_Resize);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(24, 166);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 286);
            this.panel1.TabIndex = 3;
            // 
            // lblAzureObjectName
            // 
            this.lblAzureObjectName.AutoSize = true;
            this.lblAzureObjectName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzureObjectName.Location = new System.Drawing.Point(130, 68);
            this.lblAzureObjectName.Name = "lblAzureObjectName";
            this.lblAzureObjectName.Size = new System.Drawing.Size(330, 45);
            this.lblAzureObjectName.TabIndex = 1;
            this.lblAzureObjectName.Text = "lblAzureObjectName";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(24, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(1220, 689);
            this.btnExport.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(723, 44);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // treeASM
            // 
            this.treeASM.CheckBoxes = true;
            this.treeASM.Location = new System.Drawing.Point(20, 230);
            this.treeASM.Name = "treeASM";
            this.treeASM.Size = new System.Drawing.Size(597, 570);
            this.treeASM.TabIndex = 2;
            this.treeASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeASM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // treeARM
            // 
            this.treeARM.Location = new System.Drawing.Point(635, 230);
            this.treeARM.Name = "treeARM";
            this.treeARM.Size = new System.Drawing.Size(561, 570);
            this.treeARM.TabIndex = 3;
            this.treeARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeARM_AfterSelect);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(20, 199);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(255, 25);
            this.label5.TabIndex = 51;
            this.label5.Text = "Source (ASM) Resources";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(630, 199);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(250, 25);
            this.label6.TabIndex = 52;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // azureLoginContextViewer1
            // 
            this.azureLoginContextViewer1.Location = new System.Drawing.Point(20, 12);
            this.azureLoginContextViewer1.Name = "azureLoginContextViewer1";
            this.azureLoginContextViewer1.Size = new System.Drawing.Size(894, 168);
            this.azureLoginContextViewer1.TabIndex = 53;
            this.azureLoginContextViewer1.Title = "Azure Subscription";
            // 
            // azureLoginContextViewer2
            // 
            this.azureLoginContextViewer2.Location = new System.Drawing.Point(1049, 12);
            this.azureLoginContextViewer2.Name = "azureLoginContextViewer2";
            this.azureLoginContextViewer2.Size = new System.Drawing.Size(894, 168);
            this.azureLoginContextViewer2.TabIndex = 54;
            this.azureLoginContextViewer2.Title = "Azure Subscription";
            // 
            // AsmToArmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1974, 929);
            this.Controls.Add(this.azureLoginContextViewer2);
            this.Controls.Add(this.azureLoginContextViewer1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeARM);
            this.Controls.Add(this.treeASM);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximumSize = new System.Drawing.Size(2000, 5000);
            this.MinimumSize = new System.Drawing.Size(2000, 1000);
            this.Name = "AsmToArmForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MigAz";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AsmToArmForm_FormClosing);
            this.Load += new System.EventHandler(this.AsmToArmForm_Load);
            this.Resize += new System.EventHandler(this.AsmToArmForm_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAzureObjectName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeASM;
        private System.Windows.Forms.TreeView treeARM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private MigAz.Azure.Arm.UserControls.AzureLoginContextViewer azureLoginContextViewer1;
        private MigAz.Azure.Arm.UserControls.AzureLoginContextViewer azureLoginContextViewer2;
    }
}

