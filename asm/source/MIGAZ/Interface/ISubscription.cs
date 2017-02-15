using MIGAZ.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Interface
{
    public interface ISubscription
    {
        Guid SubscriptionId { get; }
        Guid AzureAdTenantId { get; }
        string offercategories { get; }
        AzureEnvironment AzureEnvironment { get; }
    }
}
