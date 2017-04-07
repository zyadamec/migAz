using System.Collections.Generic;
using Amazon.EC2.Model;

namespace MIGAZ.Models
{
    public class AWSArtefacts
    {
        public AWSArtefacts()
        {
            StorageAccounts = new List<EbsVolume>();
            VirtualNetworks = new List<VPC>();
            Instances = new List<Ec2Instance>();
        }

        public List<EbsVolume> StorageAccounts { get; private set; }
        public List<VPC> VirtualNetworks { get; private set; }
        public List<Ec2Instance> Instances { get; private set; }
    }
}