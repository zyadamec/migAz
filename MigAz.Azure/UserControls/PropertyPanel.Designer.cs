// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class PropertyPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyPanel));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblTargetAPIVersion = new System.Windows.Forms.Label();
            this.cmbApiVersions = new System.Windows.Forms.ComboBox();
            this.lblResourceType = new System.Windows.Forms.Label();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.lblAzureObjectName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.imageList1 = new System.Windows.Forms.ImageList();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblTargetAPIVersion);
            this.groupBox1.Controls.Add(this.cmbApiVersions);
            this.groupBox1.Controls.Add(this.lblResourceType);
            this.groupBox1.Controls.Add(this.pnlProperties);
            this.groupBox1.Controls.Add(this.lblAzureObjectName);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(483, 355);
            this.groupBox1.TabIndex = 59;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target Properties";
            this.groupBox1.Resize += new System.EventHandler(this.groupBox1_Resize);
            // 
            // lblTargetAPIVersion
            // 
            this.lblTargetAPIVersion.AutoSize = true;
            this.lblTargetAPIVersion.Location = new System.Drawing.Point(13, 103);
            this.lblTargetAPIVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTargetAPIVersion.Name = "lblTargetAPIVersion";
            this.lblTargetAPIVersion.Size = new System.Drawing.Size(131, 17);
            this.lblTargetAPIVersion.TabIndex = 66;
            this.lblTargetAPIVersion.Text = "Target API Version:";
            // 
            // cmbApiVersions
            // 
            this.cmbApiVersions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbApiVersions.FormattingEnabled = true;
            this.cmbApiVersions.Location = new System.Drawing.Point(151, 100);
            this.cmbApiVersions.Name = "cmbApiVersions";
            this.cmbApiVersions.Size = new System.Drawing.Size(218, 24);
            this.cmbApiVersions.TabIndex = 65;
            this.cmbApiVersions.SelectedIndexChanged += new System.EventHandler(this.cmbApiVersions_SelectedIndexChanged);
            // 
            // lblResourceType
            // 
            this.lblResourceType.AutoSize = true;
            this.lblResourceType.Location = new System.Drawing.Point(89, 60);
            this.lblResourceType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResourceType.Name = "lblResourceType";
            this.lblResourceType.Size = new System.Drawing.Size(46, 17);
            this.lblResourceType.TabIndex = 64;
            this.lblResourceType.Text = "label1";
            // 
            // pnlProperties
            // 
            this.pnlProperties.AutoScroll = true;
            this.pnlProperties.Location = new System.Drawing.Point(16, 129);
            this.pnlProperties.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(443, 182);
            this.pnlProperties.TabIndex = 63;
            // 
            // lblAzureObjectName
            // 
            this.lblAzureObjectName.AutoSize = true;
            this.lblAzureObjectName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAzureObjectName.Location = new System.Drawing.Point(87, 34);
            this.lblAzureObjectName.Name = "lblAzureObjectName";
            this.lblAzureObjectName.Size = new System.Drawing.Size(209, 28);
            this.lblAzureObjectName.TabIndex = 1;
            this.lblAzureObjectName.Text = "lblAzureObjectName";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(16, 30);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ResourceGroup");
            this.imageList1.Images.SetKeyName(1, "Disk");
            this.imageList1.Images.SetKeyName(2, "LoadBalancer");
            this.imageList1.Images.SetKeyName(3, "NetworkInterface");
            this.imageList1.Images.SetKeyName(4, "NetworkSecurityGroup");
            this.imageList1.Images.SetKeyName(5, "PublicIp");
            this.imageList1.Images.SetKeyName(6, "StorageAccount");
            this.imageList1.Images.SetKeyName(7, "VirtualMachine");
            this.imageList1.Images.SetKeyName(8, "AvailabilitySet");
            this.imageList1.Images.SetKeyName(9, "VirtualNetwork");
            this.imageList1.Images.SetKeyName(10, "RouteTable");
            this.imageList1.Images.SetKeyName(11, "VirtualMachineImage");
            this.imageList1.Images.SetKeyName(12, "Connection");
            this.imageList1.Images.SetKeyName(13, "VirtualNetworkGateway");
            this.imageList1.Images.SetKeyName(14, "LocalNetworkGateway");
            // 
            // PropertyPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PropertyPanel";
            this.Size = new System.Drawing.Size(491, 377);
            this.Resize += new System.EventHandler(this.PropertyPanel_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblAzureObjectName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlProperties;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblResourceType;
        private System.Windows.Forms.ComboBox cmbApiVersions;
        private System.Windows.Forms.Label lblTargetAPIVersion;
    }
}

