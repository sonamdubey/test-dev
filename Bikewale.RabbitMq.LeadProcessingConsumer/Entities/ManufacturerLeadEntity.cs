using Newtonsoft.Json;

namespace Bikewale.RabbitMq.LeadProcessingConsumer
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
        public uint LeadSourceId { get; set; }

        public uint PinCodeId { get; set; }

        public uint ManufacturerDealerId { get; set; }

        public uint LeadId { get; set; }
        public string PQGUId { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 2 Feb 2017
    /// Description :   Gaadi.com Lead Entity
    /// </summary>
    public class GaadiLeadEntity
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("make")]
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("sub_source")]
        public string Source { get { return "bikewale"; } }
    }
}
