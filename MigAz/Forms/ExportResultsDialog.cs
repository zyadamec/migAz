// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Windows.Forms;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Core.Generator;
using MigAz.Azure.Generator;

namespace MigAz.Forms
{
    public partial class ExportResultsDialog : Form
    {
        private TemplateGenerator _TemplateGenerator;

        private ExportResultsDialog() { }

        public ExportResultsDialog(TemplateGenerator templateGenerator)
        {
            InitializeComponent();
            _TemplateGenerator = templateGenerator;
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnViewTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = _TemplateGenerator.OutputDirectory + _TemplateGenerator.GetTemplateFilename();
                pInfo.UseShellExecute = true;
                Process p = Process.Start(pInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("MigAz was unable to launch an application on your system to open '" + _TemplateGenerator.OutputDirectory + _TemplateGenerator.GetTemplateFilename() + "'.\r\n\r\nThis commonly indicates there is no program registered with Windows to open this file type.\r\n\r\nIt is recommended you browser for this file using Windows Explorer.  Right-click on the file, select 'Open With' then 'Choose another program'.  Select the program you would like to open the filetype and ensure the checkbox for 'always use this application to open' is selected.");
            }
        }

        private void btnGenerateInstructions_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = _TemplateGenerator.OutputDirectory + _TemplateGenerator.GetDeployInstructionFilename();
                pInfo.UseShellExecute = true;
                Process p = Process.Start(pInfo);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("MigAz was unable to launch an application on your system to open '" + _TemplateGenerator.OutputDirectory + _TemplateGenerator.GetTemplateFilename() + "'.\r\n\r\nThis commonly indicates there is no program registered with Windows to open this file type.\r\n\r\nIt is recommended you browser for this file using Windows Explorer.  Right-click on the file, select 'Open With' then 'Choose another program'.  Select the program you would like to open the filetype and ensure the checkbox for 'always use this application to open' is selected.");
            }
        }

        private void ExportResults_Load(object sender, EventArgs e)
        {
           
        }
    }
}

