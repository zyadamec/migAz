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
            this.treeViewSourceResourceManager1 = new MigAz.Azure.UserControls.TreeViewSourceResourceManager();
            this.azureStackLoginContextViewer1 = new MigAz.AzureStack.UserControls.AzureStackLoginContextViewer();
            this.SuspendLayout();
            // 
            // treeViewSourceResourceManager1
            // 
            this.treeViewSourceResourceManager1.AutoSelectDependencies = true;
            this.treeViewSourceResourceManager1.DefaultTargetDiskType = MigAz.Core.Interface.ArmDiskType.ManagedDisk;
            this.treeViewSourceResourceManager1.IsSourceContextAuthenticated = false;
            this.treeViewSourceResourceManager1.Location = new System.Drawing.Point(4, 162);
            this.treeViewSourceResourceManager1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewSourceResourceManager1.Name = "treeViewSourceResourceManager1";
            this.treeViewSourceResourceManager1.Size = new System.Drawing.Size(576, 537);
            this.treeViewSourceResourceManager1.TabIndex = 0;
            this.treeViewSourceResourceManager1.AfterNodeChecked += new MigAz.Azure.UserControls.TreeViewSourceResourceManager.AfterNodeCheckedHandler(this.treeViewSourceResourceManager1_AfterNodeChecked);
            this.treeViewSourceResourceManager1.AfterNodeUnchecked += new MigAz.Azure.UserControls.TreeViewSourceResourceManager.AfterNodeUncheckedHandler(this.treeViewSourceResourceManager1_AfterNodeUnchecked);
            this.treeViewSourceResourceManager1.AfterNodeChanged += new MigAz.Azure.UserControls.TreeViewSourceResourceManager.AfterNodeChangedHandler(this.treeViewSourceResourceManager1_AfterNodeChecked);
            this.treeViewSourceResourceManager1.ClearContext += new MigAz.Azure.UserControls.TreeViewSourceResourceManager.ClearContextHandler(this.treeViewSourceResourceManager1_ClearContext);
            this.treeViewSourceResourceManager1.AfterContextChanged += new MigAz.Azure.UserControls.TreeViewSourceResourceManager.AfterContextChangedHandler(this.treeViewSourceResourceManager1_AfterContextChanged);
            // 
            // azureStackLoginContextViewer1
            // 
            this.azureStackLoginContextViewer1.AzureContextSelectedType = MigAz.AzureStack.UserControls.AzureContextSelectedType.ExistingContext;
            this.azureStackLoginContextViewer1.ChangeType = MigAz.AzureStack.UserControls.AzureLoginChangeType.NewContext;
            this.azureStackLoginContextViewer1.ExistingContext = null;
            this.azureStackLoginContextViewer1.Location = new System.Drawing.Point(4, 2);
            this.azureStackLoginContextViewer1.Margin = new System.Windows.Forms.Padding(2);
            this.azureStackLoginContextViewer1.Name = "azureStackLoginContextViewer1";
            this.azureStackLoginContextViewer1.Size = new System.Drawing.Size(670, 169);
            this.azureStackLoginContextViewer1.TabIndex = 1;
            this.azureStackLoginContextViewer1.Title = "Azure Subscription";
            // 
            // MigrationAzureStackSourceContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.azureStackLoginContextViewer1);
            this.Controls.Add(this.treeViewSourceResourceManager1);
            this.Name = "MigrationAzureStackSourceContext";
            this.Size = new System.Drawing.Size(672, 1015);
            this.Resize += new System.EventHandler(this.MigrationAzureStackSourceContext_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private Azure.UserControls.TreeViewSourceResourceManager treeViewSourceResourceManager1;
        private AzureStackLoginContextViewer azureStackLoginContextViewer1;
    }
}
