using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class VPNClientConfiguration
    {
        public AddressSpace vpnClientAddressPool;
        public List<VPNClientCertificate> vpnClientRootCertificates;
        public List<VPNClientCertificate> vpnClientRevokedCertificates;
    }
}
