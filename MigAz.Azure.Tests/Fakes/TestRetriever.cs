// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests.Fakes
{
    public class TestRetriever : AzureRetriever
    {
        public TestRetriever(ILogProvider logProvider, IStatusProvider statusProvider) : base(logProvider, statusProvider)
        {
        }

        

    }
}

