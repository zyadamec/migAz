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
            this.SuspendLayout();
            // 
            // treeAzureASM
            // 
            this.treeAzureASM.CheckBoxes = true;
            this.treeAzureASM.Location = new System.Drawing.Point(2, 131);
            this.treeAzureASM.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeAzureASM.Name = "treeAzureASM";
            this.treeAzureASM.Size = new System.Drawing.Size(198, 284);
            this.treeAzureASM.TabIndex = 69;
            this.treeAzureASM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeAzureResourcesSource_AfterCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
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
            this.cmbAzureResourceTypeSource.Location = new System.Drawing.Point(129, 105);
            this.cmbAzureResourceTypeSource.Name = "cmbAzureResourceTypeSource";
            this.cmbAzureResourceTypeSource.Size = new System.Drawing.Size(236, 21);
            this.cmbAzureResourceTypeSource.TabIndex = 71;
            this.cmbAzureResourceTypeSource.SelectedIndexChanged += new System.EventHandler(this.cmbAzureResourceTypeSource_SelectedIndexChanged);
            // 
            // azureLoginContextViewerSource
            // 
            this.azureLoginContextViewerSource.ExistingContext = null;
            this.azureLoginContextViewerSource.Location = new System.Drawing.Point(0, 1);
            this.azureLoginContextViewerSource.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.azureLoginContextViewerSource.Name = "azureLoginContextViewerSource";
            this.azureLoginContextViewerSource.Size = new System.Drawing.Size(447, 110);
            this.azureLoginContextViewerSource.TabIndex = 67;
            this.azureLoginContextViewerSource.Title = "Azure Subscription";
            // 
            // MigrationAzureSourceContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbAzureResourceTypeSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeAzureASM);
            this.Controls.Add(this.azureLoginContextViewerSource);
            this.Name = "MigrationAzureSourceContext";
            this.Size = new System.Drawing.Size(453, 417);
            this.Resize += new System.EventHandler(this.MigrationSourceAzure_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AzureLoginContextViewer azureLoginContextViewerSource;
        private System.Windows.Forms.TreeView treeAzureASM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAzureResourceTypeSource;
    }
}
