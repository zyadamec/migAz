using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ManagedDisk : Core.ArmTemplate.ManagedDisk
    {
        private JToken _ManagedDiskToken;

        private ManagedDisk() : base(Guid.Empty) { }

        public ManagedDisk(JToken managedDisk) : base(Guid.Empty)
        {
            _ManagedDiskToken = managedDisk;
        }

        #region Properties

        public string Id
        {
            get { return (string)_ManagedDiskToken["id"]; }
        }
        public string Name
        {
            get { return (string)_ManagedDiskToken["name"]; }
        }
        public string Location
        {
            get { return (string)_ManagedDiskToken["location"]; }
        }
        public string Type
        {
            get { return (string)_ManagedDiskToken["type"]; }
        }

        public string DiskSizeGB
        {
            get { return (string)_ManagedDiskToken["properties"]["diskSizeGB"]; }
        }

        public string OwnerId
        {
            get { return (string)_ManagedDiskToken["properties"]["ownerId"]; }
        }

        public string ProvisioningState
        {
            get { return (string)_ManagedDiskToken["properties"]["provisioningState"]; }
        }

        public string DiskState
        {
            get { return (string)_ManagedDiskToken["properties"]["diskState"]; }
        }
        public string TimeCreated
        {
            get { return (string)_ManagedDiskToken["properties"]["timeCreated"]; }
        }
        public string AccountType
        {
            get { return (string)_ManagedDiskToken["properties"]["accountType"]; }
        }
        #endregion

    }
}
