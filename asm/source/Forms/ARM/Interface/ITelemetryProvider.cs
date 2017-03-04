using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Forms.ARM.Interface
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(string tenantId, string subscriptionId, Dictionary<string, string> processedItems, string offercategories);
    }
}
