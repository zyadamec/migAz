using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIGAZ.Interface
{
    public interface IStorageAccount
    {
        string Id { get; }
        string Name { get; }
        string BlobStorageNamespace { get; }
    }
}
