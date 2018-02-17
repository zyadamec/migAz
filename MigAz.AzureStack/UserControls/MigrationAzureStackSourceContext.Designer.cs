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
            this.treeAzureARM = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // azureStackLoginContextViewer1
            // 
            this.azureStackLoginContextViewer1.AzureContextSelectedType = MigAz.AzureStack.UserControls.AzureContextSelectedType.ExistingContext;
            this.azureStackLoginContextViewer1.ChangeType = MigAz.AzureStack.UserControls.AzureLoginChangeType.NewContext;
            this.azureStackLoginContextViewer1.ExistingContext = null;
            this.azureStackLoginContextViewer1.Location = new System.Drawing.Point(0, 0);
            this.azureStackLoginContextViewer1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.azureStackLoginContextViewer1.Name = "azureStackLoginContextViewer1";
            this.azureStackLoginContextViewer1.Size = new System.Drawing.Size(447, 110);
            this.azureStackLoginContextViewer1.TabIndex = 2;
            this.azureStackLoginContextViewer1.Title = "Azure Stack Subscription";
            // 
            // treeAzureARM
            // 
            this.treeAzureARM.CheckBoxes = true;
            this.treeAzureARM.Location = new System.Drawing.Point(2, 99);
            this.treeAzureARM.Margin = new System.Windows.Forms.Padding(2);
            this.treeAzureARM.Name = "treeAzureARM";
            this.treeAzureARM.Size = new System.Drawing.Size(217, 284);
            this.treeAzureARM.TabIndex = 73;
            // 
            // MigrationAzureStackSourceContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeAzureARM);
            this.Controls.Add(this.azureStackLoginContextViewer1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MigrationAzureStackSourceContext";
            this.Size = new System.Drawing.Size(448, 660);
            this.Resize += new System.EventHandler(this.MigrationAzureStackSourceContext_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private AzureStackLoginContextViewer azureStackLoginContextViewer1;
        private System.Windows.Forms.TreeView treeAzureARM;
    }
}
