using MIGAZ.Core.Asm;
using MIGAZ.Core.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Core.Interface
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(TemplateResult templateResult);
    }
}
