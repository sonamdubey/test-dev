using Newtonsoft.Json;

namespace Bikewale.Entities.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 21th October 2015
    /// Modified By :   Sumit Kate on 18 Aug 2016
    /// Description :   Removed the private variable and kept only public properties
    /// </summary>
    public class ManufacturerLeadEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("versionId")]
        public uint VersionId { get; set; }

        [JsonProperty("cityId")]
        public uint CityId { get; set; }

        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

        [JsonProperty("pqId")]
        public uint PQId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("leadSourceId")]
        public ushort LeadSourceId { get; set; }
    }
}
