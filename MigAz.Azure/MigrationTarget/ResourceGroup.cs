using MigAz.Azure.Interface;
using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class ResourceGroup : Core.MigrationTarget
    {
        private String _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private Arm.Location _TargetLocation;

        public ResourceGroup(TargetSettings targetSettings)
        {
            this.SetTargetName("NewResourceGroup", targetSettings);
        }

        public String SourceName
        {
            get { return String.Empty; }
        }


        public Arm.Location TargetLocation
        {
            get { return _TargetLocation; }
            set { _TargetLocation = value; }
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public override void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName + targetSettings.AvailabilitySetSuffix;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
