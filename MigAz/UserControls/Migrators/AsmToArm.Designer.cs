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
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.azureLoginContextViewerARM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.treeARM = new System.Windows.Forms.TreeView();
            this.treeASM = new System.Windows.Forms.TreeView();
            this.azureLoginContextViewerASM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.SuspendLayout();
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
            // azureLoginContextViewerARM
            // 
            this.azureLoginContextViewerARM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.SubscriptionOnly;
            this.azureLoginContextViewerARM.Enabled = false;
            this.azureLoginContextViewerARM.Location = new System.Drawing.Point(903, 3);
            this.azureLoginContextViewerARM.Name = "azureLoginContextViewerARM";
            this.azureLoginContextViewerARM.Size = new System.Drawing.Size(894, 204);
            this.azureLoginContextViewerARM.TabIndex = 63;
            this.azureLoginContextViewerARM.Title = "Azure ARM (Target) Subscription";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(898, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(250, 25);
            this.label6.TabIndex = 62;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(255, 25);
            this.label5.TabIndex = 61;
            this.label5.Text = "Source (ASM) Resources";
            // 
            // treeARM
            // 
            this.treeARM.Location = new System.Drawing.Point(905, 241);
            this.treeARM.Name = "treeARM";
            this.treeARM.Size = new System.Drawing.Size(855, 545);
            this.treeARM.TabIndex = 57;
            this.treeARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeARM_AfterSelect);
            // 
            // treeASM
            // 
            this.treeASM.CheckBoxes = true;
            this.treeASM.Location = new System.Drawing.Point(9, 241);
            this.treeASM.Name = "treeASM";
            this.treeASM.Size = new System.Drawing.Size(842, 545);
            this.treeASM.TabIndex = 56;
            this.treeASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeASM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // azureLoginContextViewerASM
            // 
            this.azureLoginContextViewerASM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewerASM.Location = new System.Drawing.Point(3, 3);
            this.azureLoginContextViewerASM.Name = "azureLoginContextViewerASM";
            this.azureLoginContextViewerASM.Size = new System.Drawing.Size(894, 211);
            this.azureLoginContextViewerASM.TabIndex = 64;
            this.azureLoginContextViewerASM.Title = "Azure Subscription";
            // 
            // AsmToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.azureLoginContextViewerARM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.treeARM);
            this.Controls.Add(this.treeASM);
            this.Controls.Add(this.azureLoginContextViewerASM);
            this.Name = "AsmToArm";
            this.Size = new System.Drawing.Size(1807, 803);
            this.Load += new System.EventHandler(this.AsmToArmForm_Load);
            this.Resize += new System.EventHandler(this.AsmToArmForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerARM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView treeARM;
        private System.Windows.Forms.TreeView treeASM;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerASM;
    }
}
