// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Xml;

namespace MigAz.Azure.Asm
{
    public class LoadBalancerRule
    {
        #region Variables

        private AzureSubscription _AzureSubscription;
        private XmlNode _XmlNode;

        #endregion

        #region Constructors

        private LoadBalancerRule() { }

        public LoadBalancerRule(AzureSubscription azureSubscription, XmlNode xmlNode)
        {
            _AzureSubscription = azureSubscription;
            _XmlNode = xmlNode;
        }

        #endregion

        #region Properties

        public Int64 ProbePort
        {
            get
            {
                if (_XmlNode.SelectSingleNode("LoadBalancerProbe") == null)
                    return 0;

                XmlNode probenode = _XmlNode.SelectSingleNode("LoadBalancerProbe");

                if (probenode.SelectSingleNode("Port") == null)
                    return 0;

                try
                {
                    return Int64.Parse(probenode.SelectSingleNode("Port").InnerText);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public string ProbeProtocol
        {
            get
            {
                XmlNode probenode = _XmlNode.SelectSingleNode("LoadBalancerProbe");

                if (probenode.SelectSingleNode("Protocol") == null)
                    return "Unknown";

                return probenode.SelectSingleNode("Protocol").InnerText;
            }
        }

        public Int64 Port
        {
            get
            {
                if (_XmlNode.SelectSingleNode("Port") == null)
                    return 0;

                try
                {
                    return Int64.Parse(_XmlNode.SelectSingleNode("Port").InnerText);
                }
                catch
                { return 0; }
            }
        }

        public Int64 LocalPort
        {
            get
            {
                if (_XmlNode.SelectSingleNode("LocalPort") == null)
                    return 0;

                try
                {
                    return Int64.Parse(_XmlNode.SelectSingleNode("LocalPort").InnerText);
                }
                catch
                { return 0; }
            }
        }
        
        public string Protocol
        {
            get
            {
                if (_XmlNode.SelectSingleNode("Protocol") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("Protocol").InnerText;
            }
        }

        public string Name
        {
            get
            {
                if (_XmlNode.SelectSingleNode("Name") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("Name").InnerText;
            }
        }

        public string LoadBalancedEndpointSetName
        {
            get
            {
                if (_XmlNode.SelectSingleNode("LoadBalancedEndpointSetName") == null)
                    return String.Empty;

                return _XmlNode.SelectSingleNode("LoadBalancedEndpointSetName").InnerText;
            }
        }

        #endregion
    }
}

