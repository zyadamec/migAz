namespace MigAz.Azure.Arm
{
    public class DataDisk
    {
        public string name;
        public Vhd vhd;
        public string caching;
        public string createOption;
        public long diskSizeGB;
        public long lun;
    }
}
