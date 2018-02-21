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
            this.resourceSummaryPublicIp = new MigAz.Azure.UserControls.ResourceSummary();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVirtualNetworkName
            // 
            this.lblVirtualNetworkName.AutoSize = true;
            this.lblVirtualNetworkName.Location = new System.Drawing.Point(132, 37);
            this.lblVirtualNetworkName.Name = "lblVirtualNetworkName";
            this.lblVirtualNetworkName.Size = new System.Drawing.Size(91, 20);
            this.lblVirtualNetworkName.TabIndex = 26;
            this.lblVirtualNetworkName.Text = "VNet Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 25;
            this.label5.Text = "Source VNet:";
            // 
            // lblStaticIpAddress
            // 
            this.lblStaticIpAddress.AutoSize = true;
            this.lblStaticIpAddress.Location = new System.Drawing.Point(132, 108);
            this.lblStaticIpAddress.Name = "lblStaticIpAddress";
            this.lblStaticIpAddress.Size = new System.Drawing.Size(57, 20);
            this.lblStaticIpAddress.TabIndex = 24;
            this.lblStaticIpAddress.Text = "0.0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "Private IP:";
            // 
            // lblSubnetName
            // 
            this.lblSubnetName.AutoSize = true;
            this.lblSubnetName.Location = new System.Drawing.Point(132, 72);
            this.lblSubnetName.Name = "lblSubnetName";
            this.lblSubnetName.Size = new System.Drawing.Size(107, 20);
            this.lblSubnetName.TabIndex = 20;
            this.lblSubnetName.Text = "Subnet Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 20);
            this.label4.TabIndex = 18;
            this.label4.Text = "Source Subnet:";
            // 
            // lblARMVirtualNetworkName
            // 
            this.lblARMVirtualNetworkName.AutoSize = true;
            this.lblARMVirtualNetworkName.Location = new System.Drawing.Point(6, 188);
            this.lblARMVirtualNetworkName.Name = "lblARMVirtualNetworkName";
            this.lblARMVirtualNetworkName.Size = new System.Drawing.Size(55, 20);
            this.lblARMVirtualNetworkName.TabIndex = 31;
            this.lblARMVirtualNetworkName.Text = "Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(134, 188);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(322, 26);
            this.txtTargetName.TabIndex = 30;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(132, 6);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(119, 20);
            this.lblSourceName.TabIndex = 33;
            this.lblSourceName.Text = "Interface Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 20);
            this.label6.TabIndex = 32;
            this.label6.Text = "Source Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 465);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 39;
            this.label1.Text = "IP Forwarding:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbIPForwardingDisabled);
            this.panel1.Controls.Add(this.rbIPForwardingEnabled);
            this.panel1.Location = new System.Drawing.Point(136, 462);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 37);
            this.panel1.TabIndex = 42;
            // 
            // rbIPForwardingDisabled
            // 
            this.rbIPForwardingDisabled.AutoSize = true;
            this.rbIPForwardingDisabled.Location = new System.Drawing.Point(116, 3);
            this.rbIPForwardingDisabled.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbIPForwardingDisabled.Name = "rbIPForwardingDisabled";
            this.rbIPForwardingDisabled.Size = new System.Drawing.Size(96, 24);
            this.rbIPForwardingDisabled.TabIndex = 43;
            this.rbIPForwardingDisabled.Text = "Disabled";
            this.rbIPForwardingDisabled.UseVisualStyleBackColor = true;
            this.rbIPForwardingDisabled.CheckedChanged += new System.EventHandler(this.rbIPForwardingDisabled_CheckedChanged);
            // 
            // rbIPForwardingEnabled
            // 
            this.rbIPForwardingEnabled.AutoSize = true;
            this.rbIPForwardingEnabled.Location = new System.Drawing.Point(0, 3);
            this.rbIPForwardingEnabled.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbIPForwardingEnabled.Name = "rbIPForwardingEnabled";
            this.rbIPForwardingEnabled.Size = new System.Drawing.Size(93, 24);
            this.rbIPForwardingEnabled.TabIndex = 42;
            this.rbIPForwardingEnabled.Text = "Enabled";
            this.rbIPForwardingEnabled.UseVisualStyleBackColor = true;
            this.rbIPForwardingEnabled.CheckedChanged += new System.EventHandler(this.rbIPForwardingEnabled_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 506);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(122, 20);
            this.label13.TabIndex = 44;
            this.label13.Text = "Virtual Machine:";
            // 
            // virtualMachineSummary
            // 
            this.virtualMachineSummary.AutoSize = true;
            this.virtualMachineSummary.Location = new System.Drawing.Point(189, 503);
            this.virtualMachineSummary.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.virtualMachineSummary.Name = "virtualMachineSummary";
            this.virtualMachineSummary.Size = new System.Drawing.Size(106, 43);
            this.virtualMachineSummary.TabIndex = 43;
            // 
            // networkSelectionControl1
            // 
            this.networkSelectionControl1.ExistingARMVNetEnabled = true;
            this.networkSelectionControl1.Location = new System.Drawing.Point(3, 222);
            this.networkSelectionControl1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.networkSelectionControl1.Name = "networkSelectionControl1";
            this.networkSelectionControl1.Size = new System.Drawing.Size(465, 231);
            this.networkSelectionControl1.TabIndex = 38;
            this.networkSelectionControl1.VirtualNetworkTarget = null;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 20);
            this.label7.TabIndex = 45;
            this.label7.Text = "Target";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 538);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 20);
            this.label2.TabIndex = 47;
            this.label2.Text = "Network Security Group:";
            // 
            // networkSecurityGroup
            // 
            this.networkSecurityGroup.AutoSize = true;
            this.networkSecurityGroup.Location = new System.Drawing.Point(189, 535);
            this.networkSecurityGroup.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.networkSecurityGroup.Name = "networkSecurityGroup";
            this.networkSecurityGroup.Size = new System.Drawing.Size(106, 43);
            this.networkSecurityGroup.TabIndex = 46;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 571);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 20);
            this.label8.TabIndex = 49;
            this.label8.Text = "Public IP:";
            // 
            // resourceSummaryPublicIp
            // 
            this.resourceSummaryPublicIp.AutoSize = true;
            this.resourceSummaryPublicIp.Location = new System.Drawing.Point(189, 568);
            this.resourceSummaryPublicIp.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.resourceSummaryPublicIp.Name = "resourceSummaryPublicIp";
            this.resourceSummaryPublicIp.Size = new System.Drawing.Size(106, 43);
            this.resourceSummaryPublicIp.TabIndex = 48;
            // 
            // NetworkInterfaceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.resourceSummaryPublicIp);
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
            this.Name = "NetworkInterfaceProperties";
            this.Size = new System.Drawing.Size(465, 740);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private ResourceSummary resourceSummaryPublicIp;
    }
}
