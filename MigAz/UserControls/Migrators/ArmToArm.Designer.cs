namespace MigAz.UserControls.Migrators
{
    partial class ArmToArm
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
            this.treeSource = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.azureLoginContextViewer1 = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.label6 = new System.Windows.Forms.Label();
            this.treeARM = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeSource
            // 
            this.treeSource.CheckBoxes = true;
            this.treeSource.Location = new System.Drawing.Point(3, 249);
            this.treeSource.Name = "treeSource";
            this.treeSource.Size = new System.Drawing.Size(700, 548);
            this.treeSource.TabIndex = 48;
            this.treeSource.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeSource_BeforeCheck);
            this.treeSource.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeSource_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 221);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 25);
            this.label1.TabIndex = 47;
            this.label1.Text = "Source (ARM) Resources";
            // 
            // azureLoginContextViewer1
            // 
            this.azureLoginContextViewer1.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewer1.Location = new System.Drawing.Point(3, 3);
            this.azureLoginContextViewer1.Name = "azureLoginContextViewer1";
            this.azureLoginContextViewer1.Size = new System.Drawing.Size(894, 211);
            this.azureLoginContextViewer1.TabIndex = 49;
            this.azureLoginContextViewer1.Title = "ARM (Source) Subscription";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(726, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(250, 25);
            this.label6.TabIndex = 64;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // treeARM
            // 
            this.treeARM.Location = new System.Drawing.Point(733, 252);
            this.treeARM.Name = "treeARM";
            this.treeARM.Size = new System.Drawing.Size(855, 545);
            this.treeARM.TabIndex = 63;
            // 
            // ArmToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.treeARM);
            this.Controls.Add(this.treeSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.azureLoginContextViewer1);
            this.Name = "ArmToArm";
            this.Size = new System.Drawing.Size(1666, 806);
            this.Load += new System.EventHandler(this.ArmToArm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeSource;
        private System.Windows.Forms.Label label1;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewer1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TreeView treeARM;
    }
}
