// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class NetworkSelectionControl
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbExistingARMVNet = new System.Windows.Forms.RadioButton();
            this.rbVNetInMigration = new System.Windows.Forms.RadioButton();
            this.cmbSubnet = new System.Windows.Forms.ComboBox();
            this.cmbVirtualNetwork = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAllocationMethod = new System.Windows.Forms.ComboBox();
            this.txtStaticIp = new MigAz.Azure.UserControls.IPv4AddressBox();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 20);
            this.label8.TabIndex = 36;
            this.label8.Text = "Target Subnet:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 35;
            this.label7.Text = "Target VNet:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 20);
            this.label2.TabIndex = 34;
            this.label2.Text = "Migrate To:";
            // 
            // rbExistingARMVNet
            // 
            this.rbExistingARMVNet.AutoSize = true;
            this.rbExistingARMVNet.Enabled = false;
            this.rbExistingARMVNet.Location = new System.Drawing.Point(130, 38);
            this.rbExistingARMVNet.Name = "rbExistingARMVNet";
            this.rbExistingARMVNet.Size = new System.Drawing.Size(210, 24);
            this.rbExistingARMVNet.TabIndex = 31;
            this.rbExistingARMVNet.Text = "Existing VNet in Location";
            this.rbExistingARMVNet.UseVisualStyleBackColor = true;
            this.rbExistingARMVNet.CheckedChanged += new System.EventHandler(this.rbExistingARMVNet_CheckedChanged);
            // 
            // rbVNetInMigration
            // 
            this.rbVNetInMigration.AutoSize = true;
            this.rbVNetInMigration.Location = new System.Drawing.Point(130, 3);
            this.rbVNetInMigration.Name = "rbVNetInMigration";
            this.rbVNetInMigration.Size = new System.Drawing.Size(203, 24);
            this.rbVNetInMigration.TabIndex = 30;
            this.rbVNetInMigration.Text = "VNet in MigAz Migration";
            this.rbVNetInMigration.UseVisualStyleBackColor = true;
            this.rbVNetInMigration.CheckedChanged += new System.EventHandler(this.rbVNetInMigration_CheckedChanged);
            // 
            // cmbSubnet
            // 
            this.cmbSubnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubnet.FormattingEnabled = true;
            this.cmbSubnet.Location = new System.Drawing.Point(130, 120);
            this.cmbSubnet.Name = "cmbSubnet";
            this.cmbSubnet.Size = new System.Drawing.Size(322, 28);
            this.cmbSubnet.TabIndex = 33;
            this.cmbSubnet.SelectedIndexChanged += new System.EventHandler(this.cmbSubnet_SelectedIndexChanged);
            // 
            // cmbVirtualNetwork
            // 
            this.cmbVirtualNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVirtualNetwork.FormattingEnabled = true;
            this.cmbVirtualNetwork.Location = new System.Drawing.Point(130, 80);
            this.cmbVirtualNetwork.Name = "cmbVirtualNetwork";
            this.cmbVirtualNetwork.Size = new System.Drawing.Size(322, 28);
            this.cmbVirtualNetwork.TabIndex = 32;
            this.cmbVirtualNetwork.SelectedIndexChanged += new System.EventHandler(this.cmbVirtualNetwork_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(0, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 20);
            this.label9.TabIndex = 41;
            this.label9.Text = "Static IP:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 20);
            this.label1.TabIndex = 39;
            this.label1.Text = "Allocation:";
            // 
            // cmbAllocationMethod
            // 
            this.cmbAllocationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAllocationMethod.FormattingEnabled = true;
            this.cmbAllocationMethod.Items.AddRange(new object[] {
            "Dynamic",
            "Static"});
            this.cmbAllocationMethod.Location = new System.Drawing.Point(130, 160);
            this.cmbAllocationMethod.Name = "cmbAllocationMethod";
            this.cmbAllocationMethod.Size = new System.Drawing.Size(322, 28);
            this.cmbAllocationMethod.TabIndex = 38;
            this.cmbAllocationMethod.SelectedIndexChanged += new System.EventHandler(this.cmbAllocationMethod_SelectedIndexChanged);
            // 
            // txtStaticIp
            // 
            this.txtStaticIp.Enabled = false;
            this.txtStaticIp.Location = new System.Drawing.Point(130, 200);
            this.txtStaticIp.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.txtStaticIp.Name = "txtStaticIp";
            this.txtStaticIp.Size = new System.Drawing.Size(150, 31);
            this.txtStaticIp.TabIndex = 42;
            // 
            // NetworkSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtStaticIp);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAllocationMethod);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbExistingARMVNet);
            this.Controls.Add(this.rbVNetInMigration);
            this.Controls.Add(this.cmbSubnet);
            this.Controls.Add(this.cmbVirtualNetwork);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "NetworkSelectionControl";
            this.Size = new System.Drawing.Size(465, 251);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbExistingARMVNet;
        private System.Windows.Forms.RadioButton rbVNetInMigration;
        private System.Windows.Forms.ComboBox cmbSubnet;
        private System.Windows.Forms.ComboBox cmbVirtualNetwork;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAllocationMethod;
        private IPv4AddressBox txtStaticIp;
    }
}

