// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigAz.Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Tests
{
    [TestClass]
    public class MigAzCoreTests
    {
        [TestMethod]
        public async Task IPv4CIDRTest()
        {
            string networkCIDR1 = "192.168.16.0/22";
            string networkIP1 = "192.168.16.4";
            string networkCIDR2 = "192.168.24.0/21";
            string networkIP2 = "192.168.24.4";

            Assert.IsTrue(IPv4CIDR.IsValidCIDR(networkCIDR1), "Network CIDR should be valid.");
            Assert.IsTrue(IPv4CIDR.IsValidCIDR(networkCIDR2), "Network CIDR should be valid.");
            Assert.IsFalse(IPv4CIDR.IsValidCIDR(networkIP1), "Network IP 1 should not be a valid CIDR.");
            Assert.IsFalse(IPv4CIDR.IsValidCIDR(networkIP2), "Network IP 2 should not be a valid CIDR.");

            IPv4CIDR ipv4CIDR = new IPv4CIDR(networkCIDR2);
            Assert.IsTrue(ipv4CIDR.Mask == networkCIDR2, "CIDR Mask does not match expected value.");

            // Set CIDR Mask back to Network 1
            ipv4CIDR.Mask = networkCIDR1;
            Assert.IsTrue(ipv4CIDR.Mask == networkCIDR1, "CIDR Mask does not match expected value.");
            Assert.IsTrue(ipv4CIDR.Mask != networkCIDR2, "CIDR Mask does not match expected value.");

            Assert.IsTrue(ipv4CIDR.IsIpAddressInCIDR(networkIP1), "Network IP 1 should be in Network CIDR 1.");
            Assert.IsFalse(ipv4CIDR.IsIpAddressInCIDR(networkIP2), "Network IP 2 should not be in Network CIDR 1.");

            // Set CIDR Mask back to Network 2
            ipv4CIDR.Mask = networkCIDR2;

            Assert.IsTrue(ipv4CIDR.IsIpAddressInCIDR(networkIP2), "Network IP 2 should be in Network CIDR 2.");
            Assert.IsFalse(ipv4CIDR.IsIpAddressInCIDR(networkIP1), "Network IP 1 should not be in Network CIDR 2.");

        }
    }
}

