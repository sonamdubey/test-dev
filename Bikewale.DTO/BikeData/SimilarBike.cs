using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.BikeData
{
    public class SimilarBike : MinSpecs
    {
        [JsonProperty("minPrice")]
        public int MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public int MaxPrice { get; set; }

        [JsonProperty("price")]
        public int VersionPrice { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("reviewRate")]
        public Double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public UInt16 ReviewCount { get; set; }

        [JsonProperty("makeBase")]
        public MakeBase MakeBase { get; set; }

        [JsonProperty("modelBase")]
        public ModelBase ModelBase { get; set; }

        [JsonProperty("versionBase")]
        public VersionBase VersionBase { get; set; }
    }
}
