namespace MigAz.UserControls.Migrators
{
    partial class AsmToArm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmToArm));
            this.btnOptions = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAzureObjectName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.azureLoginContextViewer2 = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.treeARM = new System.Windows.Forms.TreeView();
            this.treeASM = new System.Windows.Forms.TreeView();
            this.btnExport = new System.Windows.Forms.Button();
            this.azureLoginContextViewer21 = new MigAz.Azure.UserControls.AzureLoginContextViewer2();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(1203, 746);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(723, 44);
            this.btnOptions.TabIndex = 60;
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
            this.groupBox1.Location = new System.Drawing.Point(1203, 198);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(723, 473);
            this.groupBox1.TabIndex = 58;
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
            // azureLoginContextViewer2
            // 
            this.azureLoginContextViewer2.ChangeType = MigAz.Azure.UserControls.ChangeType.SubscriptionOnly;
            this.azureLoginContextViewer2.Enabled = false;
            this.azureLoginContextViewer2.Location = new System.Drawing.Point(1032, 3);
            this.azureLoginContextViewer2.Name = "azureLoginContextViewer2";
            this.azureLoginContextViewer2.Size = new System.Drawing.Size(894, 168);
            this.azureLoginContextViewer2.TabIndex = 63;
            this.azureLoginContextViewer2.Title = "Azure ARM (Target) Subscription";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(613, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(250, 25);
            this.label6.TabIndex = 62;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(255, 25);
            this.label5.TabIndex = 61;
            this.label5.Text = "Source (ASM) Resources";
            // 
            // treeARM
            // 
            this.treeARM.Location = new System.Drawing.Point(618, 221);
            this.treeARM.Name = "treeARM";
            this.treeARM.Size = new System.Drawing.Size(561, 570);
            this.treeARM.TabIndex = 57;
            this.treeARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeARM_AfterSelect);
            // 
            // treeASM
            // 
            this.treeASM.CheckBoxes = true;
            this.treeASM.Location = new System.Drawing.Point(3, 221);
            this.treeASM.Name = "treeASM";
            this.treeASM.Size = new System.Drawing.Size(597, 570);
            this.treeASM.TabIndex = 56;
            this.treeASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeASM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(1203, 680);
            this.btnExport.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(723, 44);
            this.btnExport.TabIndex = 59;
            this.btnExport.Text = "Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // azureLoginContextViewer21
            // 
            this.azureLoginContextViewer21.ChangeType = MigAz.Azure.UserControls.ChangeType.Full;
            this.azureLoginContextViewer21.Location = new System.Drawing.Point(3, 3);
            this.azureLoginContextViewer21.Name = "azureLoginContextViewer21";
            this.azureLoginContextViewer21.Size = new System.Drawing.Size(894, 211);
            this.azureLoginContextViewer21.TabIndex = 64;
            this.azureLoginContextViewer21.Title = "Azure Subscription";
            // 
            // AsmToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.azureLoginContextViewer2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeARM);
            this.Controls.Add(this.treeASM);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.azureLoginContextViewer21);
            this.Name = "AsmToArm";
            this.Size = new System.Drawing.Size(1947, 915);
            this.Load += new System.EventHandler(this.AsmToArmForm_Load);
            this.Resize += new System.EventHandler(this.AsmToArmForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAzureObjectName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewer2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView treeARM;
        private System.Windows.Forms.TreeView treeASM;
        private System.Windows.Forms.Button btnExport;
        private Azure.UserControls.AzureLoginContextViewer2 azureLoginContextViewer21;
    }
}
