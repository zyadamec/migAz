namespace MIGAZ.Forms
{
    partial class PreExportDialog
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnChoosePath = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtDestinationFolder = new System.Windows.Forms.TextBox();
            this.lblExportFolder = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // btnChoosePath
            // 
            this.btnChoosePath.Location = new System.Drawing.Point(936, 33);
            this.btnChoosePath.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnChoosePath.Name = "btnChoosePath";
            this.btnChoosePath.Size = new System.Drawing.Size(59, 44);
            this.btnChoosePath.TabIndex = 1;
            this.btnChoosePath.Text = "...";
            this.btnChoosePath.UseVisualStyleBackColor = true;
            this.btnChoosePath.Click += new System.EventHandler(this.btnChoosePath_Click);
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExport.Location = new System.Drawing.Point(258, 100);
            this.btnExport.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(389, 44);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "&Export 0 objects";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtDestinationFolder
            // 
            this.txtDestinationFolder.Location = new System.Drawing.Point(258, 40);
            this.txtDestinationFolder.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtDestinationFolder.Name = "txtDestinationFolder";
            this.txtDestinationFolder.Size = new System.Drawing.Size(668, 31);
            this.txtDestinationFolder.TabIndex = 0;
            // 
            // lblExportFolder
            // 
            this.lblExportFolder.AutoSize = true;
            this.lblExportFolder.Location = new System.Drawing.Point(22, 46);
            this.lblExportFolder.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblExportFolder.Name = "lblExportFolder";
            this.lblExportFolder.Size = new System.Drawing.Size(147, 25);
            this.lblExportFolder.TabIndex = 4;
            this.lblExportFolder.Text = "Export Folder:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(676, 100);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(282, 44);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // PreExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 172);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChoosePath);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.txtDestinationFolder);
            this.Controls.Add(this.lblExportFolder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreExportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export";
            this.Load += new System.EventHandler(this.PreExportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnChoosePath;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TextBox txtDestinationFolder;
        private System.Windows.Forms.Label lblExportFolder;
        private System.Windows.Forms.Button btnCancel;
    }
}