using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class Constants
    {
        // Future considerations that these could become variables of the Azure Environment, as the min/max counts could vary by Environment?  Location?
        public const int AvailabilitySetMinPlatformUpdateDomain = 5;
        public const int AvailabilitySetMaxPlatformUpdateDomain = 20;
        public const int AvailabilitySetMinPlatformFaultDomain = 2;
        public const int AvailabilitySetMaxPlatformFaultDomain = 3;
    }
