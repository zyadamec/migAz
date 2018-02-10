using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core
{
    public abstract class MigrationTarget
    {
        private Guid _MigrationTargetGuid = Guid.NewGuid();

        public Guid MigrationTargetGuid { get { return _MigrationTargetGuid; } }
        //IMigrationSource Source { get; }
        public string SourceName { get; }
        public string TargetName { get; }
        public string TargetNameResult { get; }
        public abstract void SetTargetName(string targetName, TargetSettings targetSettings);
    }
}
