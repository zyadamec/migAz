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
using MigAz.Azure;
using MigAz.Azure.Generator;
using MigAz.Azure.Generator.AsmToArm;

namespace MigAz.Forms
{
    public partial class ExportResultsDialog : Form
    {
        private AsmToArmGenerator _TemplateResult;

        private ExportResultsDialog() { }

        public ExportResultsDialog(AsmToArmGenerator templateResult)
        {
            InitializeComponent();
            _TemplateResult = templateResult;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportResults_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void btnGenerateInstructions_Click_1(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _TemplateResult.GetInstructionPath();
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }

        private void btnViewTemplate_Click_1(object sender, EventArgs e)
        {
            ProcessStartInfo pInfo = new ProcessStartInfo();
            pInfo.FileName = _TemplateResult.GetInstructionPath();
            pInfo.UseShellExecute = true;
            Process p = Process.Start(pInfo);
        }
    }
}
