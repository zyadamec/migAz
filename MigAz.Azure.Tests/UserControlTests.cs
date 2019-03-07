using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MIGAZ.Tests
{
    [TestClass]
    public class UserControlTests
    {
        [TestMethod]
        public async Task Ipv4UserControlTest()
        {
            MigAz.Azure.UserControls.IPv4AddressBox ipv4AddressBox = new MigAz.Azure.UserControls.IPv4AddressBox();
            ipv4AddressBox.Text = "10.249.16.4/32";
            Assert.AreEqual("10.249.16.4", ipv4AddressBox.Text);
            ipv4AddressBox.Text = "10.249.8.4";
            Assert.AreEqual("10.249.8.4", ipv4AddressBox.Text);
        }
    }
}
