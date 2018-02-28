// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{
    public interface ILogProvider
    {
        void WriteLog(string function, string message);

    }
}

