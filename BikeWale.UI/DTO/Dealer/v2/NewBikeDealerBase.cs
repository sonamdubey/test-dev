using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Dealer.v2
{
    /// <summary>
    /// Created by  :   Sumit Kate on 20 May 2016
    /// Description :   Dealer base DTO version 2
    /// </summary>
    public class NewBikeDealerBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("isFeatured")]
        public bool IsFeatured { get; set; }

        [JsonProperty("campaignId")]
        public int CampaignId { get; set; }

        [JsonIgnore]
        public UInt16 DealerType { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
