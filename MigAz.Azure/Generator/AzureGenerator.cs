// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Azure.Models;
using MigAz.Azure.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MigAz.Azure.Core.Generator;
using System.IO;
using MigAz.Azure.Core.ArmTemplate;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace MigAz.Azure.Generator.AsmToArm
{
    public class AzureGenerator : TemplateGenerator
    {

        private AzureGenerator() : base(null, null) { } 

        public AzureGenerator(
            ILogProvider logProvider, 
            IStatusProvider statusProvider) : base(logProvider, statusProvider)
        {
        }
    }
}

