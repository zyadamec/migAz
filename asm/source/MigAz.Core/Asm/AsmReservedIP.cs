using MIGAZ.Core.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Core.Asm
{
    public class AsmReservedIP
    {
        private XmlNode _ReservedIPNode;
        private AzureContext _AzureContext;

        public AsmReservedIP(AzureContext azureContext, XmlNode reservedIPNode)
        {
            this._AzureContext = azureContext;
            this._ReservedIPNode = reservedIPNode;
        }

        public string ServiceName
        {
            get
            {
                if (_ReservedIPNode.SelectNodes("ServiceName").Count == 0)
                    return String.Empty;

                return _ReservedIPNode.SelectSingleNode("ServiceName").InnerText;
            }
        }
    }
}
