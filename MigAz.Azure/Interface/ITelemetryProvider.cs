using MigAz.Azure.Asm;
using MigAz.Azure.Generator;
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
