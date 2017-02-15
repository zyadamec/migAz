using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Interface
{
    public interface ILogProvider
    {
        void WriteLog(string function, string message);
    }
}
