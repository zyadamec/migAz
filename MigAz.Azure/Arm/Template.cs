using System.Collections.Generic;

namespace MigAz.Azure.Arm
{
    public class Template
    {
        public string schemalink = "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
        public string contentVersion = "1.0.0.0";
        public Dictionary<string, Parameter> parameters;
        public Dictionary<string, string> variables;
        public List<ArmResource> resources;
    }
}
