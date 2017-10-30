using Bikewale.DTO.Make;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Model.v2
{
    /// <summary>
    /// Created By : Sushil Kumar on 12th Sep 2017
    /// Description : Removed unused properties from previous version
    /// Modified by sajal Gupta on 30-10-2017
    /// Desxcriptiomn : added ModelId, ModelName , MaskingName 
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
        [JsonProperty("modelId")]
        public int ModelId { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("modelMaskingName")]
        public string MaskingName { get; set; }
    }
}
