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
            this.treeTargetARM = new System.Windows.Forms.TreeView();
            this.treeSourceASM = new System.Windows.Forms.TreeView();
            this.azureLoginContextViewerASM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.tabSourceResources = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeSourceARM = new System.Windows.Forms.TreeView();
            this.tabSourceResources.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            // treeTargetARM
            // 
            this.treeTargetARM.ImageIndex = 0;
            this.treeTargetARM.ImageList = this.imageList1;
            this.treeTargetARM.Location = new System.Drawing.Point(905, 241);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.SelectedImageIndex = 0;
            this.treeTargetARM.Size = new System.Drawing.Size(855, 545);
            this.treeTargetARM.TabIndex = 57;
            this.treeTargetARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeARM_AfterSelect);
            // 
            // treeSourceASM
            // 
            this.treeSourceASM.CheckBoxes = true;
            this.treeSourceASM.Location = new System.Drawing.Point(0, 0);
            this.treeSourceASM.Name = "treeSourceASM";
            this.treeSourceASM.Size = new System.Drawing.Size(842, 500);
            this.treeSourceASM.TabIndex = 56;
            this.treeSourceASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeSourceASM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
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
            // tabSourceResources
            // 
            this.tabSourceResources.Controls.Add(this.tabPage1);
            this.tabSourceResources.Controls.Add(this.tabPage2);
            this.tabSourceResources.HotTrack = true;
            this.tabSourceResources.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tabSourceResources.Location = new System.Drawing.Point(3, 210);
            this.tabSourceResources.Name = "tabSourceResources";
            this.tabSourceResources.SelectedIndex = 0;
            this.tabSourceResources.Size = new System.Drawing.Size(889, 576);
            this.tabSourceResources.TabIndex = 65;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeSourceASM);
            this.tabPage1.Location = new System.Drawing.Point(8, 39);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(873, 529);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source ASM Resources";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeSourceARM);
            this.tabPage2.Location = new System.Drawing.Point(8, 39);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(873, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Source ARM Resources";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeSourceARM
            // 
            this.treeSourceARM.CheckBoxes = true;
            this.treeSourceARM.Location = new System.Drawing.Point(0, 0);
            this.treeSourceARM.Name = "treeSourceARM";
            this.treeSourceARM.Size = new System.Drawing.Size(842, 500);
            this.treeSourceARM.TabIndex = 57;
            this.treeSourceARM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeSourceARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // AsmToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabSourceResources);
            this.Controls.Add(this.azureLoginContextViewerARM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.treeTargetARM);
            this.Controls.Add(this.azureLoginContextViewerASM);
            this.Name = "AsmToArm";
            this.Size = new System.Drawing.Size(1807, 803);
            this.Load += new System.EventHandler(this.AsmToArmForm_Load);
            this.Resize += new System.EventHandler(this.AsmToArmForm_Resize);
            this.tabSourceResources.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerARM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TreeView treeTargetARM;
        private System.Windows.Forms.TreeView treeSourceASM;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerASM;
        private System.Windows.Forms.TabControl tabSourceResources;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView treeSourceARM;
    }
}
