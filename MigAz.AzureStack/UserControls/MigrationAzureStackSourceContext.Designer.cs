namespace MigAz.AzureStack.UserControls
{
    partial class MigrationAzureStackSourceContext
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
            this.azureStackLoginContextViewer1 = new MigAz.AzureStack.UserControls.AzureStackLoginContextViewer();
            this.treeViewSourceResourceManager1 = new MigAz.Azure.UserControls.TreeViewSourceResourceManager();
            this.SuspendLayout();
            // 
            // azureStackLoginContextViewer1
            // 
            this.azureStackLoginContextViewer1.AzureContextSelectedType = MigAz.AzureStack.UserControls.AzureContextSelectedType.ExistingContext;
            this.azureStackLoginContextViewer1.ChangeType = MigAz.AzureStack.UserControls.AzureLoginChangeType.NewContext;
            this.azureStackLoginContextViewer1.ExistingContext = null;
            this.azureStackLoginContextViewer1.Location = new System.Drawing.Point(0, 0);
            this.azureStackLoginContextViewer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.azureStackLoginContextViewer1.Name = "azureStackLoginContextViewer1";
            this.azureStackLoginContextViewer1.Size = new System.Drawing.Size(670, 169);
            this.azureStackLoginContextViewer1.TabIndex = 2;
            this.azureStackLoginContextViewer1.Title = "Azure Stack Subscription";
            // 
            // treeViewSourceResourceManager1
            // 
            this.treeViewSourceResourceManager1.AutoSelectDependencies = true;
            this.treeViewSourceResourceManager1.DefaultTargetDiskType = MigAz.Core.Interface.ArmDiskType.ManagedDisk;
            this.treeViewSourceResourceManager1.IsSourceContextAuthenticated = false;
            this.treeViewSourceResourceManager1.Location = new System.Drawing.Point(4, 164);
            this.treeViewSourceResourceManager1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewSourceResourceManager1.Name = "treeViewSourceResourceManager1";
            this.treeViewSourceResourceManager1.Size = new System.Drawing.Size(380, 315);
            this.treeViewSourceResourceManager1.TabIndex = 3;
            // 
            // MigrationAzureStackSourceContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewSourceResourceManager1);
            this.Controls.Add(this.azureStackLoginContextViewer1);
            this.Name = "MigrationAzureStackSourceContext";
            this.Size = new System.Drawing.Size(672, 1015);
            this.Resize += new System.EventHandler(this.MigrationAzureStackSourceContext_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private AzureStackLoginContextViewer azureStackLoginContextViewer1;
        private Azure.UserControls.TreeViewSourceResourceManager treeViewSourceResourceManager1;
    }
}
