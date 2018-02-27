// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace MigAz.Azure.UserControls
{
    partial class TreeViewSourceResourceManager
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
            this.treeAzureARM = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // treeAzureARM
            // 
            this.treeAzureARM.CheckBoxes = true;
            this.treeAzureARM.Location = new System.Drawing.Point(0, 0);
            this.treeAzureARM.Name = "treeAzureARM";
            this.treeAzureARM.Size = new System.Drawing.Size(324, 435);
            this.treeAzureARM.TabIndex = 72;
            this.treeAzureARM.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeAzureResourcesSource_AfterCheck);
            // 
            // TreeViewSourceResourceManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeAzureARM);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TreeViewSourceResourceManager";
            this.Size = new System.Drawing.Size(576, 537);
            this.Resize += new System.EventHandler(this.TreeViewSourceResourceManager_Resize);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView treeAzureARM;
    }
}

