// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core
{
    public abstract class MigrationTarget
    {
        Guid _MigrationTargetGuid = Guid.NewGuid();
        String _ApiVersion = String.Empty;
        String _ProviderNamespace = String.Empty;
        String _ResourceType = String.Empty;

        private MigrationTarget() { }

        public MigrationTarget(string providerNamespace, string resourceType)
        {
            _ProviderNamespace = providerNamespace;
            _ResourceType = resourceType;
        }

        public Guid MigrationTargetGuid { get { return _MigrationTargetGuid; } }

        public string ApiVersion { get { return _ApiVersion; } }
        public string ProviderNamespace { get { return _ProviderNamespace; } }
        public string ResourceType { get { return _ResourceType; } }

        public string TargetName { get; set; }
        public string TargetNameResult { get; set; }
        public abstract void SetTargetName(string targetName, TargetSettings targetSettings);

        public override string ToString()
        {
            return this.TargetNameResult;
        }

        public abstract string ImageKey { get; }
        public abstract string FriendlyObjectName { get; }

        public static String GetImageKey(Type migrationType)
        {
            return migrationType.ToString().Substring(migrationType.ToString().LastIndexOf(".") + 1);
        }
    }
}

