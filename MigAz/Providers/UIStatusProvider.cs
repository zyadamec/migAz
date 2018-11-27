// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Providers
{
    class UIStatusProvider : IStatusProvider
    {
        private ToolStripItem _label;
        public UIStatusProvider(ToolStripItem label)
        {
            _label = label;
        }

        public void UpdateStatus(string message)
        {
            _label.Text = message;
            Application.DoEvents();
        }
    }
}

