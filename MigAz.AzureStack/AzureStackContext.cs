using MigAz.Azure;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AzureStack
{
    public class AzureStackContext
    {
        private ILogProvider _LogProvider;
        private AzureTokenProvider _TokenProvider;

        private AzureStackContext() { }

        public AzureStackContext(ILogProvider logProvider)
        {
            _LogProvider = logProvider;
            _TokenProvider = new AzureTokenProvider("https://login.microsoftonline.com/", _LogProvider);
        }

        public AzureTokenProvider TokenProvider
        {
            get { return _TokenProvider; }
        }

        public async Task Login()
        {
            await this.TokenProvider.Login("https://management.core.windows.net/", Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior.Always);
            //UserAuthenticated?.Invoke(this);
        }
    }
}
