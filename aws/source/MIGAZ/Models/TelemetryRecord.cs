// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace MIGAZ.Models
{
    public class TelemetryRecord
    {
        public Guid ExecutionId;
        public string AccessKeyId;
        public Dictionary<string, string> ProcessedResources;
        public string AWSRegion;
        public string SourceVersion;

    }
}

