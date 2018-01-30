using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Core.Interface
{
    public interface IMigrationTarget
    {
        string SourceName { get; }
        string TargetName { get; }
        string TargetNameResult { get; }
        void SetTargetName(string targetName, TargetSettings targetSettings);
    }
}
