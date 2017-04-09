using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Arm
{
    public class NetworkSecurityGroupRule
    {
        public string Name => "TODO";
        public long Priority => 0; // TODO
        public string Direction => "TODO";
        public string Access => "TODO";
        public string SourceAddressPrefix => "TODO";
        public string DestinationAddressPrefix => "TODO";
        public string SourcePortRange => "TODO";
        public string DestinationPortRange => "TODO";
        public string Protocol => "TODO";


        //securityrule_properties.description = rule.name.Value;
        //        securityrule_properties.direction = rule.properties.direction.Value;
        //        securityrule_properties.priority = rule.properties.priority.Value;
        //        securityrule_properties.access = rule.properties.access.Value;
        //        securityrule_properties.sourceAddressPrefix = rule.properties.sourceAddressPrefix.Value;
        //        securityrule_properties.sourceAddressPrefix.Replace("_", "");
        //        securityrule_properties.destinationAddressPrefix = rule.properties.destinationAddressPrefix.Value;
        //        securityrule_properties.destinationAddressPrefix.Replace("_", "");
        //        securityrule_properties.sourcePortRange = rule.properties.sourcePortRange.Value;
        //        securityrule_properties.destinationPortRange = rule.properties.destinationPortRange.Value;
        //        securityrule_properties.protocol = rule.properties.protocol.Value;


    }
}
