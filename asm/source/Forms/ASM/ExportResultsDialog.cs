using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Arm;
using MigAz.Azure.Asm;
using MigAz.Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MigAz.Azure.Generator;

namespace MigAz.Forms.ASM
{
    public partial class ExportResultsDialog : Form
    {
        private TemplateResult _TemplateResult;

        private ExportResultsDialog() { }

        public ExportResultsDialog(TemplateResult templateResult)
        {
            InitializeComponent();
            _TemplateResult = templateResult;
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void btnViewTemplate_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _TemplateResult.GetTemplatePath();
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private void btnGenerateInstructions_Click(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _TemplateResult.GetInstructionPath();
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private void ExportResults_Load(object sender, EventArgs e)
        {
           
        }
    }
}
