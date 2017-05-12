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
        private List<NetworkInterface> _NetworkInterfaceCards = new List<NetworkInterface>();

        private VirtualMachine() { }

        public VirtualMachine(AzureContext azureContext, JToken virtualMachine)
        {
            _VirtualMachine = virtualMachine;

            _OSVirtualHardDisk = new Disk(azureContext, _VirtualMachine["properties"]["storageProfile"]["osDisk"]);
            foreach (JToken dataDiskToken in _VirtualMachine["properties"]["storageProfile"]["dataDisks"])
            {
                _DataDisks.Add(new Disk(azureContext, dataDiskToken));
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
        public List<NetworkInterface> NetworkInterfaces => _NetworkInterfaceCards;

        public AvailabilitySet AvailabilitySet
        {
            get; private set;
        }
        public NetworkInterface PrimaryNetworkInterface
        {
            get
            {
                foreach (NetworkInterface networkInterface in this.NetworkInterfaces)
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

            await this.OSVirtualHardDisk.InitializeChildrenAsync();

            foreach (Disk dataDisk in this.DataDisks)
            {
                await dataDisk.InitializeChildrenAsync();
            }

            foreach (JToken networkInterfaceToken in _VirtualMachine["properties"]["networkProfile"]["networkInterfaces"])
            {
                NetworkInterface networkInterface = await azureContext.AzureRetriever.GetAzureARMNetworkInterface((string)networkInterfaceToken["id"]);
                networkInterface.VirtualMachine = this;
                _NetworkInterfaceCards.Add(networkInterface);
            }

            return;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
