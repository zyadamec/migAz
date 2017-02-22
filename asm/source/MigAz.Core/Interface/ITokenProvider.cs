using Microsoft.IdentityModel.Clients.ActiveDirectory;
using MIGAZ.Core.Asm;
using MIGAZ.Core.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Core.Interface
{
    public interface ITokenProvider
    {
        Task<List<AzureSubscription>> GetSubscriptions();
        Task<AuthenticationResult> GetToken(AzureSubscription azureSubscription);
        Task<AuthenticationResult> GetCommonToken(string azureEnvironment);
    }
}
