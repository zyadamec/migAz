// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class PublicIpProperties
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDomainNameLabel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPublicIpAllocation = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(132, 20);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(247, 26);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Domain Name Label:";
            // 
            // txtDomainNameLabel
            // 
            this.txtDomainNameLabel.Location = new System.Drawing.Point(176, 92);
            this.txtDomainNameLabel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtDomainNameLabel.Name = "txtDomainNameLabel";
            this.txtDomainNameLabel.Size = new System.Drawing.Size(204, 26);
            this.txtDomainNameLabel.TabIndex = 5;
            this.txtDomainNameLabel.TextChanged += new System.EventHandler(this.txtDomainNameLabel_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Type:";
            // 
            // cmbPublicIpAllocation
            // 
            this.cmbPublicIpAllocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPublicIpAllocation.FormattingEnabled = true;
            this.cmbPublicIpAllocation.Items.AddRange(new object[] {
            "Dynamic",
            "Static"});
            this.cmbPublicIpAllocation.Location = new System.Drawing.Point(132, 55);
            this.cmbPublicIpAllocation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbPublicIpAllocation.Name = "cmbPublicIpAllocation";
            this.cmbPublicIpAllocation.Size = new System.Drawing.Size(247, 28);
            this.cmbPublicIpAllocation.TabIndex = 8;
            this.cmbPublicIpAllocation.SelectedIndexChanged += new System.EventHandler(this.cmbPublicIpAllocation_SelectedIndexChanged);
            // 
            // PublicIpProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbPublicIpAllocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDomainNameLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PublicIpProperties";
            this.Size = new System.Drawing.Size(395, 162);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDomainNameLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPublicIpAllocation;
    }
}

