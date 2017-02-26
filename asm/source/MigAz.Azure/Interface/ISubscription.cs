using System;

namespace MigAz.Azure.Interface
{
    public interface ISubscription
    {
        Guid SubscriptionId { get; }
        Guid AzureAdTenantId { get; }
        string offercategories { get; }
        AzureEnvironment AzureEnvironment { get; }
    }
}
