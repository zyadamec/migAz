using System.Collections.Generic;
using System;

namespace MigAz.Azure.Arm
{
    public class ArmResource
    {
        public string type;
        public string apiVersion;
        public string name;
        public string location = "[resourceGroup().location]";
        public Dictionary<string, string> tags;
        public List<string> dependsOn;
        public object properties;

        private ArmResource() { }

        public ArmResource(Guid executionGuid)
        {
            //if (app.Default.AllowTag)
            //{
                tags = new Dictionary<string, string>();
                tags.Add("migAz", executionGuid.ToString());
            //}
        }

        public override bool Equals(object obj)
        {
            try
            {
                ArmResource other = (ArmResource)obj;
                return this.type == other.type && this.name == other.name;
            }
            catch
            {
                return false;
            }
        }
    }
}
