using System;

namespace MigAz.Azure.Arm
{
    public class StorageAccount : ArmResource
    {
        public StorageAccount(Guid executionGuid) : base(executionGuid)
        {
            type = "Microsoft.Storage/storageAccounts";
            apiVersion = "2015-06-15";
        }
    }
}
