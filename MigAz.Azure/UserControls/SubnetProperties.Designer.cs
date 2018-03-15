// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class SubnetProperties
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
            this.lblSourceName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAddressSpace = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.networkSecurityGroup = new MigAz.Azure.UserControls.ResourceSummary();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.routeTable = new MigAz.Azure.UserControls.ResourceSummary();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAddressSpace = new MigAz.Azure.UserControls.IPv4AddressBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(174, 100);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(240, 26);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Source Name:";
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(171, 8);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(51, 20);
            this.lblSourceName.TabIndex = 6;
            this.lblSourceName.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Address Space:";
            // 
            // lblAddressSpace
            // 
            this.lblAddressSpace.AutoSize = true;
            this.lblAddressSpace.Location = new System.Drawing.Point(171, 37);
            this.lblAddressSpace.Name = "lblAddressSpace";
            this.lblAddressSpace.Size = new System.Drawing.Size(88, 20);
            this.lblAddressSpace.TabIndex = 8;
            this.lblAddressSpace.Text = "10.0.0.0/24";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 20);
            this.label4.TabIndex = 49;
            this.label4.Text = "Network Security Group:";
            // 
            // networkSecurityGroup
            // 
            this.networkSecurityGroup.AutoSize = true;
            this.networkSecurityGroup.Location = new System.Drawing.Point(192, 169);
            this.networkSecurityGroup.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.networkSecurityGroup.Name = "networkSecurityGroup";
            this.networkSecurityGroup.Size = new System.Drawing.Size(287, 43);
            this.networkSecurityGroup.TabIndex = 48;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 20);
            this.label7.TabIndex = 50;
            this.label7.Text = "Target";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 52;
            this.label5.Text = "Route Table:";
            // 
            // routeTable
            // 
            this.routeTable.AutoSize = true;
            this.routeTable.Location = new System.Drawing.Point(192, 201);
            this.routeTable.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.routeTable.Name = "routeTable";
            this.routeTable.Size = new System.Drawing.Size(287, 43);
            this.routeTable.TabIndex = 51;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 20);
            this.label6.TabIndex = 54;
            this.label6.Text = "Address Space:";
            // 
            // txtAddressSpace
            // 
            this.txtAddressSpace.Enabled = false;
            this.txtAddressSpace.Location = new System.Drawing.Point(175, 133);
            this.txtAddressSpace.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAddressSpace.Name = "txtAddressSpace";
            this.txtAddressSpace.Size = new System.Drawing.Size(150, 31);
            this.txtAddressSpace.TabIndex = 55;
            // 
            // SubnetProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtAddressSpace);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.routeTable);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.networkSecurityGroup);
            this.Controls.Add(this.lblAddressSpace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblSourceName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Name = "SubnetProperties";
            this.Size = new System.Drawing.Size(468, 339);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSourceName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAddressSpace;
        private System.Windows.Forms.Label label4;
        private MigAz.Azure.UserControls.ResourceSummary networkSecurityGroup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private MigAz.Azure.UserControls.ResourceSummary routeTable;
        private System.Windows.Forms.Label label6;
        private IPv4AddressBox txtAddressSpace;
    }
}

