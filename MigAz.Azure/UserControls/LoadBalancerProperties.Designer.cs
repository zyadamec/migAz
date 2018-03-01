// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class LoadBalancerProperties
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
            this.networkSelectionControl1 = new MigAz.Azure.UserControls.NetworkSelectionControl();
            this.cmbLoadBalancerType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnblPublicProperties = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.resourceSummaryPublicIp = new MigAz.Azure.UserControls.ResourceSummary<MigrationTarget.PublicIp>();
            this.pnblPublicProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(132, 15);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(247, 26);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // networkSelectionControl1
            // 
            this.networkSelectionControl1.ExistingARMVNetEnabled = true;
            this.networkSelectionControl1.Location = new System.Drawing.Point(0, 112);
            this.networkSelectionControl1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.networkSelectionControl1.Name = "networkSelectionControl1";
            this.networkSelectionControl1.Size = new System.Drawing.Size(465, 231);
            this.networkSelectionControl1.TabIndex = 5;
            this.networkSelectionControl1.VirtualNetworkTarget = null;
            // 
            // cmbLoadBalancerType
            // 
            this.cmbLoadBalancerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoadBalancerType.FormattingEnabled = true;
            this.cmbLoadBalancerType.Items.AddRange(new object[] {
            "Public",
            "Internal"});
            this.cmbLoadBalancerType.Location = new System.Drawing.Point(132, 54);
            this.cmbLoadBalancerType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbLoadBalancerType.Name = "cmbLoadBalancerType";
            this.cmbLoadBalancerType.Size = new System.Drawing.Size(247, 28);
            this.cmbLoadBalancerType.TabIndex = 6;
            this.cmbLoadBalancerType.SelectedIndexChanged += new System.EventHandler(this.cmbLoadBalancerType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Type:";
            // 
            // pnblPublicProperties
            // 
            this.pnblPublicProperties.Controls.Add(this.label8);
            this.pnblPublicProperties.Controls.Add(this.resourceSummaryPublicIp);
            this.pnblPublicProperties.Location = new System.Drawing.Point(7, 103);
            this.pnblPublicProperties.Name = "pnblPublicProperties";
            this.pnblPublicProperties.Size = new System.Drawing.Size(458, 51);
            this.pnblPublicProperties.TabIndex = 52;
            this.pnblPublicProperties.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 53;
            this.label8.Text = "Public IP:";
            // 
            // resourceSummaryPublicIp
            // 
            this.resourceSummaryPublicIp.AutoSize = true;
            this.resourceSummaryPublicIp.Location = new System.Drawing.Point(86, 0);
            this.resourceSummaryPublicIp.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.resourceSummaryPublicIp.Name = "resourceSummaryPublicIp";
            this.resourceSummaryPublicIp.Size = new System.Drawing.Size(106, 43);
            this.resourceSummaryPublicIp.TabIndex = 52;
            // 
            // LoadBalancerProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnblPublicProperties);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbLoadBalancerType);
            this.Controls.Add(this.networkSelectionControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Name = "LoadBalancerProperties";
            this.Size = new System.Drawing.Size(465, 707);
            this.pnblPublicProperties.ResumeLayout(false);
            this.pnblPublicProperties.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private NetworkSelectionControl networkSelectionControl1;
        private System.Windows.Forms.ComboBox cmbLoadBalancerType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnblPublicProperties;
        private System.Windows.Forms.Label label8;
        private ResourceSummary<MigrationTarget.PublicIp> resourceSummaryPublicIp;
    }
}

