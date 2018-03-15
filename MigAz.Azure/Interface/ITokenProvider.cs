// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface ITokenProvider
    {
        UserInfo LastUserInfo { get; }
        Task<AuthenticationResult> GetToken(string resourceUrl, Guid tenantGuid, PromptBehavior promptBehavior = PromptBehavior.Auto);
        Task<AuthenticationResult> Login(string resourceUrl, PromptBehavior promptBehavior = PromptBehavior.Auto);
    }
}

