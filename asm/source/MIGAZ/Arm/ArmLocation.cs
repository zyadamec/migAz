using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIGAZ.Azure;
using MIGAZ.Interface;

namespace MIGAZ.Arm
{
    public class ArmLocation : ILocation
    {
        private AzureContext _AzureContext;
        private JToken _LocationToken;

        #region Constructors

        private ArmLocation() { }

        public ArmLocation(AzureContext _AzureContext, JToken locationToken)
        {
            this._AzureContext = _AzureContext;
            this._LocationToken = locationToken;
        }

        #endregion

        #region Properties

        public string Id
        {
            get { return (string)_LocationToken["id"]; }
        }

        public string DisplayName
        {
            get { return (string)_LocationToken["displayName"]; }
        }

        public string Longitude
        {
            get { return (string)_LocationToken["longitude"]; }
        }

        public string Latitude
        {
            get { return (string)_LocationToken["latitude"]; }
        }

        public string Name
        {
            get { return (string)_LocationToken["name"]; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.DisplayName;
        }

        #endregion
    }
}
