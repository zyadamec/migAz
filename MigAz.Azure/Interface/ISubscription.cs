// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface ISubscription
    {
            Guid SubscriptionId { get; }
            Guid AzureAdTenantId { get; }
            //string offercategories { get; }
            AzureEnvironment AzureEnvironment { get; }
    }
}

