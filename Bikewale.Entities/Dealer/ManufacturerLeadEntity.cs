using Newtonsoft.Json;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 21th October 2015
    /// </summary>
    public class ManufacturerLeadEntity
    {
        private string _name;
        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _email;
        [JsonProperty("email")]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private string _mobile;
        [JsonProperty("mobile")]
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        private uint _versionId;
        [JsonProperty("versionId")]
        public uint VersionId
        {
            get { return _versionId; }
            set { _versionId = value; }
        }

        private uint _cityId;
        [JsonProperty("cityId")]
        public uint CityId
        {
            get { return _cityId; }
            set { _cityId = value; }
        }

        private uint _dealerId;
        [JsonProperty("dealerId")]
        public uint DealerId
        {
            get { return _dealerId; }
            set { _dealerId = value; }
        }

        private uint _pqId;
        [JsonProperty("pqId")]
        public uint PQId
        {
            get { return _pqId; }
            set { _pqId = value; }
        }

        private string _deviceId;
        [JsonProperty("deviceId")]
        public string DeviceId
        {
            get { return _deviceId; }
            set { _deviceId = value; }
        }

        private ushort _leadSourceId;
        [JsonProperty("leadSourceId")]
        public ushort LeadSourceId
        {
            get { return _leadSourceId; }
            set { _leadSourceId = value; }
        }
    }
}
