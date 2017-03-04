namespace MigAz.Forms.ARM
{
    partial class Window
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.btnExport = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnOptions = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.treeSource = new System.Windows.Forms.TreeView();
            this.azureLoginContextViewer1 = new MigAz.Azure.Arm.UserControls.AzureLoginContextViewer2();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(1232, 845);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(656, 44);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(1415, 901);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(6);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(258, 44);
            this.btnOptions.TabIndex = 34;
            this.btnOptions.Text = "Options...";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 230);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 25);
            this.label1.TabIndex = 36;
            this.label1.Text = "Virtual Networks";
            // 
            // treeSource
            // 
            this.treeSource.Location = new System.Drawing.Point(18, 258);
            this.treeSource.Name = "treeSource";
            this.treeSource.Size = new System.Drawing.Size(700, 455);
            this.treeSource.TabIndex = 43;
            this.treeSource.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeSource_AfterSelect);
            // 
            // azureLoginContextViewer1
            // 
            this.azureLoginContextViewer1.ChangeType = MigAz.Azure.Arm.UserControls.ChangeType.Full;
            this.azureLoginContextViewer1.Location = new System.Drawing.Point(18, 12);
            this.azureLoginContextViewer1.Name = "azureLoginContextViewer1";
            this.azureLoginContextViewer1.Size = new System.Drawing.Size(894, 211);
            this.azureLoginContextViewer1.TabIndex = 44;
            this.azureLoginContextViewer1.Title = "Azure Subscription";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1912, 1097);
            this.Controls.Add(this.treeSource);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.azureLoginContextViewer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Window";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "migAz";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
            this.Load += new System.EventHandler(this.Window_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeSource;
        private Azure.Arm.UserControls.AzureLoginContextViewer2 azureLoginContextViewer1;
    }
}

