using Bikewale.ManufacturerCampaign.Entities;
using Newtonsoft.Json;

namespace Bikewale.Entities.BikeBooking
{
    /// <summary>
    /// Modified By  :Sushil Kumar on 7th June 2016
    /// Description :Added properties to get bike make and model name
    /// </summary>
    public class PQOutputEntity
    {
        [JsonProperty("pqId")]
        public ulong PQId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("defaultVersionId")]
        public uint DefaultVersionId { get; set; }
        [JsonProperty("isDealerAvailable")]
        public bool IsDealerAvailable { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonIgnore]
        public ManufacturerCampaignEntity ManufacturerCampaign { get; set; }
    }
}
