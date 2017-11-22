using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VMImage : ArmResource
    {
        private AzureContext _AzureContext;

        private VMImage() : base(null) { }

        public VMImage(AzureContext azureContext, JToken resourceToken) : base(resourceToken)
        {
            _AzureContext = azureContext;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
