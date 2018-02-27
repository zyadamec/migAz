// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
            this.label4 = new System.Windows.Forms.Label();
            this.lblSameEnviroronment = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbExistingContext = new System.Windows.Forms.RadioButton();
            this.rbSameUserDifferentSubscription = new System.Windows.Forms.RadioButton();
            this.cboTenant = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSameUsername2 = new System.Windows.Forms.Label();
            this.lblSameEnvironment2 = new System.Windows.Forms.Label();
            this.cboSubscription = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.rbNewContext = new System.Windows.Forms.RadioButton();
            this.lblSameUsername = new System.Windows.Forms.Label();
            this.lblSameSubscription = new System.Windows.Forms.Label();
            this.lblSameTenant = new System.Windows.Forms.Label();
            this.azureArmLoginControl1 = new MigAz.Azure.UserControls.AzureArmLoginControl();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 138);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 76;
            this.label4.Text = "Tenant:";
            // 
            // lblSameEnviroronment
            // 
            this.lblSameEnviroronment.AutoSize = true;
            this.lblSameEnviroronment.Location = new System.Drawing.Point(229, 74);
            this.lblSameEnviroronment.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSameEnviroronment.Name = "lblSameEnviroronment";
            this.lblSameEnviroronment.Size = new System.Drawing.Size(14, 20);
            this.lblSameEnviroronment.TabIndex = 73;
            this.lblSameEnviroronment.Text = "-";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Location = new System.Drawing.Point(771, 699);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(160, 38);
            this.btnClose.TabIndex = 69;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 173);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 20);
            this.label3.TabIndex = 72;
            this.label3.Text = "Subscription:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 71;
            this.label2.Text = "Azure AD User:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 20);
            this.label1.TabIndex = 70;
            this.label1.Text = "Azure Environment:";
            // 
            // rbExistingContext
            // 
            this.rbExistingContext.AutoSize = true;
            this.rbExistingContext.Location = new System.Drawing.Point(33, 35);
            this.rbExistingContext.Name = "rbExistingContext";
            this.rbExistingContext.Size = new System.Drawing.Size(227, 24);
            this.rbExistingContext.TabIndex = 77;
            this.rbExistingContext.Text = "Same Subscription Context";
            this.rbExistingContext.UseVisualStyleBackColor = true;
            this.rbExistingContext.CheckedChanged += new System.EventHandler(this.rbExistingContext_CheckedChanged);
            // 
            // rbSameUserDifferentSubscription
            // 
            this.rbSameUserDifferentSubscription.AutoSize = true;
            this.rbSameUserDifferentSubscription.Location = new System.Drawing.Point(33, 229);
            this.rbSameUserDifferentSubscription.Name = "rbSameUserDifferentSubscription";
            this.rbSameUserDifferentSubscription.Size = new System.Drawing.Size(398, 24);
            this.rbSameUserDifferentSubscription.TabIndex = 86;
            this.rbSameUserDifferentSubscription.Text = "Same User Context / Different Subscription Context";
            this.rbSameUserDifferentSubscription.UseVisualStyleBackColor = true;
            this.rbSameUserDifferentSubscription.CheckedChanged += new System.EventHandler(this.rbSameUserDifferentSubscription_CheckedChanged);
            // 
            // cboTenant
            // 
            this.cboTenant.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTenant.Enabled = false;
            this.cboTenant.FormattingEnabled = true;
            this.cboTenant.Location = new System.Drawing.Point(229, 327);
            this.cboTenant.Margin = new System.Windows.Forms.Padding(2);
            this.cboTenant.Name = "cboTenant";
            this.cboTenant.Size = new System.Drawing.Size(687, 28);
            this.cboTenant.Sorted = true;
            this.cboTenant.TabIndex = 84;
            this.cboTenant.SelectedIndexChanged += new System.EventHandler(this.cboTenant_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 332);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 20);
            this.label5.TabIndex = 85;
            this.label5.Text = "Tenant:";
            // 
            // lblSameUsername2
            // 
            this.lblSameUsername2.AutoSize = true;
            this.lblSameUsername2.Location = new System.Drawing.Point(229, 300);
            this.lblSameUsername2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSameUsername2.Name = "lblSameUsername2";
            this.lblSameUsername2.Size = new System.Drawing.Size(14, 20);
            this.lblSameUsername2.TabIndex = 83;
            this.lblSameUsername2.Text = "-";
            // 
            // lblSameEnvironment2
            // 
            this.lblSameEnvironment2.AutoSize = true;
            this.lblSameEnvironment2.Location = new System.Drawing.Point(229, 268);
            this.lblSameEnvironment2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSameEnvironment2.Name = "lblSameEnvironment2";
            this.lblSameEnvironment2.Size = new System.Drawing.Size(14, 20);
            this.lblSameEnvironment2.TabIndex = 82;
            this.lblSameEnvironment2.Text = "-";
            // 
            // cboSubscription
            // 
            this.cboSubscription.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubscription.Enabled = false;
            this.cboSubscription.FormattingEnabled = true;
            this.cboSubscription.Location = new System.Drawing.Point(229, 365);
            this.cboSubscription.Margin = new System.Windows.Forms.Padding(2);
            this.cboSubscription.Name = "cboSubscription";
            this.cboSubscription.Size = new System.Drawing.Size(687, 28);
            this.cboSubscription.Sorted = true;
            this.cboSubscription.TabIndex = 78;
            this.cboSubscription.SelectedIndexChanged += new System.EventHandler(this.cboSubscription_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(60, 367);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(101, 20);
            this.label8.TabIndex = 81;
            this.label8.Text = "Subscription:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(60, 299);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 20);
            this.label9.TabIndex = 80;
            this.label9.Text = "Azure AD User:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(60, 268);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(148, 20);
            this.label10.TabIndex = 79;
            this.label10.Text = "Azure Environment:";
            // 
            // rbNewContext
            // 
            this.rbNewContext.AutoSize = true;
            this.rbNewContext.Location = new System.Drawing.Point(33, 420);
            this.rbNewContext.Name = "rbNewContext";
            this.rbNewContext.Size = new System.Drawing.Size(316, 24);
            this.rbNewContext.TabIndex = 95;
            this.rbNewContext.Text = "Different User and Subscription Context";
            this.rbNewContext.UseVisualStyleBackColor = true;
            this.rbNewContext.CheckedChanged += new System.EventHandler(this.rbNewContext_CheckedChanged);
            // 
            // lblSameUsername
            // 
            this.lblSameUsername.AutoSize = true;
            this.lblSameUsername.Location = new System.Drawing.Point(229, 106);
            this.lblSameUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSameUsername.Name = "lblSameUsername";
            this.lblSameUsername.Size = new System.Drawing.Size(14, 20);
            this.lblSameUsername.TabIndex = 74;
            this.lblSameUsername.Text = "-";
            // 
            // lblSameSubscription
            // 
            this.lblSameSubscription.AutoSize = true;
            this.lblSameSubscription.Location = new System.Drawing.Point(229, 170);
            this.lblSameSubscription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSameSubscription.Name = "lblSameSubscription";
            this.lblSameSubscription.Size = new System.Drawing.Size(14, 20);
            this.lblSameSubscription.TabIndex = 98;
            this.lblSameSubscription.Text = "-";
            // 
            // lblSameTenant
            // 
            this.lblSameTenant.AutoSize = true;
            this.lblSameTenant.Location = new System.Drawing.Point(229, 138);
            this.lblSameTenant.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSameTenant.Name = "lblSameTenant";
            this.lblSameTenant.Size = new System.Drawing.Size(14, 20);
            this.lblSameTenant.TabIndex = 97;
            this.lblSameTenant.Text = "-";
            // 
            // azureArmLoginControl1
            // 
            this.azureArmLoginControl1.Enabled = false;
            this.azureArmLoginControl1.Location = new System.Drawing.Point(53, 458);
            this.azureArmLoginControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.azureArmLoginControl1.Name = "azureArmLoginControl1";
            this.azureArmLoginControl1.Size = new System.Drawing.Size(878, 214);
            this.azureArmLoginControl1.TabIndex = 96;
            // 
            // AzureNewOrExistingLoginContextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 760);
            this.Controls.Add(this.lblSameSubscription);
            this.Controls.Add(this.lblSameTenant);
            this.Controls.Add(this.azureArmLoginControl1);
            this.Controls.Add(this.rbNewContext);
            this.Controls.Add(this.rbSameUserDifferentSubscription);
            this.Controls.Add(this.cboTenant);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblSameUsername2);
            this.Controls.Add(this.lblSameEnvironment2);
            this.Controls.Add(this.cboSubscription);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.rbExistingContext);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSameUsername);
            this.Controls.Add(this.lblSameEnviroronment);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AzureNewOrExistingLoginContextDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MigAz Target Azure Subscription Context";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AzureNewOrExistingLoginContextDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSameEnviroronment;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbExistingContext;
        private System.Windows.Forms.ComboBox cboTenant;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSameUsername2;
        private System.Windows.Forms.Label lblSameEnvironment2;
        private System.Windows.Forms.ComboBox cboSubscription;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton rbNewContext;
        private System.Windows.Forms.Label lblSameUsername;
        private UserControls.AzureArmLoginControl azureArmLoginControl1;
        private System.Windows.Forms.Label lblSameSubscription;
        private System.Windows.Forms.Label lblSameTenant;
        private System.Windows.Forms.RadioButton rbSameUserDifferentSubscription;
    }
}

