using Bikewale.DTO.Make;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Model.v2
{
    /// <summary>
    /// Created By : Sushil Kumar on 12th Sep 2017
    /// Description : Removed unused properties from previous version
    /// </summary>
    public class ModelDetails
    {
        [JsonProperty("makeDetails")]
        public MakeBase MakeBase { get; set; }

        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("minPrice")]
        public Int64 MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public Int64 MaxPrice { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }
    }
}
