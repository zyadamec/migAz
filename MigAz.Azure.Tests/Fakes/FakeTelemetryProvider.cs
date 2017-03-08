using MigAz.Azure.Generator;
using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Tests.Fakes
{
    class FakeTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(AsmToArmGenerator templateResult)
        {
        }
    }
}
