namespace MigAz.Azure.Forms
{
    partial class AzureNewOrExistingLoginContextDialog
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
            this.cboTenant = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblAzureUsername = new System.Windows.Forms.Label();
            this.lblAzureEnvironment = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmbSubscriptions = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // cboTenant
            // 
            this.cboTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenant.Enabled = false;
            this.cboTenant.FormattingEnabled = true;
            this.cboTenant.Location = new System.Drawing.Point(240, 133);
            this.cboTenant.Margin = new System.Windows.Forms.Padding(2);
            this.cboTenant.Name = "cboTenant";
            this.cboTenant.Size = new System.Drawing.Size(687, 28);
            this.cboTenant.Sorted = true;
            this.cboTenant.TabIndex = 75;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(71, 138);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 76;
            this.label4.Text = "Tenant:";
            // 
            // lblAzureUsername
            // 
            this.lblAzureUsername.AutoSize = true;
            this.lblAzureUsername.Location = new System.Drawing.Point(240, 106);
            this.lblAzureUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAzureUsername.Name = "lblAzureUsername";
            this.lblAzureUsername.Size = new System.Drawing.Size(14, 20);
            this.lblAzureUsername.TabIndex = 74;
            this.lblAzureUsername.Text = "-";
            // 
            // lblAzureEnvironment
            // 
            this.lblAzureEnvironment.AutoSize = true;
            this.lblAzureEnvironment.Location = new System.Drawing.Point(240, 74);
            this.lblAzureEnvironment.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAzureEnvironment.Name = "lblAzureEnvironment";
            this.lblAzureEnvironment.Size = new System.Drawing.Size(14, 20);
            this.lblAzureEnvironment.TabIndex = 73;
            this.lblAzureEnvironment.Text = "-";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(767, 357);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(160, 38);
            this.btnClose.TabIndex = 69;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // cmbSubscriptions
            // 
            this.cmbSubscriptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubscriptions.Enabled = false;
            this.cmbSubscriptions.FormattingEnabled = true;
            this.cmbSubscriptions.Location = new System.Drawing.Point(240, 171);
            this.cmbSubscriptions.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSubscriptions.Name = "cmbSubscriptions";
            this.cmbSubscriptions.Size = new System.Drawing.Size(687, 28);
            this.cmbSubscriptions.Sorted = true;
            this.cmbSubscriptions.TabIndex = 68;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 173);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 20);
            this.label3.TabIndex = 72;
            this.label3.Text = "Subscription:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 71;
            this.label2.Text = "Azure AD User:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(71, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 20);
            this.label1.TabIndex = 70;
            this.label1.Text = "Azure Environment:";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(33, 35);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(294, 24);
            this.radioButton1.TabIndex = 77;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Subscription available to current user";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // AzureNewOrExistingLoginContextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 406);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.cboTenant);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblAzureUsername);
            this.Controls.Add(this.lblAzureEnvironment);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AzureNewOrExistingLoginContextDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AzureNewOrExistingLoginContextDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboTenant;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblAzureUsername;
        private System.Windows.Forms.Label lblAzureEnvironment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}