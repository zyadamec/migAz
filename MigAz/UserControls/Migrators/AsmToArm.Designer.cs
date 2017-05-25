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
            this.treeSourceASM = new System.Windows.Forms.TreeView();
            this.azureLoginContextViewerASM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.tabSourceResources = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.treeSourceARM = new System.Windows.Forms.TreeView();
            this.treeTargetARM = new MigAz.Azure.UserControls.TargetTreeView();
            this.tabSourceResources.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Disk");
            this.imageList1.Images.SetKeyName(1, "LoadBalancer");
            this.imageList1.Images.SetKeyName(2, "NetworkInterface");
            this.imageList1.Images.SetKeyName(3, "NetworkSecurityGroup");
            this.imageList1.Images.SetKeyName(4, "PublicIp");
            this.imageList1.Images.SetKeyName(5, "StorageAccount");
            this.imageList1.Images.SetKeyName(6, "VirtualMachine");
            this.imageList1.Images.SetKeyName(7, "VirtualNetwork");
            this.imageList1.Images.SetKeyName(8, "Subscription");
            this.imageList1.Images.SetKeyName(9, "ResourceGroup");
            this.imageList1.Images.SetKeyName(10, "AvailabilitySet");
            // 
            // azureLoginContextViewerARM
            // 
            this.azureLoginContextViewerARM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.SubscriptionOnly;
            this.azureLoginContextViewerARM.Enabled = false;
            this.azureLoginContextViewerARM.Location = new System.Drawing.Point(452, 2);
            this.azureLoginContextViewerARM.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.azureLoginContextViewerARM.Name = "azureLoginContextViewerARM";
            this.azureLoginContextViewerARM.Size = new System.Drawing.Size(447, 106);
            this.azureLoginContextViewerARM.TabIndex = 63;
            this.azureLoginContextViewerARM.Title = "Azure ARM (Target) Subscription";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(449, 109);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 13);
            this.label6.TabIndex = 62;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // treeSourceASM
            // 
            this.treeSourceASM.CheckBoxes = true;
            this.treeSourceASM.Location = new System.Drawing.Point(0, 0);
            this.treeSourceASM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeSourceASM.Name = "treeSourceASM";
            this.treeSourceASM.Size = new System.Drawing.Size(423, 262);
            this.treeSourceASM.TabIndex = 56;
            this.treeSourceASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeSourceASM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // azureLoginContextViewerASM
            // 
            this.azureLoginContextViewerASM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewerASM.Location = new System.Drawing.Point(2, 2);
            this.azureLoginContextViewerASM.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.azureLoginContextViewerASM.Name = "azureLoginContextViewerASM";
            this.azureLoginContextViewerASM.Size = new System.Drawing.Size(447, 110);
            this.azureLoginContextViewerASM.TabIndex = 64;
            this.azureLoginContextViewerASM.Title = "Azure Subscription";
            // 
            // tabSourceResources
            // 
            this.tabSourceResources.Controls.Add(this.tabPage1);
            this.tabSourceResources.Controls.Add(this.tabPage2);
            this.tabSourceResources.HotTrack = true;
            this.tabSourceResources.ImeMode = System.Windows.Forms.ImeMode.On;
            this.tabSourceResources.Location = new System.Drawing.Point(2, 109);
            this.tabSourceResources.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabSourceResources.Name = "tabSourceResources";
            this.tabSourceResources.SelectedIndex = 0;
            this.tabSourceResources.Size = new System.Drawing.Size(444, 300);
            this.tabSourceResources.TabIndex = 65;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeSourceASM);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage1.Size = new System.Drawing.Size(436, 274);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source ASM Resources";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeSourceARM);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage2.Size = new System.Drawing.Size(436, 274);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Source ARM Resources";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // treeSourceARM
            // 
            this.treeSourceARM.CheckBoxes = true;
            this.treeSourceARM.ImageIndex = 0;
            this.treeSourceARM.ImageList = this.imageList1;
            this.treeSourceARM.Location = new System.Drawing.Point(0, 0);
            this.treeSourceARM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeSourceARM.Name = "treeSourceARM";
            this.treeSourceARM.SelectedImageIndex = 0;
            this.treeSourceARM.Size = new System.Drawing.Size(423, 262);
            this.treeSourceARM.TabIndex = 57;
            this.treeSourceARM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterCheck);
            this.treeSourceARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeASM_AfterSelect);
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.Location = new System.Drawing.Point(452, 131);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.Size = new System.Drawing.Size(447, 274);
            this.treeTargetARM.TabIndex = 66;
            // 
            // AsmToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Controls.Add(this.tabSourceResources);
            this.Controls.Add(this.azureLoginContextViewerARM);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.azureLoginContextViewerASM);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AsmToArm";
            this.Size = new System.Drawing.Size(904, 418);
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
        private System.Windows.Forms.TreeView treeSourceASM;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerASM;
        private System.Windows.Forms.TabControl tabSourceResources;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView treeSourceARM;
        private Azure.UserControls.TargetTreeView treeTargetARM;
    }
}
