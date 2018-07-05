using Bikewale.ManufacturerCampaign.Entities;
using Newtonsoft.Json;

namespace Bikewale.Entities.BikeBooking.v2
{
    /// <summary>
    /// Modified By  :Sushil Kumar on 7th June 2016
    /// Description :Added properties to get bike make and model name
    /// Modified : Kartik Rathod on 20 jun 2018 price qoute changes changed PQId datattype to string
    /// </summary>
    public class PQOutputEntity
    {
        [JsonProperty("pqId")]
        public string PQId { get; set; }
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("defaultVersionId")]
        public bool IsDealerAvailable { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonIgnore]
        public ManufacturerCampaignEntity ManufacturerCampaign { get; set; }
    }
}