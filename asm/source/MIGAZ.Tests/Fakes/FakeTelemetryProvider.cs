using MIGAZ.Asm;
using MIGAZ.Generator;
using MIGAZ.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests.Fakes
{
    class FakeTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(TemplateResult templateResult)
        {
        }
    }
}
