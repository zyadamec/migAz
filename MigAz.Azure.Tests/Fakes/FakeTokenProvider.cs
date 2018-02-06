using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MigAz.Azure.Asm;
using MigAz.Azure.Interface;
using MigAz.Azure;
using MigAz.Core.Interface;

namespace MigAz.Tests.Fakes
{
    class FakeTokenProvider : ITokenProvider
    {
        public string LogonUrl
        {
            get { return "https://logon.contoso.com"; }
        }

        public UserInfo LastUserInfo
        {
            get { return null; }
        }

        public async Task<AuthenticationResult> GetToken(string resourceUrl, Guid tenantGuid, PromptBehavior promptBehavior)
        {
            return null;
        }

        public Task<AuthenticationResult> Login(string resourceUrl, PromptBehavior promptBehavior)
        {
            return null;
        }
    }
}
