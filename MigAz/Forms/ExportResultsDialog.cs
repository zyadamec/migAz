using System;
using System.Diagnostics;
using System.Windows.Forms;
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
