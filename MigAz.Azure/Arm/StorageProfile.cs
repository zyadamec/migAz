using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class StorageProfile
    {
        public ImageReference imageReference;
        public OsDisk osDisk;
        public List<DataDisk> dataDisks;
    }
}
