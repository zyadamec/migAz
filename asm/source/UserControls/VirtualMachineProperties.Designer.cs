namespace MigAz.UserControls
{
    partial class VirtualMachineProperties
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblSubnetName = new System.Windows.Forms.Label();
            this.lblStaticIpAddress = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRoleSize = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblOS = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblVirtualNetworkName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbExistingArmVNets = new System.Windows.Forms.ComboBox();
            this.cmbExistingArmSubnet = new System.Windows.Forms.ComboBox();
            this.rbVNetInMigration = new System.Windows.Forms.RadioButton();
            this.rbExistingARMVNet = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.diskProperties1 = new MigAz.UserControls.DiskProperties();
            this.label12 = new System.Windows.Forms.Label();
            this.lblASMVMName = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtARMVMName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 259);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "ASM Subnet:";
            // 
            // lblSubnetName
            // 
            this.lblSubnetName.AutoSize = true;
            this.lblSubnetName.Location = new System.Drawing.Point(176, 259);
            this.lblSubnetName.Name = "lblSubnetName";
            this.lblSubnetName.Size = new System.Drawing.Size(142, 25);
            this.lblSubnetName.TabIndex = 1;
            this.lblSubnetName.Text = "Subnet Name";
            // 
            // lblStaticIpAddress
            // 
            this.lblStaticIpAddress.AutoSize = true;
            this.lblStaticIpAddress.Location = new System.Drawing.Point(176, 303);
            this.lblStaticIpAddress.Name = "lblStaticIpAddress";
            this.lblStaticIpAddress.Size = new System.Drawing.Size(78, 25);
            this.lblStaticIpAddress.TabIndex = 3;
            this.lblStaticIpAddress.Text = "0.0.0.0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "Static IP:";
            // 
            // lblRoleSize
            // 
            this.lblRoleSize.AutoSize = true;
            this.lblRoleSize.Location = new System.Drawing.Point(176, 68);
            this.lblRoleSize.Name = "lblRoleSize";
            this.lblRoleSize.Size = new System.Drawing.Size(56, 25);
            this.lblRoleSize.TabIndex = 5;
            this.lblRoleSize.Text = "Role";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "Size:";
            // 
            // lblOS
            // 
            this.lblOS.AutoSize = true;
            this.lblOS.Location = new System.Drawing.Point(176, 103);
            this.lblOS.Name = "lblOS";
            this.lblOS.Size = new System.Drawing.Size(42, 25);
            this.lblOS.TabIndex = 7;
            this.lblOS.Text = "OS";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 25);
            this.label6.TabIndex = 6;
            this.label6.Text = "OS:";
            // 
            // lblVirtualNetworkName
            // 
            this.lblVirtualNetworkName.AutoSize = true;
            this.lblVirtualNetworkName.Location = new System.Drawing.Point(176, 214);
            this.lblVirtualNetworkName.Name = "lblVirtualNetworkName";
            this.lblVirtualNetworkName.Size = new System.Drawing.Size(121, 25);
            this.lblVirtualNetworkName.TabIndex = 9;
            this.lblVirtualNetworkName.Text = "VNet Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 214);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 25);
            this.label5.TabIndex = 8;
            this.label5.Text = "ASM VNet:";
            // 
            // cmbExistingArmVNets
            // 
            this.cmbExistingArmVNets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmVNets.FormattingEnabled = true;
            this.cmbExistingArmVNets.Location = new System.Drawing.Point(181, 438);
            this.cmbExistingArmVNets.Name = "cmbExistingArmVNets";
            this.cmbExistingArmVNets.Size = new System.Drawing.Size(429, 33);
            this.cmbExistingArmVNets.TabIndex = 2;
            this.cmbExistingArmVNets.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmVNets_SelectedIndexChanged);
            // 
            // cmbExistingArmSubnet
            // 
            this.cmbExistingArmSubnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmSubnet.FormattingEnabled = true;
            this.cmbExistingArmSubnet.Location = new System.Drawing.Point(181, 488);
            this.cmbExistingArmSubnet.Name = "cmbExistingArmSubnet";
            this.cmbExistingArmSubnet.Size = new System.Drawing.Size(429, 33);
            this.cmbExistingArmSubnet.TabIndex = 3;
            this.cmbExistingArmSubnet.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmSubnet_SelectedIndexChanged);
            // 
            // rbVNetInMigration
            // 
            this.rbVNetInMigration.AutoSize = true;
            this.rbVNetInMigration.Location = new System.Drawing.Point(181, 342);
            this.rbVNetInMigration.Name = "rbVNetInMigration";
            this.rbVNetInMigration.Size = new System.Drawing.Size(403, 29);
            this.rbVNetInMigration.TabIndex = 0;
            this.rbVNetInMigration.Text = "VNet in MigAz ASM to ARM Migration";
            this.rbVNetInMigration.UseVisualStyleBackColor = true;
            this.rbVNetInMigration.CheckedChanged += new System.EventHandler(this.rbVNetInMigration_CheckedChanged);
            // 
            // rbExistingARMVNet
            // 
            this.rbExistingARMVNet.AutoSize = true;
            this.rbExistingARMVNet.Location = new System.Drawing.Point(181, 386);
            this.rbExistingARMVNet.Name = "rbExistingARMVNet";
            this.rbExistingARMVNet.Size = new System.Drawing.Size(359, 29);
            this.rbExistingARMVNet.TabIndex = 1;
            this.rbExistingARMVNet.Text = "ARM VNet in Target Subscription";
            this.rbExistingARMVNet.UseVisualStyleBackColor = true;
            this.rbExistingARMVNet.CheckedChanged += new System.EventHandler(this.rbExistingARMVNet_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 342);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "Migrate To:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 441);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 25);
            this.label7.TabIndex = 15;
            this.label7.Text = "Target VNet:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 496);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 25);
            this.label8.TabIndex = 16;
            this.label8.Text = "Target Subnet:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(12, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(164, 25);
            this.label9.TabIndex = 17;
            this.label9.Text = "Vitual Network";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(3, 589);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 25);
            this.label10.TabIndex = 18;
            this.label10.Text = "OS Disk";
            // 
            // diskProperties1
            // 
            this.diskProperties1.Location = new System.Drawing.Point(0, 630);
            this.diskProperties1.Name = "diskProperties1";
            this.diskProperties1.Size = new System.Drawing.Size(640, 458);
            this.diskProperties1.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(165, 25);
            this.label12.TabIndex = 22;
            this.label12.Text = "ARM VM Name:";
            // 
            // lblASMVMName
            // 
            this.lblASMVMName.AutoSize = true;
            this.lblASMVMName.Location = new System.Drawing.Point(176, 0);
            this.lblASMVMName.Name = "lblASMVMName";
            this.lblASMVMName.Size = new System.Drawing.Size(158, 25);
            this.lblASMVMName.TabIndex = 21;
            this.lblASMVMName.Text = "ASM VM Name";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(164, 25);
            this.label14.TabIndex = 20;
            this.label14.Text = "ASM VM Name:";
            // 
            // txtARMVMName
            // 
            this.txtARMVMName.Location = new System.Drawing.Point(181, 29);
            this.txtARMVMName.Name = "txtARMVMName";
            this.txtARMVMName.Size = new System.Drawing.Size(403, 31);
            this.txtARMVMName.TabIndex = 23;
            this.txtARMVMName.TextChanged += new System.EventHandler(this.txtARMVMName_TextChanged);
            // 
            // VirtualMachineProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtARMVMName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblASMVMName);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.diskProperties1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbExistingARMVNet);
            this.Controls.Add(this.rbVNetInMigration);
            this.Controls.Add(this.cmbExistingArmSubnet);
            this.Controls.Add(this.cmbExistingArmVNets);
            this.Controls.Add(this.lblVirtualNetworkName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblOS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblRoleSize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblStaticIpAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSubnetName);
            this.Controls.Add(this.label1);
            this.Name = "VirtualMachineProperties";
            this.Size = new System.Drawing.Size(625, 1101);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSubnetName;
        private System.Windows.Forms.Label lblStaticIpAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblRoleSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblOS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblVirtualNetworkName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbExistingArmVNets;
        private System.Windows.Forms.ComboBox cmbExistingArmSubnet;
        private System.Windows.Forms.RadioButton rbVNetInMigration;
        private System.Windows.Forms.RadioButton rbExistingARMVNet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private DiskProperties diskProperties1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblASMVMName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtARMVMName;
    }
}
