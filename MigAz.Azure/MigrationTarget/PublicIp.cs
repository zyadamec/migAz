using MigAz.Core;
using MigAz.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.MigrationTarget
{
    public class PublicIp : IMigrationTarget
    {
        private String _SourceName = String.Empty;
        private String _TargetName = String.Empty;
        private string _TargetNameResult = String.Empty;
        private String _DomainNameLabel = String.Empty;
        private Arm.PublicIP _Source;

        public PublicIp() { }

        public PublicIp(Arm.PublicIP armPublicIP, TargetSettings targetSettings)
        {
            this._Source = armPublicIP;
            this.SourceName = armPublicIP.Name;
            this.SetTargetName(armPublicIP.Name, targetSettings);

            this.DomainNameLabel = armPublicIP.DomainNameLabel;
        }

        public String DomainNameLabel
        {
            get { return _DomainNameLabel; }
            set { _DomainNameLabel = value.ToLower(); }
        }

        public Arm.PublicIP Source
        {
            get { return _Source; }
        }

        public String SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; }
        }

        public string TargetName
        {
            get { return _TargetName; }
        }

        public string TargetNameResult
        {
            get { return _TargetNameResult; }
        }

        public void SetTargetName(string targetName, TargetSettings targetSettings)
        {
            _TargetName = targetName.Trim().Replace(" ", String.Empty);
            _TargetNameResult = _TargetName;
        }

        public override string ToString()
        {
            return this.TargetNameResult;
        }
    }
}
