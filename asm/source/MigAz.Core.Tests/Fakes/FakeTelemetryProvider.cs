using MIGAZ.Core.Asm;
using MIGAZ.Core.Generator;
using MIGAZ.Core.Interface;
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
