// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MIGAZ.Forms
{
    partial class AuthenticationForm
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
            this.cancelButton = new System.Windows.Forms.Button();
            this.continueButton = new System.Windows.Forms.Button();
            this.secretKeyTextBox = new System.Windows.Forms.TextBox();
            this.accessKeyTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(432, 132);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(130, 35);
            this.cancelButton.TabIndex = 23;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // continueButton
            // 
            this.continueButton.Location = new System.Drawing.Point(266, 132);
            this.continueButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(130, 35);
            this.continueButton.TabIndex = 1;
            this.continueButton.Text = "Continue";
            this.continueButton.UseVisualStyleBackColor = true;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // secretKeyTextBox
            // 
            this.secretKeyTextBox.Location = new System.Drawing.Point(264, 84);
            this.secretKeyTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.secretKeyTextBox.Name = "secretKeyTextBox";
            this.secretKeyTextBox.Size = new System.Drawing.Size(298, 26);
            this.secretKeyTextBox.TabIndex = 21;
            // 
            // accessKeyTextBox
            // 
            this.accessKeyTextBox.Location = new System.Drawing.Point(264, 42);
            this.accessKeyTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.accessKeyTextBox.Name = "accessKeyTextBox";
            this.accessKeyTextBox.Size = new System.Drawing.Size(298, 26);
            this.accessKeyTextBox.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "AWS Secret Access Key";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(72, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 20);
            this.label1.TabIndex = 18;
            this.label1.Text = "AWS Access Key ID";
            // 
            // AuthenticationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 206);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.secretKeyTextBox);
            this.Controls.Add(this.accessKeyTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "AuthenticationForm";
            this.Text = "AuthenticationForm";
            this.Load += new System.EventHandler(this.AuthenticationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button continueButton;
        private System.Windows.Forms.TextBox secretKeyTextBox;
        private System.Windows.Forms.TextBox accessKeyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

