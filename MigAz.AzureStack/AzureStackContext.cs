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
        private AzureStackTokenProvider _TokenProvider;

        private AzureStackContext() { }

        public AzureStackContext(ILogProvider logProvider)
        {
            _LogProvider = logProvider;
            _TokenProvider = new AzureStackTokenProvider(_LogProvider);
        }

        public AzureStackTokenProvider TokenProvider
        {
            get { return _TokenProvider; }
        }

        public async Task Login()
        {
            await this.TokenProvider.LoginAzureProvider();
            //UserAuthenticated?.Invoke(this);
        }
    }
}
