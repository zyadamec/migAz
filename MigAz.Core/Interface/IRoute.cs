using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{

    public enum NextHopTypeEnum
    {
        VirtualAppliance,
        VirtualNetworkGateway,
        Internet,
        VnetLocal,
        None
    }

    public interface IRoute
    {
        string Name { get; }
        NextHopTypeEnum NextHopType { get; }
        string AddressPrefix { get; }
        string NextHopIpAddress { get; }
    }
}
