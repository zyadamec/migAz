using MigAz.Azure.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class ManagedDisk : ArmResource, IArmDisk
    {
        private VirtualMachine _VirtualMachine = null;

        private ManagedDisk() : base(null) { }

        public ManagedDisk(VirtualMachine virtualMachine, JToken resourceToken) : base(resourceToken)
        {
            _VirtualMachine = virtualMachine;
        }
        public ManagedDisk(JToken resourceToken) : base(resourceToken)
        {
        }

        public new async Task InitializeChildrenAsync(AzureContext azureContext)
        {
            await base.InitializeChildrenAsync(azureContext);
        }

        #region Properties

        public VirtualMachine ParentVirtualMachine
        {
            get { return _VirtualMachine; }
        }

        public string Type
        {
            get { return (string)ResourceToken["type"]; }
        }

        public Int32 DiskSizeGb
        {
            get
            {
                try
                {
                    Int32 diskSizeGb = 0;
                    Int32.TryParse((string)this.ResourceToken["properties"]["diskSizeGB"], out diskSizeGb);

                    return diskSizeGb;
                }
                catch (System.NullReferenceException)
                {
                    return 0;
                }
            }
        }

        public string OwnerId
        {
            get { return (string)ResourceToken["properties"]["ownerId"]; }
        }

        public string ProvisioningState
        {
            get { return (string)ResourceToken["properties"]["provisioningState"]; }
        }

        public string DiskState
        {
            get { return (string)ResourceToken["properties"]["diskState"]; }
        }
        public string TimeCreated
        {
            get { return (string)ResourceToken["properties"]["timeCreated"]; }
        }
        public string AccountType
        {
            get { return (string)ResourceToken["properties"]["accountType"]; }
        }

        public string CreateOption
        {
            get { return (string)ResourceToken["properties"]["creationData"]["createOption"]; }
        }

        public string SourceUri
        {
            get { return (string)ResourceToken["properties"]["creationData"]["sourceUri"]; }
        }

        public bool IsEncrypted
        {
            get
            {
                // todo now russell, this is not verified, only copied code
                if (this.ResourceToken["encryptionSettings"] == null)
                    return false;

                if (this.ResourceToken["encryptionSettings"]["enabled"] == null)
                    return false;

                return Convert.ToBoolean((string)this.ResourceToken["encryptionSettings"]["enabled"]);
            }
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
