namespace MigAz.Azure.UserControls
{
    partial class ResourceGroupProperties
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.cboTargetLocation = new System.Windows.Forms.ComboBox();
            this.lblTargetContext = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Location:";
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(98, 16);
            this.txtTargetName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(293, 26);
            this.txtTargetName.TabIndex = 0;
            this.txtTargetName.TextChanged += new System.EventHandler(this.txtTargetName_TextChanged);
            this.txtTargetName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTargetName_KeyPress);
            // 
            // cboTargetLocation
            // 
            this.cboTargetLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTargetLocation.FormattingEnabled = true;
            this.cboTargetLocation.Location = new System.Drawing.Point(98, 52);
            this.cboTargetLocation.Name = "cboTargetLocation";
            this.cboTargetLocation.Size = new System.Drawing.Size(293, 28);
            this.cboTargetLocation.TabIndex = 1;
            this.cboTargetLocation.SelectedIndexChanged += new System.EventHandler(this.cboTargetLocation_SelectedIndexChanged);
            // 
            // lblTargetContext
            // 
            this.lblTargetContext.AutoSize = true;
            this.lblTargetContext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetContext.Location = new System.Drawing.Point(104, 55);
            this.lblTargetContext.Name = "lblTargetContext";
            this.lblTargetContext.Size = new System.Drawing.Size(260, 20);
            this.lblTargetContext.TabIndex = 2;
            this.lblTargetContext.Text = "<Set Migration Target Context>";
            this.lblTargetContext.Visible = false;
            // 
            // ResourceGroupProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTargetContext);
            this.Controls.Add(this.cboTargetLocation);
            this.Controls.Add(this.txtTargetName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ResourceGroupProperties";
            this.Size = new System.Drawing.Size(469, 107);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.ComboBox cboTargetLocation;
        private System.Windows.Forms.Label lblTargetContext;
    }
}
