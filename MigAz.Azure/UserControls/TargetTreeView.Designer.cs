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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNewAvailabilitySet = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblAddNew = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.Enabled = false;
            this.treeTargetARM.Location = new System.Drawing.Point(5, 45);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.Size = new System.Drawing.Size(128, 129);
            this.treeTargetARM.TabIndex = 58;
            this.treeTargetARM.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TargetTreeView_AfterSelect);
            this.treeTargetARM.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeTargetARM_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 20);
            this.label1.TabIndex = 59;
            this.label1.Text = "Target Resource(s):";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNewAvailabilitySet);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(138, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(181, 97);
            this.panel1.TabIndex = 60;
            // 
            // btnNewAvailabilitySet
            // 
            this.btnNewAvailabilitySet.ImageKey = "AvailabilitySet";
            this.btnNewAvailabilitySet.ImageList = this.imageList1;
            this.btnNewAvailabilitySet.Location = new System.Drawing.Point(0, 0);
            this.btnNewAvailabilitySet.Name = "btnNewAvailabilitySet";
            this.btnNewAvailabilitySet.Size = new System.Drawing.Size(178, 39);
            this.btnNewAvailabilitySet.TabIndex = 0;
            this.btnNewAvailabilitySet.Text = "Availability Set";
            this.btnNewAvailabilitySet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewAvailabilitySet.UseVisualStyleBackColor = true;
            this.btnNewAvailabilitySet.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ResourceGroup");
            this.imageList1.Images.SetKeyName(1, "Disk");
            this.imageList1.Images.SetKeyName(2, "LoadBalancer");
            this.imageList1.Images.SetKeyName(3, "NetworkInterface");
            this.imageList1.Images.SetKeyName(4, "NetworkSecurityGroup");
            this.imageList1.Images.SetKeyName(5, "PublicIp");
            this.imageList1.Images.SetKeyName(6, "StorageAccount");
            this.imageList1.Images.SetKeyName(7, "VirtualMachine");
            this.imageList1.Images.SetKeyName(8, "AvailabilitySet");
            this.imageList1.Images.SetKeyName(9, "VirtualNetwork");
            this.imageList1.Images.SetKeyName(10, "RouteTable");
            this.imageList1.Images.SetKeyName(11, "VirtualMachineImage");
            // 
            // lblAddNew
            // 
            this.lblAddNew.AutoSize = true;
            this.lblAddNew.Location = new System.Drawing.Point(139, 45);
            this.lblAddNew.Name = "lblAddNew";
            this.lblAddNew.Size = new System.Drawing.Size(77, 20);
            this.lblAddNew.TabIndex = 61;
            this.lblAddNew.Text = "Add New:";
            // 
            // TargetTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAddNew);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeTargetARM);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TargetTreeView";
            this.Size = new System.Drawing.Size(378, 189);
            this.Resize += new System.EventHandler(this.TargetTreeView_Resize);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeTargetARM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnNewAvailabilitySet;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblAddNew;
    }
}
