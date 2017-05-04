using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine : IVirtualMachine
    {
        private JToken _VirtualMachine;
        private List<Disk> _DataDisks = new List<Disk>();
        private Disk _OSVirtualHardDisk;
        private NetworkSecurityGroup _NetworkSecurityGroup;
        private List<NetworkInterfaceCard> _NetworkInterfaceCards = new List<NetworkInterfaceCard>();

        private VirtualMachine() { }

        public VirtualMachine(JToken virtualMachine)
        {
            _VirtualMachine = virtualMachine;

            _OSVirtualHardDisk = new Disk(_VirtualMachine["properties"]["storageProfile"]["osDisk"]);
            foreach (JToken dataDiskToken in _VirtualMachine["properties"]["storageProfile"]["dataDisks"])
            {
                _DataDisks.Add(new DataDisk(dataDiskToken));
            }

            foreach (JToken networkInterfaceToken in _VirtualMachine["properties"]["networkProfile"]["networkInterfaces"])
            {
                _NetworkInterfaceCards.Add(new NetworkInterfaceCard(networkInterfaceToken));
            }
        }

        public string Name => (string)_VirtualMachine["name"];
        public string Location => (string)_VirtualMachine["location"];
        public string Type => (string)_VirtualMachine["type"];
        public string Id => (string)_VirtualMachine["id"];
        public Guid VmId => new Guid((string)_VirtualMachine["properties"]["vmId"]);
        public string VmSize => (string)_VirtualMachine["properties"]["hardwareProfile"]["vmSize"];
        public string OSVirtualHardDiskOS => (string)_VirtualMachine["properties"]["storageProfile"]["osDisk"]["osType"];

        internal string AvailabilitySetId
        {
            get
            {
                try
                {
                    return (string)_VirtualMachine["properties"]["availabilitySet"]["id"];
                }
                catch (NullReferenceException)
                {
                    return String.Empty;
                }
            }
        }

        public List<Disk> DataDisks => _DataDisks;
        public NetworkSecurityGroup NetworkSecurityGroup => _NetworkSecurityGroup;
        public ResourceGroup ResourceGroup { get; set; }
        public Disk OSVirtualHardDisk => _OSVirtualHardDisk;
        public List<NetworkInterfaceCard> NetworkInterfaces => _NetworkInterfaceCards;

        public AvailabilitySet AvailabilitySet
        {
            get; private set;
        }
        public NetworkInterfaceCard PrimaryNetworkInterface
        {
            get
            {
                foreach (NetworkInterfaceCard networkInterface in this.NetworkInterfaces)
                {
                    if (networkInterface.IsPrimary)
                        return networkInterface;
                }

                return null;
            }
        }
        internal async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            this.AvailabilitySet = await azureContext.AzureRetriever.GetAzureARMAvailabilitySet(this.AvailabilitySetId);
            if (this.AvailabilitySet != null)
                this.AvailabilitySet.VirtualMachines.Add(this);

            this.ResourceGroup = await azureContext.AzureRetriever.GetAzureARMResourceGroup(this.Id);

            return;
        }

    }
}
