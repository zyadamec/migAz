using MigAz.Core.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ManagedDisk : IArmDisk
    {
        private JToken _ManagedDiskToken;

        private ManagedDisk() { }

        public ManagedDisk(JToken managedDisk) 
        {
            _ManagedDiskToken = managedDisk;
        }

        public async Task InitializeChildrenAsync()
        {

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
