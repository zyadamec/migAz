using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine : Core.ArmTemplate.VirtualMachine, IVirtualMachine
    {
        private JToken _VirtualMachine;
        private List<Disk> _DataDisks = new List<Disk>();
        private ResourceGroup _ResourceGroup;
        private NetworkSecurityGroup _NetworkSecurityGroup;
        private Disk _OSVirtualHardDisk;
        private VirtualNetwork _VirtualNetwork;
        private List<NetworkInterfaceCard> _NetworkInterfaceCards = new List<NetworkInterfaceCard>();

        private VirtualMachine() : base(Guid.Empty) { }

        public VirtualMachine(JToken virtualMachine) : base(Guid.Empty)
        {
            _VirtualMachine = virtualMachine;

            _OSVirtualHardDisk = new Disk(_VirtualMachine["properties"]["storageProfile"]["osDisk"]);
            foreach (JToken dataDiskToken in _VirtualMachine["properties"]["storageProfile"]["dataDisks"])
            {
                _DataDisks.Add(new DataDisk(dataDiskToken));
            }
        }

        public string Name => (string)_VirtualMachine["name"];
        public string Location => (string)_VirtualMachine["location"];
        public string Type => (string)_VirtualMachine["type"];
        public string Id => (string)_VirtualMachine["id"];
        public Guid VmId => new Guid((string)_VirtualMachine["properties"]["vmId"]);
        public string VmSize => (string)_VirtualMachine["properties"]["hardwareProfile"]["vmSize"];

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
        public ResourceGroup ResourceGroup => _ResourceGroup;
        public Disk OSVirtualHardDisk => _OSVirtualHardDisk;
        public VirtualNetwork VirtualNetwork => _VirtualNetwork;
        public List<NetworkInterfaceCard> NetworkInterfaces => _NetworkInterfaceCards;

        public AvailabilitySet AvailabilitySet
        {
            get; private set;
        }
        public AvailabilitySet TargetAvailabilitySet
        {
            get; set;
        }
        public Subnet TargetSubnet
        {
            get; set;
        }
        public string TargetName { get; set; }

        internal async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            this.AvailabilitySet = await azureContext.AzureRetriever.GetAzureARMAvailabilitySet(this.AvailabilitySetId);
            if (this.AvailabilitySet != null)
                this.AvailabilitySet.VirtualMachines.Add(this);

            return;
        }

    }
}
