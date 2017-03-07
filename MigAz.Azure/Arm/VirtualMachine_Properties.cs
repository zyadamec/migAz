namespace MigAz.Azure.Arm
{
    public class VirtualMachine_Properties
    {
        public HardwareProfile hardwareProfile;
        public Reference availabilitySet;
        public OsProfile osProfile;
        public StorageProfile storageProfile;
        public NetworkProfile networkProfile;
        public DiagnosticsProfile diagnosticsProfile;
    }
}
