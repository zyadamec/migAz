using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.IO;

namespace MigAz.Providers
{
    class FileLogProvider : ILogProvider
    {
        public FileLogProvider()
        {

        }

        public void WriteLog(string function, string message)
        {
            string logfiledir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz";
            if (!Directory.Exists(logfiledir)) { Directory.CreateDirectory(logfiledir); }

            string logfilepath = logfiledir + "\\MigAz-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
            string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
            File.AppendAllText(logfilepath, text);
        }
    }
}
