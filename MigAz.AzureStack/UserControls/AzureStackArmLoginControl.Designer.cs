namespace MigAz.AzureStack.UserControls
{
    partial class AzureStackArmLoginControl
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
            this.lblAuthenticatedUser = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSubscriptions = new System.Windows.Forms.Label();
            this.cmbSubscriptions = new System.Windows.Forms.ComboBox();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTenant = new System.Windows.Forms.ComboBox();
            this.txtAzureStackEnvironment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAuthenticatedUser
            // 
            this.lblAuthenticatedUser.AutoSize = true;
            this.lblAuthenticatedUser.Location = new System.Drawing.Point(118, 40);
            this.lblAuthenticatedUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAuthenticatedUser.Name = "lblAuthenticatedUser";
            this.lblAuthenticatedUser.Size = new System.Drawing.Size(105, 13);
            this.lblAuthenticatedUser.TabIndex = 60;
            this.lblAuthenticatedUser.Text = "<Not Authenticated>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 59;
            this.label2.Text = "Azure AD User:";
            // 
            // lblSubscriptions
            // 
            this.lblSubscriptions.AutoSize = true;
            this.lblSubscriptions.Location = new System.Drawing.Point(6, 116);
            this.lblSubscriptions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubscriptions.Name = "lblSubscriptions";
            this.lblSubscriptions.Size = new System.Drawing.Size(68, 13);
            this.lblSubscriptions.TabIndex = 56;
            this.lblSubscriptions.Text = "Subscription:";
            // 
            // cmbSubscriptions
            // 
            this.cmbSubscriptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubscriptions.Enabled = false;
            this.cmbSubscriptions.FormattingEnabled = true;
            this.cmbSubscriptions.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbSubscriptions.Location = new System.Drawing.Point(120, 113);
            this.cmbSubscriptions.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cmbSubscriptions.MaxDropDownItems = 15;
            this.cmbSubscriptions.Name = "cmbSubscriptions";
            this.cmbSubscriptions.Size = new System.Drawing.Size(462, 21);
            this.cmbSubscriptions.Sorted = true;
            this.cmbSubscriptions.TabIndex = 2;
            this.cmbSubscriptions.SelectedIndexChanged += new System.EventHandler(this.cmbSubscriptions_SelectedIndexChanged);
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(120, 56);
            this.btnAuthenticate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(101, 23);
            this.btnAuthenticate.TabIndex = 1;
            this.btnAuthenticate.Text = "Sign In";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 89);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "Tenant:";
            // 
            // cboTenant
            // 
            this.cboTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenant.Enabled = false;
            this.cboTenant.FormattingEnabled = true;
            this.cboTenant.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cboTenant.Location = new System.Drawing.Point(120, 85);
            this.cboTenant.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboTenant.MaxDropDownItems = 15;
            this.cboTenant.Name = "cboTenant";
            this.cboTenant.Size = new System.Drawing.Size(462, 21);
            this.cboTenant.Sorted = true;
            this.cboTenant.TabIndex = 61;
            this.cboTenant.SelectedIndexChanged += new System.EventHandler(this.cboTenant_SelectedIndexChanged);
            // 
            // txtAzureStackEnvironment
            // 
            this.txtAzureStackEnvironment.Location = new System.Drawing.Point(121, 3);
            this.txtAzureStackEnvironment.Name = "txtAzureStackEnvironment";
            this.txtAzureStackEnvironment.Size = new System.Drawing.Size(462, 20);
            this.txtAzureStackEnvironment.TabIndex = 64;
            this.txtAzureStackEnvironment.Text = "https://adminmanagement.local.azurestack.external";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 65;
            this.label1.Text = "Azure Stack:";
            // 
            // AzureStackArmLoginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAzureStackEnvironment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTenant);
            this.Controls.Add(this.lblAuthenticatedUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblSubscriptions);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.btnAuthenticate);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AzureStackArmLoginControl";
            this.Size = new System.Drawing.Size(586, 145);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAuthenticatedUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSubscriptions;
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Button btnAuthenticate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTenant;
        private System.Windows.Forms.TextBox txtAzureStackEnvironment;
        private System.Windows.Forms.Label label1;
    }
}
