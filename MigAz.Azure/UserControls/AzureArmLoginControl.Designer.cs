// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class AzureArmLoginControl
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
            this.cboAzureEnvironment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTenant = new System.Windows.Forms.ComboBox();
            this.ckbIncludePreviewRegions = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblAuthenticatedUser
            // 
            this.lblAuthenticatedUser.AutoSize = true;
            this.lblAuthenticatedUser.Location = new System.Drawing.Point(177, 101);
            this.lblAuthenticatedUser.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAuthenticatedUser.Name = "lblAuthenticatedUser";
            this.lblAuthenticatedUser.Size = new System.Drawing.Size(156, 20);
            this.lblAuthenticatedUser.TabIndex = 60;
            this.lblAuthenticatedUser.Text = "<Not Authenticated>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 59;
            this.label2.Text = "Azure AD User:";
            // 
            // lblSubscriptions
            // 
            this.lblSubscriptions.AutoSize = true;
            this.lblSubscriptions.Location = new System.Drawing.Point(6, 176);
            this.lblSubscriptions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubscriptions.Name = "lblSubscriptions";
            this.lblSubscriptions.Size = new System.Drawing.Size(101, 20);
            this.lblSubscriptions.TabIndex = 56;
            this.lblSubscriptions.Text = "Subscription:";
            // 
            // cmbSubscriptions
            // 
            this.cmbSubscriptions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubscriptions.Enabled = false;
            this.cmbSubscriptions.FormattingEnabled = true;
            this.cmbSubscriptions.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbSubscriptions.Location = new System.Drawing.Point(181, 170);
            this.cmbSubscriptions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSubscriptions.MaxDropDownItems = 15;
            this.cmbSubscriptions.Name = "cmbSubscriptions";
            this.cmbSubscriptions.Size = new System.Drawing.Size(692, 28);
            this.cmbSubscriptions.Sorted = true;
            this.cmbSubscriptions.TabIndex = 2;
            this.cmbSubscriptions.SelectedIndexChanged += new System.EventHandler(this.cmbSubscriptions_SelectedIndexChanged);
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.Location = new System.Drawing.Point(181, 52);
            this.btnAuthenticate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(152, 37);
            this.btnAuthenticate.TabIndex = 1;
            this.btnAuthenticate.Text = "Sign In";
            this.btnAuthenticate.UseVisualStyleBackColor = true;
            this.btnAuthenticate.Click += new System.EventHandler(this.btnAuthenticate_Click);
            // 
            // cboAzureEnvironment
            // 
            this.cboAzureEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAzureEnvironment.FormattingEnabled = true;
            this.cboAzureEnvironment.Items.AddRange(new object[] {
            "AzureCloud",
            "AzureChinaCloud",
            "AzureGermanCloud",
            "AzureUSGovernment"});
            this.cboAzureEnvironment.Location = new System.Drawing.Point(181, 10);
            this.cboAzureEnvironment.Name = "cboAzureEnvironment";
            this.cboAzureEnvironment.Size = new System.Drawing.Size(243, 28);
            this.cboAzureEnvironment.TabIndex = 0;
            this.cboAzureEnvironment.SelectedIndexChanged += new System.EventHandler(this.cboAzureEnvironment_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 20);
            this.label1.TabIndex = 57;
            this.label1.Text = "Azure Environment:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 20);
            this.label3.TabIndex = 62;
            this.label3.Text = "Tenant:";
            // 
            // cboTenant
            // 
            this.cboTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenant.Enabled = false;
            this.cboTenant.FormattingEnabled = true;
            this.cboTenant.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cboTenant.Location = new System.Drawing.Point(181, 131);
            this.cboTenant.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboTenant.MaxDropDownItems = 15;
            this.cboTenant.Name = "cboTenant";
            this.cboTenant.Size = new System.Drawing.Size(692, 28);
            this.cboTenant.Sorted = true;
            this.cboTenant.TabIndex = 61;
            this.cboTenant.SelectedIndexChanged += new System.EventHandler(this.cboTenant_SelectedIndexChanged);
            // 
            // ckbIncludePreviewRegions
            // 
            this.ckbIncludePreviewRegions.AutoSize = true;
            this.ckbIncludePreviewRegions.Location = new System.Drawing.Point(525, 12);
            this.ckbIncludePreviewRegions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ckbIncludePreviewRegions.Name = "ckbIncludePreviewRegions";
            this.ckbIncludePreviewRegions.Size = new System.Drawing.Size(264, 24);
            this.ckbIncludePreviewRegions.TabIndex = 63;
            this.ckbIncludePreviewRegions.Text = "Include preview regions (France)";
            this.ckbIncludePreviewRegions.UseVisualStyleBackColor = true;
            this.ckbIncludePreviewRegions.CheckedChanged += new System.EventHandler(this.ckbIncludePreviewRegions_CheckedChanged);
            // 
            // AzureArmLoginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ckbIncludePreviewRegions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTenant);
            this.Controls.Add(this.lblSubscriptions);
            this.Controls.Add(this.cmbSubscriptions);
            this.Controls.Add(this.btnAuthenticate);
            this.Controls.Add(this.cboAzureEnvironment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAuthenticatedUser);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "AzureArmLoginControl";
            this.Size = new System.Drawing.Size(878, 214);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAuthenticatedUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSubscriptions;
        private System.Windows.Forms.ComboBox cmbSubscriptions;
        private System.Windows.Forms.Button btnAuthenticate;
        private System.Windows.Forms.ComboBox cboAzureEnvironment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTenant;
        private System.Windows.Forms.CheckBox ckbIncludePreviewRegions;
    }
}

