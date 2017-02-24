namespace MIGAZ
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
            this.groupASMSubscription = new System.Windows.Forms.GroupBox();
            this.lblSourceSubscriptionId = new System.Windows.Forms.Label();
            this.lblSourceSubscriptionName = new System.Windows.Forms.Label();
            this.lblSourceUser = new System.Windows.Forms.Label();
            this.lblSourceEnvironment = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAzureContextASM = new System.Windows.Forms.Button();
            this.groupARMSubscription = new System.Windows.Forms.GroupBox();
            this.lblTargetSubscriptionId = new System.Windows.Forms.Label();
            this.lblTargetSubscriptionName = new System.Windows.Forms.Label();
            this.lblTargetUser = new System.Windows.Forms.Label();
            this.lblTargetEnvironment = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAzureContextARM = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupASMSubscription.SuspendLayout();
            this.groupARMSubscription.SuspendLayout();
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
            // groupASMSubscription
            // 
            this.groupASMSubscription.Controls.Add(this.lblSourceSubscriptionId);
            this.groupASMSubscription.Controls.Add(this.lblSourceSubscriptionName);
            this.groupASMSubscription.Controls.Add(this.lblSourceUser);
            this.groupASMSubscription.Controls.Add(this.lblSourceEnvironment);
            this.groupASMSubscription.Controls.Add(this.label4);
            this.groupASMSubscription.Controls.Add(this.label3);
            this.groupASMSubscription.Controls.Add(this.label2);
            this.groupASMSubscription.Controls.Add(this.label1);
            this.groupASMSubscription.Controls.Add(this.btnAzureContextASM);
            this.groupASMSubscription.Location = new System.Drawing.Point(20, 25);
            this.groupASMSubscription.Name = "groupASMSubscription";
            this.groupASMSubscription.Size = new System.Drawing.Size(883, 158);
            this.groupASMSubscription.TabIndex = 0;
            this.groupASMSubscription.TabStop = false;
            this.groupASMSubscription.Text = "Source (ASM) Subscription";
            // 
            // lblSourceSubscriptionId
            // 
            this.lblSourceSubscriptionId.AutoSize = true;
            this.lblSourceSubscriptionId.Location = new System.Drawing.Point(224, 117);
            this.lblSourceSubscriptionId.Name = "lblSourceSubscriptionId";
            this.lblSourceSubscriptionId.Size = new System.Drawing.Size(19, 25);
            this.lblSourceSubscriptionId.TabIndex = 8;
            this.lblSourceSubscriptionId.Text = "-";
            // 
            // lblSourceSubscriptionName
            // 
            this.lblSourceSubscriptionName.AutoSize = true;
            this.lblSourceSubscriptionName.Location = new System.Drawing.Point(224, 90);
            this.lblSourceSubscriptionName.Name = "lblSourceSubscriptionName";
            this.lblSourceSubscriptionName.Size = new System.Drawing.Size(19, 25);
            this.lblSourceSubscriptionName.TabIndex = 7;
            this.lblSourceSubscriptionName.Text = "-";
            // 
            // lblSourceUser
            // 
            this.lblSourceUser.AutoSize = true;
            this.lblSourceUser.Location = new System.Drawing.Point(224, 65);
            this.lblSourceUser.Name = "lblSourceUser";
            this.lblSourceUser.Size = new System.Drawing.Size(19, 25);
            this.lblSourceUser.TabIndex = 6;
            this.lblSourceUser.Text = "-";
            // 
            // lblSourceEnvironment
            // 
            this.lblSourceEnvironment.AutoSize = true;
            this.lblSourceEnvironment.Location = new System.Drawing.Point(224, 38);
            this.lblSourceEnvironment.Name = "lblSourceEnvironment";
            this.lblSourceEnvironment.Size = new System.Drawing.Size(19, 25);
            this.lblSourceEnvironment.TabIndex = 5;
            this.lblSourceEnvironment.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Subscription Id:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(199, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Subscription Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "User:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Environment:";
            // 
            // btnAzureContextASM
            // 
            this.btnAzureContextASM.Location = new System.Drawing.Point(736, 23);
            this.btnAzureContextASM.Name = "btnAzureContextASM";
            this.btnAzureContextASM.Size = new System.Drawing.Size(141, 40);
            this.btnAzureContextASM.TabIndex = 0;
            this.btnAzureContextASM.Text = "Change";
            this.btnAzureContextASM.UseVisualStyleBackColor = true;
            this.btnAzureContextASM.Click += new System.EventHandler(this.btnAzureContextASM_Click);
            // 
            // groupARMSubscription
            // 
            this.groupARMSubscription.Controls.Add(this.lblTargetSubscriptionId);
            this.groupARMSubscription.Controls.Add(this.lblTargetSubscriptionName);
            this.groupARMSubscription.Controls.Add(this.lblTargetUser);
            this.groupARMSubscription.Controls.Add(this.lblTargetEnvironment);
            this.groupARMSubscription.Controls.Add(this.label9);
            this.groupARMSubscription.Controls.Add(this.label10);
            this.groupARMSubscription.Controls.Add(this.label11);
            this.groupARMSubscription.Controls.Add(this.label12);
            this.groupARMSubscription.Controls.Add(this.btnAzureContextARM);
            this.groupARMSubscription.Location = new System.Drawing.Point(943, 25);
            this.groupARMSubscription.Name = "groupARMSubscription";
            this.groupARMSubscription.Size = new System.Drawing.Size(883, 158);
            this.groupARMSubscription.TabIndex = 1;
            this.groupARMSubscription.TabStop = false;
            this.groupARMSubscription.Text = "Target (ARM) Subscription";
            // 
            // lblTargetSubscriptionId
            // 
            this.lblTargetSubscriptionId.AutoSize = true;
            this.lblTargetSubscriptionId.Location = new System.Drawing.Point(231, 117);
            this.lblTargetSubscriptionId.Name = "lblTargetSubscriptionId";
            this.lblTargetSubscriptionId.Size = new System.Drawing.Size(19, 25);
            this.lblTargetSubscriptionId.TabIndex = 16;
            this.lblTargetSubscriptionId.Text = "-";
            // 
            // lblTargetSubscriptionName
            // 
            this.lblTargetSubscriptionName.AutoSize = true;
            this.lblTargetSubscriptionName.Location = new System.Drawing.Point(231, 90);
            this.lblTargetSubscriptionName.Name = "lblTargetSubscriptionName";
            this.lblTargetSubscriptionName.Size = new System.Drawing.Size(19, 25);
            this.lblTargetSubscriptionName.TabIndex = 15;
            this.lblTargetSubscriptionName.Text = "-";
            // 
            // lblTargetUser
            // 
            this.lblTargetUser.AutoSize = true;
            this.lblTargetUser.Location = new System.Drawing.Point(231, 65);
            this.lblTargetUser.Name = "lblTargetUser";
            this.lblTargetUser.Size = new System.Drawing.Size(19, 25);
            this.lblTargetUser.TabIndex = 14;
            this.lblTargetUser.Text = "-";
            // 
            // lblTargetEnvironment
            // 
            this.lblTargetEnvironment.AutoSize = true;
            this.lblTargetEnvironment.Location = new System.Drawing.Point(231, 38);
            this.lblTargetEnvironment.Name = "lblTargetEnvironment";
            this.lblTargetEnvironment.Size = new System.Drawing.Size(19, 25);
            this.lblTargetEnvironment.TabIndex = 13;
            this.lblTargetEnvironment.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 25);
            this.label9.TabIndex = 12;
            this.label9.Text = "Subscription Id:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(199, 25);
            this.label10.TabIndex = 11;
            this.label10.Text = "Subscription Name:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 25);
            this.label11.TabIndex = 10;
            this.label11.Text = "User:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 38);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(138, 25);
            this.label12.TabIndex = 9;
            this.label12.Text = "Environment:";
            // 
            // btnAzureContextARM
            // 
            this.btnAzureContextARM.Enabled = false;
            this.btnAzureContextARM.Location = new System.Drawing.Point(733, 23);
            this.btnAzureContextARM.Name = "btnAzureContextARM";
            this.btnAzureContextARM.Size = new System.Drawing.Size(144, 40);
            this.btnAzureContextARM.TabIndex = 0;
            this.btnAzureContextARM.Text = "Change";
            this.btnAzureContextARM.UseVisualStyleBackColor = true;
            this.btnAzureContextARM.Click += new System.EventHandler(this.btnAzureContextARM_Click);
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
            // AsmToArmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1974, 929);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupASMSubscription);
            this.Controls.Add(this.treeARM);
            this.Controls.Add(this.treeASM);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupARMSubscription);
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
            this.groupASMSubscription.ResumeLayout(false);
            this.groupASMSubscription.PerformLayout();
            this.groupARMSubscription.ResumeLayout(false);
            this.groupARMSubscription.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupASMSubscription;
        private System.Windows.Forms.GroupBox groupARMSubscription;
        private System.Windows.Forms.Button btnAzureContextASM;
        private System.Windows.Forms.Button btnAzureContextARM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSourceSubscriptionId;
        private System.Windows.Forms.Label lblSourceSubscriptionName;
        private System.Windows.Forms.Label lblSourceUser;
        private System.Windows.Forms.Label lblSourceEnvironment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTargetSubscriptionId;
        private System.Windows.Forms.Label lblTargetSubscriptionName;
        private System.Windows.Forms.Label lblTargetUser;
        private System.Windows.Forms.Label lblTargetEnvironment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

