using MigAz.Azure.Asm.Generator;
using MigAz.Azure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests.Fakes
{
    class FakeLogProvider : ILogProvider
    {
        public void WriteLog(string function, string message)
        {
        }
    }
}
