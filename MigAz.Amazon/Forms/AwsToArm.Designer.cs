namespace MigAz.AWS.Forms
{
    partial class AwsToArm
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
            this.azureLoginContextViewer21 = new MigAz.Azure.UserControls.AzureLoginContextViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.secretKeyTextBox = new System.Windows.Forms.TextBox();
            this.accessKeyTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.treeSource = new System.Windows.Forms.TreeView();
            this.treeTargetARM = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // azureLoginContextViewer21
            // 
            this.azureLoginContextViewer21.ChangeType = MigAz.Azure.UserControls.AzureLoginChangeType.Full;
            this.azureLoginContextViewer21.Location = new System.Drawing.Point(741, 16);
            this.azureLoginContextViewer21.Name = "azureLoginContextViewer21";
            this.azureLoginContextViewer21.Size = new System.Drawing.Size(894, 211);
            this.azureLoginContextViewer21.TabIndex = 53;
            this.azureLoginContextViewer21.Title = "Azure Subscription";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.secretKeyTextBox);
            this.groupBox1.Controls.Add(this.accessKeyTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(719, 205);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AWS Account";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(301, 153);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(173, 44);
            this.btnLogin.TabIndex = 22;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // secretKeyTextBox
            // 
            this.secretKeyTextBox.Location = new System.Drawing.Point(301, 99);
            this.secretKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.secretKeyTextBox.Name = "secretKeyTextBox";
            this.secretKeyTextBox.PasswordChar = '*';
            this.secretKeyTextBox.Size = new System.Drawing.Size(396, 31);
            this.secretKeyTextBox.TabIndex = 26;
            // 
            // accessKeyTextBox
            // 
            this.accessKeyTextBox.Location = new System.Drawing.Point(301, 46);
            this.accessKeyTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.accessKeyTextBox.Name = "accessKeyTextBox";
            this.accessKeyTextBox.Size = new System.Drawing.Size(396, 31);
            this.accessKeyTextBox.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 99);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 25);
            this.label2.TabIndex = 24;
            this.label2.Text = "AWS Secret Access Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 46);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(205, 25);
            this.label5.TabIndex = 23;
            this.label5.Text = "AWS Access Key ID";
            // 
            // treeSource
            // 
            this.treeSource.CheckBoxes = true;
            this.treeSource.Location = new System.Drawing.Point(3, 240);
            this.treeSource.Name = "treeSource";
            this.treeSource.Size = new System.Drawing.Size(719, 479);
            this.treeSource.TabIndex = 58;
            // 
            // treeTargetARM
            // 
            this.treeTargetARM.Location = new System.Drawing.Point(741, 233);
            this.treeTargetARM.Name = "treeTargetARM";
            this.treeTargetARM.Size = new System.Drawing.Size(884, 486);
            this.treeTargetARM.TabIndex = 59;
            // 
            // AwsToArm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeTargetARM);
            this.Controls.Add(this.treeSource);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.azureLoginContextViewer21);
            this.Name = "AwsToArm";
            this.Size = new System.Drawing.Size(1818, 1032);
            this.Load += new System.EventHandler(this.AwsToArm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Azure.UserControls.AzureLoginContextViewer azureLoginContextViewer21;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox secretKeyTextBox;
        private System.Windows.Forms.TextBox accessKeyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView treeSource;
        private System.Windows.Forms.TreeView treeTargetARM;
    }
}
