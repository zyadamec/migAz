// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class LocalNetworkSite
    {
        private XmlNode _XmlNode;
        private AzureSubscription _AzureSubscription;
        private VirtualNetwork _VirtualNetwork;
        private String _SharedKey = String.Empty;
        private String _TargetName;
        
        private LocalNetworkSite() { }

        public LocalNetworkSite(VirtualNetwork virtualNetwork, XmlNode xmlNode)
        {
            _AzureSubscription = virtualNetwork.AzureSubscription;
            _VirtualNetwork = virtualNetwork;
            _XmlNode = xmlNode;
            this.TargetName = this.Name;
        }

        public async Task InitializeChildrenAsync()
        {
            _SharedKey = await this.AzureSubscription.GetAzureAsmVirtualNetworkSharedKey(this.VirtualNetwork.Name, this.Name);
        }

        public String Name
        {
            get { return _XmlNode.SelectSingleNode("Name").InnerText; }
        }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public String TargetName
        {
            get { return _TargetName; }
            set { _TargetName = value.Replace(' ', '_'); }
        }

        public String ConnectionType
        {
            get { return _XmlNode.SelectSingleNode("Connections/Connection/Type").InnerText; }
        }

        public String VpnGatewayAddress
        {
            get { return _XmlNode.SelectSingleNode("VpnGatewayAddress").InnerText; }
        }

        public String SharedKey
        {
            get { return _SharedKey; }
        }

        public List<String> AddressPrefixes
        {
            get
            {
                List<String> addressPrefixes = new List<string>();

                foreach (XmlNode addressprefix in _XmlNode.SelectNodes("AddressSpace/AddressPrefixes/AddressPrefix"))
                {
                    addressPrefixes.Add(addressprefix.InnerText);
                }

                return addressPrefixes;
            }
        }

        public VirtualNetwork VirtualNetwork
        {
            get { return _VirtualNetwork; }
        }
    }
}

