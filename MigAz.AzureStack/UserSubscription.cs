using MigAz.Azure;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AzureStack
{
    public class UserSubscription : AzureSubscription
    {
        #region Constructors

        private UserSubscription() : base(null, null, null, new Core.Interface.AzureEnvironment()) { }

        public UserSubscription(AzureContext azureContext, JObject subscriptionJson, AzureTenant parentAzureTenant, AzureEnvironment azureEnvironment) : base(azureContext, subscriptionJson, parentAzureTenant, azureEnvironment)
        {

        }

        #endregion
    }
}
