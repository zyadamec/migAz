// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{
    public interface IMigrationSourceUserControl
    {
        bool IsSourceContextAuthenticated { get; set; }
        String MigrationSourceType { get; }
        Task UncheckMigrationTarget(MigrationTarget migrationTarget);
    }
}

