namespace MIGAZ.UserControls
{
    partial class StorageAccountProperties
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
            this.lblAccountType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSourceASMName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTargetName
            // 
            this.txtTargetName.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtTargetName.Location = new System.Drawing.Point(230, 94);
            this.txtTargetName.MaxLength = 24;
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(390, 31);
            this.txtTargetName.TabIndex = 7;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Target (ARM) Name:";
            // 
            // lblAccountType
            // 
            this.lblAccountType.AutoSize = true;
            this.lblAccountType.Location = new System.Drawing.Point(230, 10);
            this.lblAccountType.Name = "lblAccountType";
            this.lblAccountType.Size = new System.Drawing.Size(70, 25);
            this.lblAccountType.TabIndex = 5;
            this.lblAccountType.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Account Type:";
            // 
            // lblSourceASMName
            // 
            this.lblSourceASMName.AutoSize = true;
            this.lblSourceASMName.Location = new System.Drawing.Point(230, 51);
            this.lblSourceASMName.Name = "lblSourceASMName";
            this.lblSourceASMName.Size = new System.Drawing.Size(114, 25);
            this.lblSourceASMName.TabIndex = 9;
            this.lblSourceASMName.Text = "ASMName";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(214, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Source (ASM) Name:";
            // 
            // StorageAccountProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblSourceASMName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblAccountType);
            this.Controls.Add(this.label1);
            this.Name = "StorageAccountProperties";
            this.Size = new System.Drawing.Size(625, 140);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAccountType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSourceASMName;
        private System.Windows.Forms.Label label4;
    }
}
