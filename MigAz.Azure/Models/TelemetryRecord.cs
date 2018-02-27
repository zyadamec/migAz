// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace MigAz.Azure.Models
{
    public class TelemetryRecord
    {
        public Guid AppSessionGuid;
        public Guid ExecutionGuid;
        public Guid SourceTenantGuid;
        public Guid SourceSubscriptionGuid;
        public string SourceEnvironment;
        public Guid TargetTenantGuid;
        public Guid TargetSubscriptionGuid;
        public string TargetEnvironment;
        public Dictionary<string, string> ProcessedResources;
        public string OfferCategories;
        public string SourceVersion;
    }
}

