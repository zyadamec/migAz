namespace MIGAZ.Forms
{
    partial class formOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formOptions));
            this.lblSuffix = new System.Windows.Forms.Label();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.chkAllowTelemetry = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.chkBuildEmpty = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAutoSelectDependencies = new System.Windows.Forms.CheckBox();
            this.chkSaveSelection = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblSuffix
            // 
            this.lblSuffix.AutoSize = true;
            this.lblSuffix.Location = new System.Drawing.Point(29, 18);
            this.lblSuffix.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(123, 17);
            this.lblSuffix.TabIndex = 2;
            this.lblSuffix.Text = "Uniqueness suffix:";
            // 
            // txtSuffix
            // 
            this.txtSuffix.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSuffix.Location = new System.Drawing.Point(178, 15);
            this.txtSuffix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(52, 22);
            this.txtSuffix.TabIndex = 3;
            this.txtSuffix.Text = "v2";
            this.txtSuffix.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuffix_KeyPress);
            // 
            // chkAllowTelemetry
            // 
            this.chkAllowTelemetry.AutoSize = true;
            this.chkAllowTelemetry.Checked = true;
            this.chkAllowTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowTelemetry.Location = new System.Drawing.Point(33, 141);
            this.chkAllowTelemetry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAllowTelemetry.Name = "chkAllowTelemetry";
            this.chkAllowTelemetry.Size = new System.Drawing.Size(187, 21);
            this.chkAllowTelemetry.TabIndex = 5;
            this.chkAllowTelemetry.Text = "Allow telemetry collection";
            this.chkAllowTelemetry.UseVisualStyleBackColor = true;
            this.chkAllowTelemetry.CheckedChanged += new System.EventHandler(this.chkAllowTelemetry_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(178, 188);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 28);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chkBuildEmpty
            // 
            this.chkBuildEmpty.AutoSize = true;
            this.chkBuildEmpty.Checked = true;
            this.chkBuildEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBuildEmpty.Location = new System.Drawing.Point(33, 59);
            this.chkBuildEmpty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBuildEmpty.Name = "chkBuildEmpty";
            this.chkBuildEmpty.Size = new System.Drawing.Size(185, 21);
            this.chkBuildEmpty.TabIndex = 4;
            this.chkBuildEmpty.Text = "Build empty environment";
            this.chkBuildEmpty.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(294, 188);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkAutoSelectDependencies
            // 
            this.chkAutoSelectDependencies.AutoSize = true;
            this.chkAutoSelectDependencies.Location = new System.Drawing.Point(33, 87);
            this.chkAutoSelectDependencies.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkAutoSelectDependencies.Name = "chkAutoSelectDependencies";
            this.chkAutoSelectDependencies.Size = new System.Drawing.Size(285, 21);
            this.chkAutoSelectDependencies.TabIndex = 8;
            this.chkAutoSelectDependencies.Text = "Auto select dependencies (for VMs only)";
            this.chkAutoSelectDependencies.UseVisualStyleBackColor = true;
            // 
            // chkSaveSelection
            // 
            this.chkSaveSelection.AutoSize = true;
            this.chkSaveSelection.Location = new System.Drawing.Point(33, 114);
            this.chkSaveSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSaveSelection.Name = "chkSaveSelection";
            this.chkSaveSelection.Size = new System.Drawing.Size(122, 21);
            this.chkSaveSelection.TabIndex = 9;
            this.chkSaveSelection.Text = "Save selection";
            this.chkSaveSelection.UseVisualStyleBackColor = true;
            // 
            // formOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(416, 236);
            this.Controls.Add(this.chkSaveSelection);
            this.Controls.Add(this.chkAutoSelectDependencies);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.chkBuildEmpty);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblSuffix);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.chkAllowTelemetry);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formOptions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.formOptions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkAllowTelemetry;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkBuildEmpty;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkAutoSelectDependencies;
        private System.Windows.Forms.CheckBox chkSaveSelection;
    }
}