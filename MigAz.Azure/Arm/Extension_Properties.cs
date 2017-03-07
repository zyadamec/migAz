using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class Extension_Properties
    {
        public string publisher;
        public string type;
        public string typeHandlerVersion;
        public bool autoUpgradeMinorVersion;
        public Dictionary<string, string> settings;
    }
}
