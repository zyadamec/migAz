using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core
{
    public class IPv4CIDR
    {
        private String _Mask;

        #region Constructors

        private IPv4CIDR() { }

        public IPv4CIDR(string mask)
        {
            if (!IsValidCIDR(mask))
                throw new ArgumentException("Invalid IP v4 CIDR Mask: " + mask);

            _Mask = mask;
        }

        #endregion

        #region Properties

        public String Mask
        {
            get { return _Mask; }
            set { _Mask = value; }
        }
        #endregion

        #region Methods

        public bool IsIpAddressInCIDR(string ipAddress)
        {
            string[] CIDRMaskArray = _Mask.Split('/');

            int intCIDRIPAddress = BitConverter.ToInt32(IPAddress.Parse(CIDRMaskArray[0]).GetAddressBytes(), 0);
            int intIpAddress = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
            int intCIDRNetworkOrder = IPAddress.HostToNetworkOrder(-1 << (32 - int.Parse(CIDRMaskArray[1])));

            return ((intCIDRIPAddress & intCIDRNetworkOrder) == (intIpAddress & intCIDRNetworkOrder));
        }

        #endregion

        #region Static Methods

        public static bool IsValidCIDR(string CIDRMask)
        {
            string[] CIDRMaskArray = CIDRMask.Split('/');

            if (CIDRMaskArray.Count() != 2)
                return false; // Should have 2 parts:  1) The IP Address; 2) Block

            int intBlock = -1;
            int.TryParse(CIDRMaskArray[1], out intBlock);

            if (intBlock < 0 || intBlock > 32)
                return false; // Block value must be between 0 and 32

            string[] CIDRIPArray = CIDRMaskArray[0].Split('.');

            if (CIDRIPArray.Count() != 4)
                return false; // Should have 4 IP Address parts

            IPAddress ipAddress;
            if (!IPAddress.TryParse(CIDRMaskArray[0], out ipAddress))
                return false;

            return true;
        }

        public static bool IsIpAddressInAddressPrefix(string addressPrefix, string ipAddress)
        {
            IPv4CIDR ipv4CIDR = new IPv4CIDR(addressPrefix);
            return ipv4CIDR.IsIpAddressInCIDR(ipAddress);
        }

        #endregion
    }
}
