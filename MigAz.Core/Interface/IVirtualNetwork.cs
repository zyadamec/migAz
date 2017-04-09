using System.Collections.Generic;

namespace MigAz.Core.Interface
{
    public interface IVirtualNetwork
    {
        string Id { get; }
        List<ISubnet> Subnets { get; }
        string TargetId { get; }
    }
}
