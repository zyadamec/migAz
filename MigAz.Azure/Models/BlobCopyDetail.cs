using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigAz.Azure.Models
{
    public class BlobCopyDetail
    {
        public string SourceEnvironment;
        public string SourceStorageAccount;
        public string SourceContainer;
        public string SourceBlob;
        public string SourceKey;
        public string SourceAbsoluteUri;
        public DateTime? SourceExpiration;
        public string TargetLocation;
        public string TargetResourceGroup;
        public string TargetStorageAccount;
        public string TargetStorageAccountType;
        public string TargetContainer;
        public string TargetBlob;
        public string TargetKey = String.Empty;
        public string OutputParameterName;
        public string Status = String.Empty;
        public string StatusDescription = String.Empty;
        public long TotalBytes = 0;
        public long BytesCopied = 0;
        public string StartTime = String.Empty;
        public string EndTime = String.Empty;
        public string SnapshotTime = String.Empty;
    }

}
