namespace MigAz.Forms
{
    partial class OptionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPublicIPSuffix = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLoadBalancerSuffix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.txtStorageAccountSuffix = new System.Windows.Forms.TextBox();
            this.txtVirtualNetworkSuffix = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtVirtualNetworkGatewaySuffix = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNetworkSecurityGroupSuffix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResourceGroupSuffix = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApplyDefaultNaming = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.txtAvailabilitySetSuffix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVirtualMachineSuffix = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNetworkInterfaceCardSuffix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbLoginPromptBehavior = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbDefaultAzureEnvironment = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.upDownAccessSASMinutes = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbManagedDisk = new System.Windows.Forms.RadioButton();
            this.rbClassicDisk = new System.Windows.Forms.RadioButton();
            this.chkSaveSelection = new System.Windows.Forms.CheckBox();
            this.chkAutoSelectDependencies = new System.Windows.Forms.CheckBox();
            this.chkBuildEmpty = new System.Windows.Forms.CheckBox();
            this.chkAllowTelemetry = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownAccessSASMinutes)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(632, 691);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(111, 35);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(762, 691);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(111, 35);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPublicIPSuffix);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtLoadBalancerSuffix);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblSuffix);
            this.groupBox1.Controls.Add(this.txtStorageAccountSuffix);
            this.groupBox1.Controls.Add(this.txtVirtualNetworkSuffix);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtVirtualNetworkGatewaySuffix);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtNetworkSecurityGroupSuffix);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtResourceGroupSuffix);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnApplyDefaultNaming);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.txtAvailabilitySetSuffix);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtVirtualMachineSuffix);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNetworkInterfaceCardSuffix);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(20, 339);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(855, 332);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Azure Resource Manager (ARM) Object Naming Suffixes";
            // 
            // txtPublicIPSuffix
            // 
            this.txtPublicIPSuffix.Location = new System.Drawing.Point(243, 262);
            this.txtPublicIPSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtPublicIPSuffix.MaxLength = 10;
            this.txtPublicIPSuffix.Name = "txtPublicIPSuffix";
            this.txtPublicIPSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtPublicIPSuffix.TabIndex = 9;
            this.txtPublicIPSuffix.Text = "-pip";
            this.txtPublicIPSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(46, 262);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 20);
            this.label9.TabIndex = 23;
            this.label9.Text = "Public IP:";
            // 
            // txtLoadBalancerSuffix
            // 
            this.txtLoadBalancerSuffix.Location = new System.Drawing.Point(711, 102);
            this.txtLoadBalancerSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtLoadBalancerSuffix.MaxLength = 10;
            this.txtLoadBalancerSuffix.Name = "txtLoadBalancerSuffix";
            this.txtLoadBalancerSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtLoadBalancerSuffix.TabIndex = 10;
            this.txtLoadBalancerSuffix.Text = "-lb";
            this.txtLoadBalancerSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(525, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 20);
            this.label8.TabIndex = 21;
            this.label8.Text = "Load Balancer:";
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(46, 228);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(133, 20);
            this.lblSuffix.TabIndex = 19;
            this.lblSuffix.Text = "Storage Account:";
            // 
            // txtStorageAccountSuffix
            // 
            this.txtStorageAccountSuffix.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtStorageAccountSuffix.Location = new System.Drawing.Point(243, 226);
            this.txtStorageAccountSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtStorageAccountSuffix.MaxLength = 10;
            this.txtStorageAccountSuffix.Name = "txtStorageAccountSuffix";
            this.txtStorageAccountSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtStorageAccountSuffix.TabIndex = 8;
            this.txtStorageAccountSuffix.Text = "v2";
            this.txtStorageAccountSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // txtVirtualNetworkSuffix
            // 
            this.txtVirtualNetworkSuffix.Location = new System.Drawing.Point(243, 132);
            this.txtVirtualNetworkSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtVirtualNetworkSuffix.MaxLength = 10;
            this.txtVirtualNetworkSuffix.Name = "txtVirtualNetworkSuffix";
            this.txtVirtualNetworkSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtVirtualNetworkSuffix.TabIndex = 5;
            this.txtVirtualNetworkSuffix.Text = "-vnet";
            this.txtVirtualNetworkSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Virtual Network:";
            // 
            // txtVirtualNetworkGatewaySuffix
            // 
            this.txtVirtualNetworkGatewaySuffix.Location = new System.Drawing.Point(243, 165);
            this.txtVirtualNetworkGatewaySuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtVirtualNetworkGatewaySuffix.MaxLength = 10;
            this.txtVirtualNetworkGatewaySuffix.Name = "txtVirtualNetworkGatewaySuffix";
            this.txtVirtualNetworkGatewaySuffix.Size = new System.Drawing.Size(58, 26);
            this.txtVirtualNetworkGatewaySuffix.TabIndex = 6;
            this.txtVirtualNetworkGatewaySuffix.Text = "-gw";
            this.txtVirtualNetworkGatewaySuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(46, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(187, 20);
            this.label6.TabIndex = 16;
            this.label6.Text = "Virtual Network Gateway:";
            // 
            // txtNetworkSecurityGroupSuffix
            // 
            this.txtNetworkSecurityGroupSuffix.Location = new System.Drawing.Point(243, 195);
            this.txtNetworkSecurityGroupSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtNetworkSecurityGroupSuffix.MaxLength = 10;
            this.txtNetworkSecurityGroupSuffix.Name = "txtNetworkSecurityGroupSuffix";
            this.txtNetworkSecurityGroupSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtNetworkSecurityGroupSuffix.TabIndex = 7;
            this.txtNetworkSecurityGroupSuffix.Text = "-nsg";
            this.txtNetworkSecurityGroupSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(181, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Network Security Group:";
            // 
            // txtResourceGroupSuffix
            // 
            this.txtResourceGroupSuffix.Location = new System.Drawing.Point(243, 98);
            this.txtResourceGroupSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtResourceGroupSuffix.MaxLength = 10;
            this.txtResourceGroupSuffix.Name = "txtResourceGroupSuffix";
            this.txtResourceGroupSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtResourceGroupSuffix.TabIndex = 4;
            this.txtResourceGroupSuffix.Text = "-rg";
            this.txtResourceGroupSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Resource Group:";
            // 
            // btnApplyDefaultNaming
            // 
            this.btnApplyDefaultNaming.Location = new System.Drawing.Point(530, 271);
            this.btnApplyDefaultNaming.Name = "btnApplyDefaultNaming";
            this.btnApplyDefaultNaming.Size = new System.Drawing.Size(310, 40);
            this.btnApplyDefaultNaming.TabIndex = 14;
            this.btnApplyDefaultNaming.Text = "Apply Default Naming Conventions";
            this.btnApplyDefaultNaming.UseVisualStyleBackColor = true;
            this.btnApplyDefaultNaming.Click += new System.EventHandler(this.btnApplyDefaultNaming_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(102, 42);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(567, 20);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "https://docs.microsoft.com/en-us/azure/guidance/guidance-naming-conventions";
            // 
            // txtAvailabilitySetSuffix
            // 
            this.txtAvailabilitySetSuffix.Location = new System.Drawing.Point(711, 131);
            this.txtAvailabilitySetSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtAvailabilitySetSuffix.MaxLength = 10;
            this.txtAvailabilitySetSuffix.Name = "txtAvailabilitySetSuffix";
            this.txtAvailabilitySetSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtAvailabilitySetSuffix.TabIndex = 11;
            this.txtAvailabilitySetSuffix.Text = "-as";
            this.txtAvailabilitySetSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(525, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Availability Set:";
            // 
            // txtVirtualMachineSuffix
            // 
            this.txtVirtualMachineSuffix.Location = new System.Drawing.Point(711, 163);
            this.txtVirtualMachineSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtVirtualMachineSuffix.MaxLength = 10;
            this.txtVirtualMachineSuffix.Name = "txtVirtualMachineSuffix";
            this.txtVirtualMachineSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtVirtualMachineSuffix.TabIndex = 12;
            this.txtVirtualMachineSuffix.Text = "-vm";
            this.txtVirtualMachineSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(525, 163);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "Virtual Machine:";
            // 
            // txtNetworkInterfaceCardSuffix
            // 
            this.txtNetworkInterfaceCardSuffix.Location = new System.Drawing.Point(711, 195);
            this.txtNetworkInterfaceCardSuffix.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtNetworkInterfaceCardSuffix.MaxLength = 10;
            this.txtNetworkInterfaceCardSuffix.Name = "txtNetworkInterfaceCardSuffix";
            this.txtNetworkInterfaceCardSuffix.Size = new System.Drawing.Size(58, 26);
            this.txtNetworkInterfaceCardSuffix.TabIndex = 13;
            this.txtNetworkInterfaceCardSuffix.Text = "-nic";
            this.txtNetworkInterfaceCardSuffix.TextChanged += new System.EventHandler(this.migAzOption_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(525, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Network Interface Card:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.cmbLoginPromptBehavior);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.cmbDefaultAzureEnvironment);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.upDownAccessSASMinutes);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.chkSaveSelection);
            this.groupBox2.Controls.Add(this.chkAutoSelectDependencies);
            this.groupBox2.Controls.Add(this.chkBuildEmpty);
            this.groupBox2.Controls.Add(this.chkAllowTelemetry);
            this.groupBox2.Location = new System.Drawing.Point(20, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(854, 292);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "MigAz Options";
            // 
            // cmbLoginPromptBehavior
            // 
            this.cmbLoginPromptBehavior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoginPromptBehavior.FormattingEnabled = true;
            this.cmbLoginPromptBehavior.Items.AddRange(new object[] {
            "Always",
            "Auto",
            "SelectAccount"});
            this.cmbLoginPromptBehavior.Location = new System.Drawing.Point(129, 222);
            this.cmbLoginPromptBehavior.Name = "cmbLoginPromptBehavior";
            this.cmbLoginPromptBehavior.Size = new System.Drawing.Size(149, 28);
            this.cmbLoginPromptBehavior.TabIndex = 13;
            this.cmbLoginPromptBehavior.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(35, 187);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(303, 20);
            this.label14.TabIndex = 12;
            this.label14.Text = "Default Azure AD Prompt Behavior";
            // 
            // cmbDefaultAzureEnvironment
            // 
            this.cmbDefaultAzureEnvironment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultAzureEnvironment.FormattingEnabled = true;
            this.cmbDefaultAzureEnvironment.Items.AddRange(new object[] {
            "AzureCloud",
            "AzureGermanCloud",
            "AzureChinaCloud",
            "AzureUSGovernment"});
            this.cmbDefaultAzureEnvironment.Location = new System.Drawing.Point(530, 57);
            this.cmbDefaultAzureEnvironment.Name = "cmbDefaultAzureEnvironment";
            this.cmbDefaultAzureEnvironment.Size = new System.Drawing.Size(273, 28);
            this.cmbDefaultAzureEnvironment.TabIndex = 11;
            this.cmbDefaultAzureEnvironment.SelectedIndexChanged += new System.EventHandler(this.comboBox_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(482, 22);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(235, 20);
            this.label13.TabIndex = 10;
            this.label13.Text = "Default Azure Environment";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(482, 108);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(220, 20);
            this.label12.TabIndex = 9;
            this.label12.Text = "Default Target Disk Type";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(482, 217);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(316, 20);
            this.label11.TabIndex = 8;
            this.label11.Text = "Managed Disk Access SAS Duration";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(618, 254);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 20);
            this.label10.TabIndex = 7;
            this.label10.Text = "Minutes";
            // 
            // upDownAccessSASMinutes
            // 
            this.upDownAccessSASMinutes.Location = new System.Drawing.Point(534, 248);
            this.upDownAccessSASMinutes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upDownAccessSASMinutes.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.upDownAccessSASMinutes.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.upDownAccessSASMinutes.Name = "upDownAccessSASMinutes";
            this.upDownAccessSASMinutes.Size = new System.Drawing.Size(75, 26);
            this.upDownAccessSASMinutes.TabIndex = 6;
            this.upDownAccessSASMinutes.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.upDownAccessSASMinutes.ValueChanged += new System.EventHandler(this.upDownAccessSASMinutes_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbManagedDisk);
            this.panel1.Controls.Add(this.rbClassicDisk);
            this.panel1.Location = new System.Drawing.Point(530, 142);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 65);
            this.panel1.TabIndex = 5;
            // 
            // rbManagedDisk
            // 
            this.rbManagedDisk.AutoSize = true;
            this.rbManagedDisk.Location = new System.Drawing.Point(4, 5);
            this.rbManagedDisk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbManagedDisk.Name = "rbManagedDisk";
            this.rbManagedDisk.Size = new System.Drawing.Size(259, 24);
            this.rbManagedDisk.TabIndex = 3;
            this.rbManagedDisk.TabStop = true;
            this.rbManagedDisk.Text = "Managed Disk (Recommended)";
            this.rbManagedDisk.UseVisualStyleBackColor = true;
            this.rbManagedDisk.CheckedChanged += new System.EventHandler(this.migAzOption_CheckChanged);
            // 
            // rbClassicDisk
            // 
            this.rbClassicDisk.AutoSize = true;
            this.rbClassicDisk.Location = new System.Drawing.Point(4, 34);
            this.rbClassicDisk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rbClassicDisk.Name = "rbClassicDisk";
            this.rbClassicDisk.Size = new System.Drawing.Size(119, 24);
            this.rbClassicDisk.TabIndex = 2;
            this.rbClassicDisk.TabStop = true;
            this.rbClassicDisk.Text = "Classic Disk";
            this.rbClassicDisk.UseVisualStyleBackColor = true;
            this.rbClassicDisk.CheckedChanged += new System.EventHandler(this.migAzOption_CheckChanged);
            // 
            // chkSaveSelection
            // 
            this.chkSaveSelection.AutoSize = true;
            this.chkSaveSelection.Location = new System.Drawing.Point(39, 75);
            this.chkSaveSelection.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.chkSaveSelection.Name = "chkSaveSelection";
            this.chkSaveSelection.Size = new System.Drawing.Size(138, 24);
            this.chkSaveSelection.TabIndex = 2;
            this.chkSaveSelection.Text = "Save selection";
            this.chkSaveSelection.UseVisualStyleBackColor = true;
            this.chkSaveSelection.CheckedChanged += new System.EventHandler(this.migAzOption_CheckedChanged);
            // 
            // chkAutoSelectDependencies
            // 
            this.chkAutoSelectDependencies.AutoSize = true;
            this.chkAutoSelectDependencies.Location = new System.Drawing.Point(39, 43);
            this.chkAutoSelectDependencies.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.chkAutoSelectDependencies.Name = "chkAutoSelectDependencies";
            this.chkAutoSelectDependencies.Size = new System.Drawing.Size(320, 24);
            this.chkAutoSelectDependencies.TabIndex = 1;
            this.chkAutoSelectDependencies.Text = "Auto select dependencies (for VMs only)";
            this.chkAutoSelectDependencies.UseVisualStyleBackColor = true;
            this.chkAutoSelectDependencies.CheckedChanged += new System.EventHandler(this.migAzOption_CheckedChanged);
            // 
            // chkBuildEmpty
            // 
            this.chkBuildEmpty.AutoSize = true;
            this.chkBuildEmpty.Checked = true;
            this.chkBuildEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBuildEmpty.Location = new System.Drawing.Point(39, 108);
            this.chkBuildEmpty.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.chkBuildEmpty.Name = "chkBuildEmpty";
            this.chkBuildEmpty.Size = new System.Drawing.Size(208, 24);
            this.chkBuildEmpty.TabIndex = 0;
            this.chkBuildEmpty.Text = "Build empty environment";
            this.chkBuildEmpty.UseVisualStyleBackColor = true;
            this.chkBuildEmpty.CheckedChanged += new System.EventHandler(this.migAzOption_CheckedChanged);
            // 
            // chkAllowTelemetry
            // 
            this.chkAllowTelemetry.AutoSize = true;
            this.chkAllowTelemetry.Checked = true;
            this.chkAllowTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowTelemetry.Location = new System.Drawing.Point(39, 138);
            this.chkAllowTelemetry.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.chkAllowTelemetry.Name = "chkAllowTelemetry";
            this.chkAllowTelemetry.Size = new System.Drawing.Size(211, 24);
            this.chkAllowTelemetry.TabIndex = 3;
            this.chkAllowTelemetry.Text = "Allow telemetry collection";
            this.chkAllowTelemetry.UseVisualStyleBackColor = true;
            this.chkAllowTelemetry.CheckedChanged += new System.EventHandler(this.chkAllowTelemetry_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(68, 225);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(52, 20);
            this.label15.TabIndex = 14;
            this.label15.Text = "Login:";
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(893, 755);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsDialog_FormClosing);
            this.Load += new System.EventHandler(this.formOptions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownAccessSASMinutes)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtNetworkInterfaceCardSuffix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSaveSelection;
        private System.Windows.Forms.CheckBox chkAutoSelectDependencies;
        private System.Windows.Forms.CheckBox chkBuildEmpty;
        private System.Windows.Forms.CheckBox chkAllowTelemetry;
        private System.Windows.Forms.TextBox txtVirtualMachineSuffix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAvailabilitySetSuffix;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnApplyDefaultNaming;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox txtPublicIPSuffix;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLoadBalancerSuffix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox txtStorageAccountSuffix;
        private System.Windows.Forms.TextBox txtVirtualNetworkSuffix;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtVirtualNetworkGatewaySuffix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNetworkSecurityGroupSuffix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtResourceGroupSuffix;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown upDownAccessSASMinutes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbManagedDisk;
        private System.Windows.Forms.RadioButton rbClassicDisk;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbDefaultAzureEnvironment;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbLoginPromptBehavior;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
    }
}