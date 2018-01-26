namespace MigAz.AWS.UserControls
{
    partial class MigrationAWSSourceContext
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
            this.treeSourceAWS = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.secretKeyTextBox = new System.Windows.Forms.TextBox();
            this.accessKeyTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeSourceAWS
            // 
            this.treeSourceAWS.CheckBoxes = true;
            this.treeSourceAWS.Location = new System.Drawing.Point(3, 183);
            this.treeSourceAWS.Name = "treeSourceAWS";
            this.treeSourceAWS.Size = new System.Drawing.Size(541, 371);
            this.treeSourceAWS.TabIndex = 59;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.secretKeyTextBox);
            this.groupBox1.Controls.Add(this.accessKeyTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(540, 165);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AWS Account";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(225, 123);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(129, 35);
            this.btnLogin.TabIndex = 22;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // secretKeyTextBox
            // 
            this.secretKeyTextBox.Location = new System.Drawing.Point(225, 78);
            this.secretKeyTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.secretKeyTextBox.Name = "secretKeyTextBox";
            this.secretKeyTextBox.PasswordChar = '*';
            this.secretKeyTextBox.Size = new System.Drawing.Size(298, 26);
            this.secretKeyTextBox.TabIndex = 26;
            // 
            // accessKeyTextBox
            // 
            this.accessKeyTextBox.AllowDrop = true;
            this.accessKeyTextBox.Location = new System.Drawing.Point(225, 37);
            this.accessKeyTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.accessKeyTextBox.Name = "accessKeyTextBox";
            this.accessKeyTextBox.Size = new System.Drawing.Size(298, 26);
            this.accessKeyTextBox.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 20);
            this.label2.TabIndex = 24;
            this.label2.Text = "AWS Secret Access Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 20);
            this.label5.TabIndex = 23;
            this.label5.Text = "AWS Access Key ID";
            // 
            // MigrationAWSSourceContext
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeSourceAWS);
            this.Name = "MigrationAWSSourceContext";
            this.Size = new System.Drawing.Size(551, 557);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeSourceAWS;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox secretKeyTextBox;
        private System.Windows.Forms.TextBox accessKeyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
    }
}
