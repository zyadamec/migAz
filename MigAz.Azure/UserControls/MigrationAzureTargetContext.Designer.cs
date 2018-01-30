namespace MigAz.Azure.UserControls
{
    partial class MigrationAzureTargetContext
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
            this.azureLoginContextViewerTarget = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.SuspendLayout();
            // 
            // azureLoginContextViewerTarget
            // 
            this.azureLoginContextViewerTarget.AzureContextSelectedType = MigAz.Azure.UserControls.AzureContextSelectedType.ExistingContext;
            this.azureLoginContextViewerTarget.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.NewOrExistingContext;
            this.azureLoginContextViewerTarget.ExistingContext = null;
            this.azureLoginContextViewerTarget.Location = new System.Drawing.Point(2, 2);
            this.azureLoginContextViewerTarget.Margin = new System.Windows.Forms.Padding(2);
            this.azureLoginContextViewerTarget.Name = "azureLoginContextViewerTarget";
            this.azureLoginContextViewerTarget.Size = new System.Drawing.Size(670, 163);
            this.azureLoginContextViewerTarget.TabIndex = 68;
            this.azureLoginContextViewerTarget.Title = "Azure Subscription (Target)";
            // 
            // MigrationAzureTargetContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.azureLoginContextViewerTarget);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MigrationAzureTargetContext";
            this.Size = new System.Drawing.Size(674, 159);
            this.Resize += new System.EventHandler(this.MigrationTargetAzure_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private AzureLoginContextViewer azureLoginContextViewerTarget;
    }
}
