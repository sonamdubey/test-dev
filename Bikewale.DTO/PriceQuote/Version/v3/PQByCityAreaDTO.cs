using Bikewale.DTO.Campaign;
using Bikewale.DTO.Model.v3;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.PriceQuote.Version.v3
{
    public class PQByCityAreaDTO
    {
        [JsonProperty("isCityExists")]
        public bool IsCityExists { get; set; }

        [JsonProperty("isAreaExists")]
        public bool IsAreaExists { get; set; }

        [JsonProperty("isAreaSelected")]
        public bool IsAreaSelected { get; set; }

        [JsonProperty("isExShowroomPrice")]
        public bool IsExShowroomPrice { get; set; }

        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }

        [JsonProperty("pqId")]
        public ulong PqId { get; set; }

        [JsonProperty("versionList")]
        public IEnumerable<VersionDetail> VersionList { get; set; }

        [JsonProperty("campaign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CampaignBaseDto Campaign { get; set; }
    }
}
