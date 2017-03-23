using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MigAz.Core.Generator
{
    public enum AlertType
    {
        Error,
        Warning
    }

    public class MigAzGeneratorAlert
    {
        public string Message;
        public AlertType AlertType = AlertType.Error;
        public object SourceTreeNode;

        public MigAzGeneratorAlert(AlertType alertType, string message, object source)
        {
            AlertType = alertType;
            Message = message;
            SourceTreeNode = source;
        }
    }
}
