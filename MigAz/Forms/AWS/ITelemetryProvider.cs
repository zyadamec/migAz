using System.Collections.Generic;

namespace MigAz.Forms.AWS
{
    public interface ITelemetryProvider
    {
        void PostTelemetryRecord(string AccessKey, Dictionary<string, string> processedItems, string Region);
    }
}
