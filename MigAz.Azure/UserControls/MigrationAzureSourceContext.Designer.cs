namespace MigAz.Azure.UserControls
{
    partial class MigrationAzureSourceContext
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
            this.treeAzureASM = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAzureResourceTypeSource = new System.Windows.Forms.ComboBox();
            this.azureLoginContextViewerSource = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.treeViewSourceResourceManager1 = new MigAz.Azure.UserControls.TreeViewSourceResourceManager();
            this.SuspendLayout();
            // 
            // treeAzureASM
            // 
            this.treeAzureASM.CheckBoxes = true;
            this.treeAzureASM.Location = new System.Drawing.Point(3, 202);
            this.treeAzureASM.Name = "treeAzureASM";
            this.treeAzureASM.Size = new System.Drawing.Size(295, 435);
            this.treeAzureASM.TabIndex = 69;
            this.treeAzureASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeAzureResourcesSource_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 166);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 20);
            this.label1.TabIndex = 70;
            this.label1.Text = "Source Resource Type:";
            // 
            // cmbAzureResourceTypeSource
            // 
            this.cmbAzureResourceTypeSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAzureResourceTypeSource.FormattingEnabled = true;
            this.cmbAzureResourceTypeSource.Items.AddRange(new object[] {
            "Azure Resource Manager (ARM)",
            "Azure Service Management (ASM / Classic)"});
            this.cmbAzureResourceTypeSource.Location = new System.Drawing.Point(194, 162);
            this.cmbAzureResourceTypeSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbAzureResourceTypeSource.Name = "cmbAzureResourceTypeSource";
            this.cmbAzureResourceTypeSource.Size = new System.Drawing.Size(352, 28);
            this.cmbAzureResourceTypeSource.TabIndex = 71;
            this.cmbAzureResourceTypeSource.SelectedIndexChanged += new System.EventHandler(this.cmbAzureResourceTypeSource_SelectedIndexChanged);
            // 
            // azureLoginContextViewerSource
            // 
            this.azureLoginContextViewerSource.AzureContextSelectedType = MigAz.Azure.UserControls.AzureContextSelectedType.ExistingContext;
            this.azureLoginContextViewerSource.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.NewContext;
            this.azureLoginContextViewerSource.ExistingContext = null;
            this.azureLoginContextViewerSource.Location = new System.Drawing.Point(0, 2);
            this.azureLoginContextViewerSource.Margin = new System.Windows.Forms.Padding(2);
            this.azureLoginContextViewerSource.Name = "azureLoginContextViewerSource";
            this.azureLoginContextViewerSource.Size = new System.Drawing.Size(670, 169);
            this.azureLoginContextViewerSource.TabIndex = 67;
            this.azureLoginContextViewerSource.Title = "Azure Subscription";
            // 
            // treeViewSourceResourceManager1
            // 
            this.treeViewSourceResourceManager1.AutoSelectDependencies = true;
            this.treeViewSourceResourceManager1.DefaultTargetDiskType = MigAz.Core.Interface.ArmDiskType.ManagedDisk;
            this.treeViewSourceResourceManager1.IsSourceContextAuthenticated = false;
            this.treeViewSourceResourceManager1.Location = new System.Drawing.Point(330, 202);
            this.treeViewSourceResourceManager1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeViewSourceResourceManager1.Name = "treeViewSourceResourceManager1";
            this.treeViewSourceResourceManager1.Size = new System.Drawing.Size(290, 319);
            this.treeViewSourceResourceManager1.TabIndex = 72;
            // 
            // MigrationAzureSourceContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewSourceResourceManager1);
            this.Controls.Add(this.cmbAzureResourceTypeSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeAzureASM);
            this.Controls.Add(this.azureLoginContextViewerSource);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MigrationAzureSourceContext";
            this.Size = new System.Drawing.Size(680, 642);
            this.Resize += new System.EventHandler(this.MigrationSourceAzure_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AzureLoginContextViewer azureLoginContextViewerSource;
        private System.Windows.Forms.TreeView treeAzureASM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAzureResourceTypeSource;
        private TreeViewSourceResourceManager treeViewSourceResourceManager1;
    }
}
