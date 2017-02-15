using MIGAZ.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MIGAZ.Asm
{
    public class AsmStorageAccountKeys
    {
        private AzureContext _AzureContext;
        private XmlDocument _StorageAccountKeysXml;

        public AsmStorageAccountKeys(AzureContext azureContext, XmlDocument storageAccountKeysXml)
        {
            this._AzureContext = azureContext;
            this._StorageAccountKeysXml = storageAccountKeysXml;
        }

        public string Primary
        {
            get { return _StorageAccountKeysXml.SelectSingleNode("//StorageServiceKeys/Primary").InnerText; }
        }

        public string Secondary
        {
            get { return _StorageAccountKeysXml.SelectSingleNode("//StorageServiceKeys/Secondary").InnerText; }
        }
    }
}
