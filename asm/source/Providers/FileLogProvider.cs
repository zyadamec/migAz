using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using System;
using System.IO;

namespace MigAzASM.Providers
{
    class FileLogProvider : ILogProvider
    {
        private object lockObject = new object();

        public FileLogProvider()
        {

        }

        public void WriteLog(string function, string message)
        {
            lock (lockObject)
            {
                string logfiledir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\MigAz";
                if (!Directory.Exists(logfiledir)) { Directory.CreateDirectory(logfiledir); }

                string logfilepath = logfiledir + "\\MigAz-" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".log";
                string text = DateTime.Now.ToString() + "   " + function + "  " + message + Environment.NewLine;
                File.AppendAllText(logfilepath, text);
            }
        }
    }
}
