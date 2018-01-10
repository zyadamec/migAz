using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachineImage : ArmResource, IVirtualMachineImage
    {
        private AzureContext _AzureContext;

        private VirtualMachineImage() : base(null) { }

        public VirtualMachineImage(AzureContext azureContext, JToken resourceToken) : base(resourceToken)
        {
            _AzureContext = azureContext;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
