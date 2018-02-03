using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.AzureStack
{
    public class AzureStackContext
    {

        public async Task Login()
        {
            await this.TokenProvider.LoginAzureProvider();
            UserAuthenticated?.Invoke(this);
        }
    }
}
