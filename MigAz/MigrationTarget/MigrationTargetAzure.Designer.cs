namespace MigAz.MigrationTarget
{
    partial class MigrationTargetAzure
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
            this.label6 = new System.Windows.Forms.Label();
            this.treeTargetARM = new MigAz.Azure.UserControls.TargetTreeView();
            this.azureLoginContextViewerARM = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(-3, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(186, 20);
            this.label6.TabIndex = 67;
            this.label6.Text = "Target (ARM) Resources";
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.ImageList = null;
            this.treeTargetARM.Location = new System.Drawing.Point(2, 200);
            this.treeTargetARM.LogProvider = null;
            this.treeTargetARM.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.PropertyPanel = null;
            this.treeTargetARM.SelectedNode = null;
            this.treeTargetARM.SettingsProvider = null;
            this.treeTargetARM.Size = new System.Drawing.Size(670, 403);
            this.treeTargetARM.StatusProvider = null;
            this.treeTargetARM.TabIndex = 69;
            // 
            // azureLoginContextViewerARM
            // 
            this.azureLoginContextViewerARM.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewerARM.Enabled = false;
            this.azureLoginContextViewerARM.Location = new System.Drawing.Point(2, 2);
            this.azureLoginContextViewerARM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.azureLoginContextViewerARM.Name = "azureLoginContextViewerARM";
            this.azureLoginContextViewerARM.Size = new System.Drawing.Size(670, 163);
            this.azureLoginContextViewerARM.TabIndex = 68;
            this.azureLoginContextViewerARM.Title = "Azure Subscription (Target)";
            // 
            // MigrationTargetAzure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Controls.Add(this.azureLoginContextViewerARM);
            this.Controls.Add(this.label6);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MigrationTargetAzure";
            this.Size = new System.Drawing.Size(674, 602);
            this.Resize += new System.EventHandler(this.MigrationTargetAzure_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Azure.UserControls.TargetTreeView treeTargetARM;
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewerARM;
        private System.Windows.Forms.Label label6;
    }
}
