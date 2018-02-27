// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.AzureStack.UserControls
{
    public partial class MigrationAzureStackTargetContext : UserControl
    {
        public MigrationAzureStackTargetContext()
        {
            InitializeComponent();
        }

        private void MigrationAzureStackTargetContext_Resize(object sender, EventArgs e)
        {
            this.azureStackLoginContextViewer1.Width = this.Width - 10;
            this.azureStackLoginContextViewer1.Height = this.Height - 10;
        }
    }
}

