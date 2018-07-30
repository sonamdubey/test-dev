using Bikewale.DTO.Make;
using Bikewale.DTO.Series;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 12-Aug-2017
    /// Description : RatingCount property added.
    /// </summary>
    public class ModelDetail  : ModelBase
    {
        [JsonProperty("makeBase")]
        public MakeBase MakeBase { get; set; }

        [JsonProperty("seriesBase")]
        public SeriesBase ModelSeries { get; set; }

        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("minPrice")]
        public Int64 MinPrice { get; set; }

        [JsonProperty("maxPrice")]
        public Int64 MaxPrice { get; set; }

        [JsonProperty("ratingCount")]
        public int RatingCount { get; set; }

        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }
        [JsonProperty("reviewRateStar")]
        public byte ReviewRateStar { get; set; }
    }
}
