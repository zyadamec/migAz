// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class ReservedIP
    {
        private XmlNode _ReservedIPNode;
        private AzureSubscription _AzureSubscription;

        public ReservedIP(AzureSubscription azureSubscription, XmlNode reservedIPNode)
        {
            this._AzureSubscription = azureSubscription;
            this._ReservedIPNode = reservedIPNode;
        }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
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

