namespace MigAz.Azure.UserControls
{
    partial class TargetTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TargetTreeView));
            this.treeTargetARM = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.ImageIndex = 0;
            this.treeTargetARM.ImageList = this.imageList1;
            this.treeTargetARM.Location = new System.Drawing.Point(2, 2);
            this.treeTargetARM.Margin = new System.Windows.Forms.Padding(2);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.SelectedImageIndex = 0;
            this.treeTargetARM.Size = new System.Drawing.Size(87, 85);
            this.treeTargetARM.TabIndex = 58;
            this.treeTargetARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TargetTreeView_AfterSelect);
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
            // TargetTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Name = "TargetTreeView";
            this.Size = new System.Drawing.Size(132, 123);
            this.Resize += new System.EventHandler(this.TargetTreeView_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeTargetARM;
        private System.Windows.Forms.ImageList imageList1;
    }
}
