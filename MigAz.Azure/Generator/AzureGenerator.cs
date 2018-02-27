// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Azure.Models;
using MigAz.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MigAz.Core.Generator;
using System.IO;
using MigAz.Core.ArmTemplate;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace MigAz.Azure.Generator.AsmToArm
{
    public class AzureGenerator : TemplateGenerator
    {

        private AzureGenerator() : base(null, null, null, null, null) { } 

        public AzureGenerator(
            AzureContext sourceAzureContext,
            ISubscription sourceSubscription, 
            ISubscription targetSubscription,
            ILogProvider logProvider, 
            IStatusProvider statusProvider) : base(sourceAzureContext, logProvider, statusProvider, sourceSubscription, targetSubscription)
        {
        }
    }
}

