// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Generator.AsmToArm;
using MigAz.Azure.Core.Interface;

namespace MigAz.Tests.Fakes
{
    class FakeTelemetryProvider : ITelemetryProvider
    {
        public void PostTelemetryRecord(AzureGenerator templateResult)
        {
        }
    }
}

