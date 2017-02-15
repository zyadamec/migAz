using MIGAZ.Asm;
using MIGAZ.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Interface
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(TemplateResult templateResult);
    }
}
