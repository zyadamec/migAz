namespace MigAz.Forms
{
    partial class ExportResultsDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnGenerateInstructions = new System.Windows.Forms.Button();
            this.btnViewTemplate = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(788, 562);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(188, 52);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnGenerateInstructions
            // 
            this.btnGenerateInstructions.Location = new System.Drawing.Point(299, 403);
            this.btnGenerateInstructions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnGenerateInstructions.Name = "btnGenerateInstructions";
            this.btnGenerateInstructions.Size = new System.Drawing.Size(267, 59);
            this.btnGenerateInstructions.TabIndex = 14;
            this.btnGenerateInstructions.Text = "Show Instructions";
            this.btnGenerateInstructions.UseVisualStyleBackColor = true;
            this.btnGenerateInstructions.Click += new System.EventHandler(this.btnGenerateInstructions_Click_1);
            // 
            // btnViewTemplate
            // 
            this.btnViewTemplate.Location = new System.Drawing.Point(29, 405);
            this.btnViewTemplate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnViewTemplate.Name = "btnViewTemplate";
            this.btnViewTemplate.Size = new System.Drawing.Size(225, 55);
            this.btnViewTemplate.TabIndex = 13;
            this.btnViewTemplate.Text = "View Template";
            this.btnViewTemplate.UseVisualStyleBackColor = true;
            this.btnViewTemplate.Click += new System.EventHandler(this.btnViewTemplate_Click_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MigAz.Properties.Resources.Resource_group;
            this.pictureBox1.Location = new System.Drawing.Point(15, 16);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 134);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(176, 70);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(757, 25);
            this.label7.TabIndex = 10;
            this.label7.Text = "Click the Next Steps tab to learn how to deploy it to Azure Resource Manager.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(486, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Your template has been generated successfully!  ";
            // 
            // ExportResultsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 638);
            this.Controls.Add(this.btnGenerateInstructions);
            this.Controls.Add(this.btnViewTemplate);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportResultsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Complete";
            this.Load += new System.EventHandler(this.ExportResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnGenerateInstructions;
        private System.Windows.Forms.Button btnViewTemplate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
    }
}