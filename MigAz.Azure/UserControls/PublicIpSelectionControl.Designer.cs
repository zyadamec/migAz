// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class PublicIpSelectionControl
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
            this.label7 = new System.Windows.Forms.Label();
            this.rbExistingARMPublicIp = new System.Windows.Forms.RadioButton();
            this.rbPublicIPInMigration = new System.Windows.Forms.RadioButton();
            this.cmbPublicIp = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 17);
            this.label7.TabIndex = 35;
            this.label7.Text = "Target Public IP:";
            // 
            // rbExistingARMPublicIp
            // 
            this.rbExistingARMPublicIp.AutoSize = true;
            this.rbExistingARMPublicIp.Enabled = false;
            this.rbExistingARMPublicIp.Location = new System.Drawing.Point(116, 30);
            this.rbExistingARMPublicIp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbExistingARMPublicIp.Name = "rbExistingARMPublicIp";
            this.rbExistingARMPublicIp.Size = new System.Drawing.Size(208, 21);
            this.rbExistingARMPublicIp.TabIndex = 31;
            this.rbExistingARMPublicIp.Text = "Existing Public IP in Location";
            this.rbExistingARMPublicIp.UseVisualStyleBackColor = true;
            this.rbExistingARMPublicIp.CheckedChanged += new System.EventHandler(this.rbExistingARMPublicIP_CheckedChanged);
            // 
            // rbPublicIPInMigration
            // 
            this.rbPublicIPInMigration.AutoSize = true;
            this.rbPublicIPInMigration.Location = new System.Drawing.Point(116, 2);
            this.rbPublicIPInMigration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rbPublicIPInMigration.Name = "rbPublicIPInMigration";
            this.rbPublicIPInMigration.Size = new System.Drawing.Size(202, 21);
            this.rbPublicIPInMigration.TabIndex = 30;
            this.rbPublicIPInMigration.Text = "Public IP in MigAz Migration";
            this.rbPublicIPInMigration.UseVisualStyleBackColor = true;
            this.rbPublicIPInMigration.CheckedChanged += new System.EventHandler(this.rbPublicIPInMigration_CheckedChanged);
            // 
            // cmbPublicIp
            // 
            this.cmbPublicIp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPublicIp.FormattingEnabled = true;
            this.cmbPublicIp.Location = new System.Drawing.Point(116, 64);
            this.cmbPublicIp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPublicIp.Name = "cmbPublicIp";
            this.cmbPublicIp.Size = new System.Drawing.Size(287, 24);
            this.cmbPublicIp.TabIndex = 32;
            this.cmbPublicIp.SelectedIndexChanged += new System.EventHandler(this.cmbPublicIp_SelectedIndexChanged);
            // 
            // PublicIpSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.rbExistingARMPublicIp);
            this.Controls.Add(this.rbPublicIPInMigration);
            this.Controls.Add(this.cmbPublicIp);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PublicIpSelectionControl";
            this.Size = new System.Drawing.Size(413, 94);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rbExistingARMPublicIp;
        private System.Windows.Forms.RadioButton rbPublicIPInMigration;
        private System.Windows.Forms.ComboBox cmbPublicIp;
    }
}

