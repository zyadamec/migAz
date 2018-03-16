using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigAz.Core;
using MigAz.Core.Interface;

namespace MigAz.Azure.MigrationTarget
{
    public class VirtualNetworkGatewayConnection : Core.MigrationTarget
    {
        private IConnection _SourceConnection;

        private VirtualNetworkGatewayConnection() { }

        public VirtualNetworkGatewayConnection(IConnection connection, TargetSettings targetSettings)
        {
            this._SourceConnection = connection;
            this.SetTargetName(this.SourceName, targetSettings);
        }
        public IConnection SourceConnection { get { return _SourceConnection; } }

        public String SourceName
        {
            get
            {
                if (this.SourceConnection == null)
                    return String.Empty;
                else
                    return this.SourceConnection.ToString();
            }
        }

        public override string ImageKey { get { return "Connection"; } }

        public override string FriendlyObjectName { get { return "Connection"; } }


        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            this.TargetName = targetName.Trim().Replace(" ", String.Empty);
            this.TargetNameResult = this.TargetName;
        }
    }
}
