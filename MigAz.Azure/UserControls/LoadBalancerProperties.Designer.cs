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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rbExistingARMVNet = new System.Windows.Forms.RadioButton();
            this.rbVNetInMigration = new System.Windows.Forms.RadioButton();
            this.cmbExistingArmSubnet = new System.Windows.Forms.ComboBox();
            this.cmbExistingArmVNets = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Target Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(176, 19);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(328, 31);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 25);
            this.label8.TabIndex = 36;
            this.label8.Text = "Target Subnet:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 25);
            this.label7.TabIndex = 35;
            this.label7.Text = "Target VNet:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 25);
            this.label1.TabIndex = 34;
            this.label1.Text = "Migrate To:";
            // 
            // rbExistingARMVNet
            // 
            this.rbExistingARMVNet.AutoSize = true;
            this.rbExistingARMVNet.Location = new System.Drawing.Point(176, 110);
            this.rbExistingARMVNet.Name = "rbExistingARMVNet";
            this.rbExistingARMVNet.Size = new System.Drawing.Size(316, 29);
            this.rbExistingARMVNet.TabIndex = 31;
            this.rbExistingARMVNet.Text = "Existing ARM VNet in Target";
            this.rbExistingARMVNet.UseVisualStyleBackColor = true;
            this.rbExistingARMVNet.CheckedChanged += new System.EventHandler(this.rbExistingARMVNet_CheckedChanged);
            // 
            // rbVNetInMigration
            // 
            this.rbVNetInMigration.AutoSize = true;
            this.rbVNetInMigration.Location = new System.Drawing.Point(176, 66);
            this.rbVNetInMigration.Name = "rbVNetInMigration";
            this.rbVNetInMigration.Size = new System.Drawing.Size(274, 29);
            this.rbVNetInMigration.TabIndex = 30;
            this.rbVNetInMigration.Text = "VNet in MigAz Migration";
            this.rbVNetInMigration.UseVisualStyleBackColor = true;
            this.rbVNetInMigration.CheckedChanged += new System.EventHandler(this.rbVNetInMigration_CheckedChanged);
            // 
            // cmbExistingArmSubnet
            // 
            this.cmbExistingArmSubnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmSubnet.FormattingEnabled = true;
            this.cmbExistingArmSubnet.Location = new System.Drawing.Point(176, 212);
            this.cmbExistingArmSubnet.Name = "cmbExistingArmSubnet";
            this.cmbExistingArmSubnet.Size = new System.Drawing.Size(429, 33);
            this.cmbExistingArmSubnet.TabIndex = 33;
            this.cmbExistingArmSubnet.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmSubnet_SelectedIndexChanged);
            // 
            // cmbExistingArmVNets
            // 
            this.cmbExistingArmVNets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExistingArmVNets.FormattingEnabled = true;
            this.cmbExistingArmVNets.Location = new System.Drawing.Point(176, 162);
            this.cmbExistingArmVNets.Name = "cmbExistingArmVNets";
            this.cmbExistingArmVNets.Size = new System.Drawing.Size(429, 33);
            this.cmbExistingArmVNets.TabIndex = 32;
            this.cmbExistingArmVNets.SelectedIndexChanged += new System.EventHandler(this.cmbExistingArmVNets_SelectedIndexChanged);
            // 
            // LoadBalancerProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbExistingARMVNet);
            this.Controls.Add(this.rbVNetInMigration);
            this.Controls.Add(this.cmbExistingArmSubnet);
            this.Controls.Add(this.cmbExistingArmVNets);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Name = "LoadBalancerProperties";
            this.Size = new System.Drawing.Size(621, 273);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbExistingARMVNet;
        private System.Windows.Forms.RadioButton rbVNetInMigration;
        private System.Windows.Forms.ComboBox cmbExistingArmSubnet;
        private System.Windows.Forms.ComboBox cmbExistingArmVNets;
    }
}
