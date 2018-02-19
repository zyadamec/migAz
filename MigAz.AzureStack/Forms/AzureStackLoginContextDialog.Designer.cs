namespace MigAz.AzureStack.Forms
{
    partial class AzureStackLoginContextDialog
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
            this.label4 = new System.Windows.Forms.Label();
            this.btnCloseDialog = new System.Windows.Forms.Button();
            this.azureStackArmLoginControl1 = new MigAz.AzureStack.UserControls.AzureStackArmLoginControl();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 12);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(538, 13);
            this.label4.TabIndex = 54;
            this.label4.Text = "The following Azure Stack Subscription context will be utilized to retrieve Azure" +
    " resource information for migration.";
            // 
            // btnCloseDialog
            // 
            this.btnCloseDialog.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCloseDialog.Location = new System.Drawing.Point(483, 175);
            this.btnCloseDialog.Margin = new System.Windows.Forms.Padding(2);
            this.btnCloseDialog.Name = "btnCloseDialog";
            this.btnCloseDialog.Size = new System.Drawing.Size(112, 27);
            this.btnCloseDialog.TabIndex = 1;
            this.btnCloseDialog.Text = "&Close";
            this.btnCloseDialog.UseVisualStyleBackColor = true;
            this.btnCloseDialog.Click += new System.EventHandler(this.btnCloseDialog_Click);
            // 
            // azureStackArmLoginControl1
            // 
            this.azureStackArmLoginControl1.Location = new System.Drawing.Point(9, 27);
            this.azureStackArmLoginControl1.Margin = new System.Windows.Forms.Padding(2);
            this.azureStackArmLoginControl1.Name = "azureStackArmLoginControl1";
            this.azureStackArmLoginControl1.Size = new System.Drawing.Size(586, 144);
            this.azureStackArmLoginControl1.TabIndex = 55;
            // 
            // AzureStackLoginContextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 218);
            this.Controls.Add(this.azureStackArmLoginControl1);
            this.Controls.Add(this.btnCloseDialog);
            this.Controls.Add(this.label4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AzureStackLoginContextDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Azure Stack Authentication";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCloseDialog;
        private UserControls.AzureStackArmLoginControl azureStackArmLoginControl1;
    }
}
