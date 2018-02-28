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
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 67);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(116, 65);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(161, 20);
            this.txtTargetName.TabIndex = 3;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Source Name:";
            // 
            // lblSourceName
            // 
            this.lblSourceName.AutoSize = true;
            this.lblSourceName.Location = new System.Drawing.Point(114, 5);
            this.lblSourceName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSourceName.Name = "lblSourceName";
            this.lblSourceName.Size = new System.Drawing.Size(35, 13);
            this.lblSourceName.TabIndex = 6;
            this.lblSourceName.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 24);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Address Space:";
            // 
            // lblAddressSpace
            // 
            this.lblAddressSpace.AutoSize = true;
            this.lblAddressSpace.Location = new System.Drawing.Point(114, 24);
            this.lblAddressSpace.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddressSpace.Name = "lblAddressSpace";
            this.lblAddressSpace.Size = new System.Drawing.Size(63, 13);
            this.lblAddressSpace.TabIndex = 8;
            this.lblAddressSpace.Text = "10.0.0.0/24";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 89);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 49;
            this.label4.Text = "Network Security Group:";
            // 
            // networkSecurityGroup
            // 
            this.networkSecurityGroup.AutoSize = true;
            this.networkSecurityGroup.Location = new System.Drawing.Point(128, 87);
            this.networkSecurityGroup.Name = "networkSecurityGroup";
            this.networkSecurityGroup.Size = new System.Drawing.Size(71, 28);
            this.networkSecurityGroup.TabIndex = 48;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(2, 43);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 50;
            this.label7.Text = "Target";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 110);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 52;
            this.label5.Text = "Route Table:";
            // 
            // routeTable
            // 
            this.routeTable.AutoSize = true;
            this.routeTable.Location = new System.Drawing.Point(128, 108);
            this.routeTable.Name = "routeTable";
            this.routeTable.Size = new System.Drawing.Size(71, 28);
            this.routeTable.TabIndex = 51;
            // 
            // SubnetProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SubnetProperties";
            this.Size = new System.Drawing.Size(312, 156);
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
        private ResourceSummary networkSecurityGroup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private ResourceSummary routeTable;
    }
}

