using MIGAZ.Core.Azure;
using MIGAZ.Core.Interface;
using System;

namespace MIGAZ.Tests.Fakes
{
    class FakeAzureSubscription : ISubscription
    {
        private AzureEnvironment _AzureEnvironment = AzureEnvironment.AzureCloud;

        public Guid SubscriptionId
        {
            get { return Guid.NewGuid(); }
        }

        public Guid AzureAdTenantId
        {
            get { return Guid.NewGuid(); }
        }

        public string offercategories
        {
            get { return String.Empty; }
        }

        public AzureEnvironment AzureEnvironment
        {
            get { return _AzureEnvironment; }
            set { _AzureEnvironment = value; }
        }
    }
}
