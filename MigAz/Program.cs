using MigAz.Forms;
using System;
using System.Windows.Forms;

namespace MigAz
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.Run(new MigAzForm());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            UnhandledExceptionDialog exceptionDialog = new UnhandledExceptionDialog(e.Exception);
            exceptionDialog.ShowDialog();
        }
    }
}
