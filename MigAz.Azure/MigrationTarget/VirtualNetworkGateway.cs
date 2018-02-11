using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualNetworkGateway : Core.MigrationTarget, IMigrationVirtualNetworkGateway
    {
        private IVirtualNetworkGateway _SourceVirtualNetworkGateway;
        private List<Subnet> _TargetSubnets = new List<Subnet>();

        private VirtualNetworkGateway() { }

        public VirtualNetworkGateway(IVirtualNetworkGateway virtualNetworkGateway, TargetSettings targetSettings)
        {
            this._SourceVirtualNetworkGateway = virtualNetworkGateway;
            this.SetTargetName(this.SourceName, targetSettings);
        }

        public IVirtualNetworkGateway SourceVirtualNetworkGateway { get; }

        public String SourceName
        {
            get
            {
                if (this.SourceVirtualNetworkGateway == null)
                    return String.Empty;
                else
                    return this.SourceVirtualNetworkGateway.ToString();
            }
        }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName;
        }

    }
}
