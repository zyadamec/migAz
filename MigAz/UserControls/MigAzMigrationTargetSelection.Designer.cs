namespace MigAz.UserControls
{
    partial class MigAzMigrationTargetSelection
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAzureStack = new System.Windows.Forms.Button();
            this.btnAzure = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnAzureStack);
            this.groupBox1.Controls.Add(this.btnAzure);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 128);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Migration Target";
            // 
            // btnAzureStack
            // 
            this.btnAzureStack.Enabled = false;
            this.btnAzureStack.Location = new System.Drawing.Point(199, 73);
            this.btnAzureStack.Name = "btnAzureStack";
            this.btnAzureStack.Size = new System.Drawing.Size(124, 39);
            this.btnAzureStack.TabIndex = 1;
            this.btnAzureStack.Text = "Azure Stack";
            this.btnAzureStack.UseVisualStyleBackColor = true;
            this.btnAzureStack.Click += new System.EventHandler(this.btnAzureStack_Click);
            // 
            // btnAzure
            // 
            this.btnAzure.Enabled = false;
            this.btnAzure.Location = new System.Drawing.Point(62, 73);
            this.btnAzure.Name = "btnAzure";
            this.btnAzure.Size = new System.Drawing.Size(117, 39);
            this.btnAzure.TabIndex = 0;
            this.btnAzure.Text = "Azure";
            this.btnAzure.UseVisualStyleBackColor = true;
            this.btnAzure.Click += new System.EventHandler(this.btnAzure_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(325, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Migration Source prior to Target.";
            // 
            // MigAzMigrationTargetSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "MigAzMigrationTargetSelection";
            this.Size = new System.Drawing.Size(364, 133);
            this.Resize += new System.EventHandler(this.MigAzMigrationTargetSelection_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAzureStack;
        private System.Windows.Forms.Button btnAzure;
        private System.Windows.Forms.Label label1;
    }
}
