using MigAz.Azure.Asm;
using MigAz.Azure.Asm.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(TemplateResult templateResult);
    }
}
