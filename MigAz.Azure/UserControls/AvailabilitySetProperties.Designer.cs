// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class AvailabilitySetProperties
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
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.upDownUpdateDomains = new System.Windows.Forms.NumericUpDown();
            this.upDownFaultDomains = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownUpdateDomains)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownFaultDomains)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(99, 5);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(166, 20);
            this.txtTargetName.TabIndex = 0;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target Name:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(11, 96);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(299, 350);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 41;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Update Domains:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Fault Domains:";
            // 
            // upDownUpdateDomains
            // 
            this.upDownUpdateDomains.Location = new System.Drawing.Point(99, 31);
            this.upDownUpdateDomains.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.upDownUpdateDomains.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upDownUpdateDomains.Name = "upDownUpdateDomains";
            this.upDownUpdateDomains.Size = new System.Drawing.Size(44, 20);
            this.upDownUpdateDomains.TabIndex = 46;
            this.upDownUpdateDomains.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upDownUpdateDomains.ValueChanged += new System.EventHandler(this.upDownUpdateDomains_ValueChanged);
            // 
            // upDownFaultDomains
            // 
            this.upDownFaultDomains.Location = new System.Drawing.Point(99, 57);
            this.upDownFaultDomains.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.upDownFaultDomains.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.upDownFaultDomains.Name = "upDownFaultDomains";
            this.upDownFaultDomains.Size = new System.Drawing.Size(44, 20);
            this.upDownFaultDomains.TabIndex = 47;
            this.upDownFaultDomains.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.upDownFaultDomains.ValueChanged += new System.EventHandler(this.upDownFaultDomains_ValueChanged);
            // 
            // AvailabilitySetProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.upDownFaultDomains);
            this.Controls.Add(this.upDownUpdateDomains);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTargetName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AvailabilitySetProperties";
            this.Size = new System.Drawing.Size(320, 452);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownUpdateDomains)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownFaultDomains)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown upDownUpdateDomains;
        private System.Windows.Forms.NumericUpDown upDownFaultDomains;
    }
}

