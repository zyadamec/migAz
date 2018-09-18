// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class NetworkInterfaceProperties
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
            this.lblVirtualNetworkName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblStaticIpAddress = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSubnetName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblARMVirtualNetworkName = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.lblSourceName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbIPForwardingDisabled = new System.Windows.Forms.RadioButton();
            this.rbIPForwardingEnabled = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.virtualMachineSummary = new MigAz.Azure.UserControls.ResourceSummary();
            this.networkSelectionControl1 = new MigAz.Azure.UserControls.NetworkSelectionControl();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.networkSecurityGroup = new MigAz.Azure.UserControls.ResourceSummary();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlAcceleratedNetworking = new System.Windows.Forms.Panel();
            this.rbAcceleratedNetworkingDisabled = new System.Windows.Forms.RadioButton();
            this.rbAcceleratedNetworkingEnabled = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.publicIpSelectionControl1 = new MigAz.Azure.UserControls.PublicIpSelectionControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbPublicIpDisabled = new System.Windows.Forms.RadioButton();
            this.rbPublicIpEnabled = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.pnlAcceleratedNetworking.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVirtualNetworkName
            // 
            this.lblVirtualNetworkName.AutoSize = true;
            this.lblVirtualNetworkName.Location = new System.Drawing.Point(88, 24);
            this.lblVirtualNetworkName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVirtualNetworkName.Name = "lblVirtualNetworkName";
            this.lblVirtualNetworkName.Size = new System.Drawing.Size(62, 13);
            this.lblVirtualNetworkName.TabIndex = 26;
            this.lblVirtualNetworkName.Text = "VNet Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 24);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Source VNet:";
            // 
            // lblStaticIpAddress
            // 
            this.lblStaticIpAddress.AutoSize = true;
            this.lblStaticIpAddress.Location = new System.Drawing.Point(88, 70);
            this.lblStaticIpAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStaticIpAddress.Name = "lblStaticIpAddress";
            this.lblStaticIpAddress.Size = new System.Drawing.Size(40, 13);
            this.lblStaticIpAddress.TabIndex = 24;
            this.lblStaticIpAddress.Text = "0.0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Private IP:";
            // 
            // lblSubnetName
            // 
            this.lblSubnetName.AutoSize = true;
            this.lblSubnetName.Location = new System.Drawing.Point(88, 47);
            this.lblSubnetName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubnetName.Name = "lblSubnetName";
            this.lblSubnetName.Size = new System.Drawing.Size(72, 13);
            this.lblSubnetName.TabIndex = 20;
            this.lblSubnetName.Text = "Subnet Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 47);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Source Subnet:";
            // 
            // lblARMVirtualNetworkName
            // 
            this.lblARMVirtualNetworkName.AutoSize = true;
            this.lblARMVirtualNetworkName.Location = new System.Drawing.Point(4, 122);
            this.lblARMVirtualNetworkName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblARMVirtualNetworkName.Name = "lblARMVirtualNetworkName";
            this.lblARMVirtualNetworkName.Size = new System.Drawing.Size(38, 13);
            this.lblARMVirtualNetworkName.TabIndex = 31;
            this.lblARMVirtualNetworkName.Text = "Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(89, 122);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(216, 20);
            this.txtTargetName.TabIndex = 30;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(88, 4);
            this.lblSourceName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(80, 13);
            this.lblSourceName.TabIndex = 33;
            this.lblSourceName.Text = "Interface Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 4);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Source Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 301);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "IP Forwarding:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbIPForwardingDisabled);
            this.panel1.Controls.Add(this.rbIPForwardingEnabled);
            this.panel1.Location = new System.Drawing.Point(130, 298);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 24);
            this.panel1.TabIndex = 42;
            // 
            // rbIPForwardingDisabled
            // 
            this.rbIPForwardingDisabled.AutoSize = true;
            this.rbIPForwardingDisabled.Checked = true;
            this.rbIPForwardingDisabled.Location = new System.Drawing.Point(0, 2);
            this.rbIPForwardingDisabled.Name = "rbIPForwardingDisabled";
            this.rbIPForwardingDisabled.Size = new System.Drawing.Size(66, 17);
            this.rbIPForwardingDisabled.TabIndex = 43;
            this.rbIPForwardingDisabled.TabStop = true;
            this.rbIPForwardingDisabled.Text = "Disabled";
            this.rbIPForwardingDisabled.UseVisualStyleBackColor = true;
            this.rbIPForwardingDisabled.CheckedChanged += new System.EventHandler(this.rbIPForwardingDisabled_CheckedChanged);
            // 
            // rbIPForwardingEnabled
            // 
            this.rbIPForwardingEnabled.AutoSize = true;
            this.rbIPForwardingEnabled.Location = new System.Drawing.Point(75, 2);
            this.rbIPForwardingEnabled.Name = "rbIPForwardingEnabled";
            this.rbIPForwardingEnabled.Size = new System.Drawing.Size(64, 17);
            this.rbIPForwardingEnabled.TabIndex = 42;
            this.rbIPForwardingEnabled.Text = "Enabled";
            this.rbIPForwardingEnabled.UseVisualStyleBackColor = true;
            this.rbIPForwardingEnabled.CheckedChanged += new System.EventHandler(this.rbIPForwardingEnabled_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 454);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 13);
            this.label13.TabIndex = 44;
            this.label13.Text = "Virtual Machine:";
            // 
            // virtualMachineSummary
            // 
            this.virtualMachineSummary.AutoSize = true;
            this.virtualMachineSummary.Location = new System.Drawing.Point(126, 453);
            this.virtualMachineSummary.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.virtualMachineSummary.Name = "virtualMachineSummary";
            this.virtualMachineSummary.Size = new System.Drawing.Size(193, 28);
            this.virtualMachineSummary.TabIndex = 43;
            // 
            // networkSelectionControl1
            // 
            this.networkSelectionControl1.ExistingARMVNetEnabled = true;
            this.networkSelectionControl1.Location = new System.Drawing.Point(2, 145);
            this.networkSelectionControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.networkSelectionControl1.Name = "networkSelectionControl1";
            this.networkSelectionControl1.Size = new System.Drawing.Size(310, 150);
            this.networkSelectionControl1.TabIndex = 38;
            this.networkSelectionControl1.VirtualNetworkTarget = null;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 96);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 45;
            this.label7.Text = "Target";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 478);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Network Security Group:";
            // 
            // networkSecurityGroup
            // 
            this.networkSecurityGroup.AutoSize = true;
            this.networkSecurityGroup.Location = new System.Drawing.Point(126, 476);
            this.networkSecurityGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.networkSecurityGroup.Name = "networkSecurityGroup";
            this.networkSecurityGroup.Size = new System.Drawing.Size(193, 28);
            this.networkSecurityGroup.TabIndex = 46;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(2, 349);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 49;
            this.label8.Text = "Public IP:";
            // 
            // pnlAcceleratedNetworking
            // 
            this.pnlAcceleratedNetworking.Controls.Add(this.rbAcceleratedNetworkingDisabled);
            this.pnlAcceleratedNetworking.Controls.Add(this.rbAcceleratedNetworkingEnabled);
            this.pnlAcceleratedNetworking.Location = new System.Drawing.Point(130, 323);
            this.pnlAcceleratedNetworking.Name = "pnlAcceleratedNetworking";
            this.pnlAcceleratedNetworking.Size = new System.Drawing.Size(153, 24);
            this.pnlAcceleratedNetworking.TabIndex = 51;
            // 
            // rbAcceleratedNetworkingDisabled
            // 
            this.rbAcceleratedNetworkingDisabled.AutoSize = true;
            this.rbAcceleratedNetworkingDisabled.Checked = true;
            this.rbAcceleratedNetworkingDisabled.Location = new System.Drawing.Point(0, 2);
            this.rbAcceleratedNetworkingDisabled.Name = "rbAcceleratedNetworkingDisabled";
            this.rbAcceleratedNetworkingDisabled.Size = new System.Drawing.Size(66, 17);
            this.rbAcceleratedNetworkingDisabled.TabIndex = 43;
            this.rbAcceleratedNetworkingDisabled.TabStop = true;
            this.rbAcceleratedNetworkingDisabled.Text = "Disabled";
            this.rbAcceleratedNetworkingDisabled.UseVisualStyleBackColor = true;
            this.rbAcceleratedNetworkingDisabled.CheckedChanged += new System.EventHandler(this.rbAcceleratedNetworkingDisabled_CheckedChanged);
            // 
            // rbAcceleratedNetworkingEnabled
            // 
            this.rbAcceleratedNetworkingEnabled.AutoSize = true;
            this.rbAcceleratedNetworkingEnabled.Location = new System.Drawing.Point(75, 2);
            this.rbAcceleratedNetworkingEnabled.Name = "rbAcceleratedNetworkingEnabled";
            this.rbAcceleratedNetworkingEnabled.Size = new System.Drawing.Size(64, 17);
            this.rbAcceleratedNetworkingEnabled.TabIndex = 42;
            this.rbAcceleratedNetworkingEnabled.Text = "Enabled";
            this.rbAcceleratedNetworkingEnabled.UseVisualStyleBackColor = true;
            this.rbAcceleratedNetworkingEnabled.CheckedChanged += new System.EventHandler(this.rbAcceleratedNetworkingEnabled_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(2, 325);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(124, 13);
            this.label9.TabIndex = 50;
            this.label9.Text = "Accelerated Networking:";
            // 
            // publicIpSelectionControl1
            // 
            this.publicIpSelectionControl1.Enabled = false;
            this.publicIpSelectionControl1.ExistingARMPublicIpEnabled = false;
            this.publicIpSelectionControl1.Location = new System.Drawing.Point(0, 375);
            this.publicIpSelectionControl1.Name = "publicIpSelectionControl1";
            this.publicIpSelectionControl1.PublicIp = null;
            this.publicIpSelectionControl1.Size = new System.Drawing.Size(310, 76);
            this.publicIpSelectionControl1.TabIndex = 52;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbPublicIpDisabled);
            this.panel2.Controls.Add(this.rbPublicIpEnabled);
            this.panel2.Location = new System.Drawing.Point(130, 349);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(153, 24);
            this.panel2.TabIndex = 53;
            // 
            // rbPublicIpDisabled
            // 
            this.rbPublicIpDisabled.AutoSize = true;
            this.rbPublicIpDisabled.Checked = true;
            this.rbPublicIpDisabled.Location = new System.Drawing.Point(0, 2);
            this.rbPublicIpDisabled.Name = "rbPublicIpDisabled";
            this.rbPublicIpDisabled.Size = new System.Drawing.Size(66, 17);
            this.rbPublicIpDisabled.TabIndex = 43;
            this.rbPublicIpDisabled.TabStop = true;
            this.rbPublicIpDisabled.Text = "Disabled";
            this.rbPublicIpDisabled.UseVisualStyleBackColor = true;
            this.rbPublicIpDisabled.CheckedChanged += new System.EventHandler(this.rbPublicIpDisabled_CheckedChanged);
            // 
            // rbPublicIpEnabled
            // 
            this.rbPublicIpEnabled.AutoSize = true;
            this.rbPublicIpEnabled.Location = new System.Drawing.Point(75, 2);
            this.rbPublicIpEnabled.Name = "rbPublicIpEnabled";
            this.rbPublicIpEnabled.Size = new System.Drawing.Size(64, 17);
            this.rbPublicIpEnabled.TabIndex = 42;
            this.rbPublicIpEnabled.Text = "Enabled";
            this.rbPublicIpEnabled.UseVisualStyleBackColor = true;
            this.rbPublicIpEnabled.CheckedChanged += new System.EventHandler(this.rbPublicIpEnabled_CheckedChanged);
            // 
            // NetworkInterfaceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.publicIpSelectionControl1);
            this.Controls.Add(this.pnlAcceleratedNetworking);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.networkSecurityGroup);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.virtualMachineSummary);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSourceName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblARMVirtualNetworkName);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.lblVirtualNetworkName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblStaticIpAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSubnetName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.networkSelectionControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "NetworkInterfaceProperties";
            this.Size = new System.Drawing.Size(310, 514);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlAcceleratedNetworking.ResumeLayout(false);
            this.pnlAcceleratedNetworking.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblVirtualNetworkName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStaticIpAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSubnetName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblARMVirtualNetworkName;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label lblSourceName;
        private System.Windows.Forms.Label label6;
        private NetworkSelectionControl networkSelectionControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbIPForwardingDisabled;
        private System.Windows.Forms.RadioButton rbIPForwardingEnabled;
        private System.Windows.Forms.Label label13;
        private ResourceSummary virtualMachineSummary;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private ResourceSummary networkSecurityGroup;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel pnlAcceleratedNetworking;
        private System.Windows.Forms.RadioButton rbAcceleratedNetworkingDisabled;
        private System.Windows.Forms.RadioButton rbAcceleratedNetworkingEnabled;
        private System.Windows.Forms.Label label9;
        private PublicIpSelectionControl publicIpSelectionControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rbPublicIpDisabled;
        private System.Windows.Forms.RadioButton rbPublicIpEnabled;
    }
}

