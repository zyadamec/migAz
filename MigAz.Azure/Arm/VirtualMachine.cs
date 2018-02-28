// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using MigAz.Azure.Interface;
using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class VirtualMachine : ArmResource, IVirtualMachine
    {
        private List<IArmDisk> _DataDisks = new List<IArmDisk>();
        private IArmDisk _OSVirtualHardDisk;
        private List<NetworkInterface> _NetworkInterfaceCards = new List<NetworkInterface>();
        private VMSize _VMSize;
        private AzureSubscription _AzureSubscription;


        private VirtualMachine() : base(null) { }

        public VirtualMachine(AzureSubscription azureSubscription, JToken resourceToken) : base(resourceToken)
        {
            _AzureSubscription = azureSubscription;

            if (ResourceToken["properties"]["storageProfile"]["osDisk"]["vhd"] == null)
            {
                _OSVirtualHardDisk = new ManagedDisk(this, ResourceToken["properties"]["storageProfile"]["osDisk"]);
            }
            else
            {
                _OSVirtualHardDisk = new ClassicDisk(this, ResourceToken["properties"]["storageProfile"]["osDisk"]);
            }

            foreach (JToken dataDiskToken in ResourceToken["properties"]["storageProfile"]["dataDisks"])
            {
                if (dataDiskToken["vhd"] == null)
                {
                    _DataDisks.Add(new ManagedDisk(this, dataDiskToken));
                }
                else
                {
                    _DataDisks.Add(new ClassicDisk(this, dataDiskToken));
                }
            }
        }

        public bool HasPlan
        {
            get
            {
                return ResourceToken["plan"] != null;
            }
        }

        public string Type => (string)ResourceToken["type"];
        public Guid VmId => new Guid((string)ResourceToken["properties"]["vmId"]);
        private string VmSizeString => (string)ResourceToken["properties"]["hardwareProfile"]["vmSize"];
        public VMSize VmSize
        {
            get { return _VMSize; }
            set { _VMSize = value;  }
        }

        public AzureSubscription AzureSubscription
        {
            get { return _AzureSubscription; }
        }

        public string OSVirtualHardDiskOS => (string)ResourceToken["properties"]["storageProfile"]["osDisk"]["osType"];

        internal string AvailabilitySetId
        {
            get
            {
                try
                {
                    return (string)ResourceToken["properties"]["availabilitySet"]["id"];
                }
                catch (NullReferenceException)
                {
                    return String.Empty;
                }
            }
        }

        public List<IArmDisk> DataDisks => _DataDisks;
        public IArmDisk OSVirtualHardDisk => _OSVirtualHardDisk;

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

        internal new async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);

            if (this.AvailabilitySetId != String.Empty)
                this.AvailabilitySet = azureContext.AzureSubscription.GetAzureARMAvailabilitySet(azureContext, this.AvailabilitySetId);

            if (this.AvailabilitySet != null)
                this.AvailabilitySet.VirtualMachines.Add(this);


            await this.OSVirtualHardDisk.InitializeChildrenAsync(azureContext);

            foreach (IArmDisk dataDisk in this.DataDisks)
            {
                if (dataDisk.GetType() == typeof(Arm.ClassicDisk))
                {
                    ClassicDisk classicDisk = (Arm.ClassicDisk)dataDisk;
                    await classicDisk.InitializeChildrenAsync(azureContext);
                }
            }

            foreach (JToken networkInterfaceToken in ResourceToken["properties"]["networkProfile"]["networkInterfaces"])
            {
                NetworkInterface networkInterface = await azureContext.AzureSubscription.GetAzureARMNetworkInterface(azureContext, (string)networkInterfaceToken["id"]);
                networkInterface.VirtualMachine = this;
                _NetworkInterfaceCards.Add(networkInterface);
            }

            // Seek the VmSize object that corresponds to the VmSize String obtained from the VM Json
            if (this.ResourceGroup != null && this.ResourceGroup.Location != null)
            {
                this.VmSize = this.ResourceGroup.Location.SeekVmSize(this.VmSizeString);
            }

            return;
        }

        public async Task Refresh(AzureContext azureContext)
        {
            base.SetResourceToken(await _AzureSubscription.GetAzureArmVirtualMachineDetail(azureContext, this));
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

