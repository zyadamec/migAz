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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbExistingARMVNet = new System.Windows.Forms.RadioButton();
            this.rbVNetInMigration = new System.Windows.Forms.RadioButton();
            this.cmbExistingArmSubnet = new System.Windows.Forms.ComboBox();
            this.cmbExistingArmVNets = new System.Windows.Forms.ComboBox();
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
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 437);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 25);
            this.label8.TabIndex = 29;
            this.label8.Text = "Target Subnet:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 382);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 25);
            this.label7.TabIndex = 28;
            this.label7.Text = "Target VNet:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 283);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 25);
            this.label2.TabIndex = 27;
            this.label2.Text = "Migrate To:";
            // 
            // rbExistingARMVNet
            // 
            this.rbExistingARMVNet.AutoSize = true;
            this.rbExistingARMVNet.Location = new System.Drawing.Point(178, 327);
            this.rbExistingARMVNet.Name = "rbExistingARMVNet";
            this.rbExistingARMVNet.Size = new System.Drawing.Size(316, 29);
            this.rbExistingARMVNet.TabIndex = 19;
            this.rbExistingARMVNet.Text = "Existing ARM VNet in Target";
            this.rbExistingARMVNet.UseVisualStyleBackColor = true;
            this.rbExistingARMVNet.CheckedChanged += new System.EventHandler(this.rbExistingARMVNet_CheckedChanged);
            // 
            // rbVNetInMigration
            // 
            this.rbVNetInMigration.AutoSize = true;
            this.rbVNetInMigration.Location = new System.Drawing.Point(178, 283);
            this.rbVNetInMigration.Name = "rbVNetInMigration";
            this.rbVNetInMigration.Size = new System.Drawing.Size(274, 29);
            this.rbVNetInMigration.TabIndex = 17;
            this.rbVNetInMigration.Text = "VNet in MigAz Migration";
            this.rbVNetInMigration.UseVisualStyleBackColor = true;
            this.rbVNetInMigration.CheckedChanged += new System.EventHandler(this.rbVNetInMigration_CheckedChanged);
            // 
            // cmbExistingArmSubnet
            // 
            this.cmbExistingArmSubnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmSubnet.FormattingEnabled = true;
            this.cmbExistingArmSubnet.Location = new System.Drawing.Point(178, 429);
            this.cmbExistingArmSubnet.Name = "cmbExistingArmSubnet";
            this.cmbExistingArmSubnet.Size = new System.Drawing.Size(429, 33);
            this.cmbExistingArmSubnet.TabIndex = 23;
            this.cmbExistingArmSubnet.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmSubnet_SelectedIndexChanged);
            // 
            // cmbExistingArmVNets
            // 
            this.cmbExistingArmVNets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmVNets.FormattingEnabled = true;
            this.cmbExistingArmVNets.Location = new System.Drawing.Point(178, 379);
            this.cmbExistingArmVNets.Name = "cmbExistingArmVNets";
            this.cmbExistingArmVNets.Size = new System.Drawing.Size(429, 33);
            this.cmbExistingArmVNets.TabIndex = 21;
            this.cmbExistingArmVNets.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmVNets_SelectedIndexChanged);
            // 
            // lblVirtualNetworkName
            // 
            this.lblVirtualNetworkName.AutoSize = true;
            this.lblVirtualNetworkName.Location = new System.Drawing.Point(176, 46);
            this.lblVirtualNetworkName.Name = "lblVirtualNetworkName";
            this.lblVirtualNetworkName.Size = new System.Drawing.Size(121, 25);
            this.lblVirtualNetworkName.TabIndex = 26;
            this.lblVirtualNetworkName.Text = "VNet Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 25);
            this.label5.TabIndex = 25;
            this.label5.Text = "Source VNet:";
            // 
            // lblStaticIpAddress
            // 
            this.lblStaticIpAddress.AutoSize = true;
            this.lblStaticIpAddress.Location = new System.Drawing.Point(176, 135);
            this.lblStaticIpAddress.Name = "lblStaticIpAddress";
            this.lblStaticIpAddress.Size = new System.Drawing.Size(78, 25);
            this.lblStaticIpAddress.TabIndex = 24;
            this.lblStaticIpAddress.Text = "0.0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 25);
            this.label3.TabIndex = 22;
            this.label3.Text = "Private IP:";
            // 
            // lblSubnetName
            // 
            this.lblSubnetName.AutoSize = true;
            this.lblSubnetName.Location = new System.Drawing.Point(176, 91);
            this.lblSubnetName.Name = "lblSubnetName";
            this.lblSubnetName.Size = new System.Drawing.Size(142, 25);
            this.lblSubnetName.TabIndex = 20;
            this.lblSubnetName.Text = "Subnet Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 25);
            this.label4.TabIndex = 18;
            this.label4.Text = "Source Subnet:";
            // 
            // lblARMVirtualNetworkName
            // 
            this.lblARMVirtualNetworkName.AutoSize = true;
            this.lblARMVirtualNetworkName.Location = new System.Drawing.Point(9, 234);
            this.lblARMVirtualNetworkName.Name = "lblARMVirtualNetworkName";
            this.lblARMVirtualNetworkName.Size = new System.Drawing.Size(142, 25);
            this.lblARMVirtualNetworkName.TabIndex = 31;
            this.lblARMVirtualNetworkName.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(178, 234);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(429, 31);
            this.txtTargetName.TabIndex = 30;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(176, 7);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(157, 25);
            this.lblSourceName.TabIndex = 33;
            this.lblSourceName.Text = "Interface Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 25);
            this.label6.TabIndex = 32;
            this.label6.Text = "Source Name:";
            // 
            // NetworkInterfaceProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSourceName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblARMVirtualNetworkName);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbExistingARMVNet);
            this.Controls.Add(this.rbVNetInMigration);
            this.Controls.Add(this.cmbExistingArmSubnet);
            this.Controls.Add(this.cmbExistingArmVNets);
            this.Controls.Add(this.lblVirtualNetworkName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblStaticIpAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSubnetName);
            this.Controls.Add(this.label4);
            this.Name = "NetworkInterfaceProperties";
            this.Size = new System.Drawing.Size(621, 557);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbExistingARMVNet;
        private System.Windows.Forms.RadioButton rbVNetInMigration;
        private System.Windows.Forms.ComboBox cmbExistingArmSubnet;
        private System.Windows.Forms.ComboBox cmbExistingArmVNets;
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
    }
}
