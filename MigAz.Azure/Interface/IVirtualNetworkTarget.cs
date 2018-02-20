using MigAz.Azure.MigrationTarget;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Interface
{
    public interface IVirtualNetworkTarget
    {
        IMigrationVirtualNetwork TargetVirtualNetwork
        {
            get;
            set;
        }
        IMigrationSubnet TargetSubnet
        {
            get;
            set;
        }
        PrivateIPAllocationMethodEnum TargetPrivateIPAllocationMethod
        {
            get;
            set;
        }

        String TargetPrivateIpAddress
        {
            get;
            set;
        }

    }
}
