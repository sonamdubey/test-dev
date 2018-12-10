using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class CarDetail
    {
        [JsonProperty("make")]
        public string Make { get; set; }
        [JsonProperty("model")]
        public string Model { get; set; }
        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }
        [JsonProperty("minPrice")]
        public string MinPrice { get; set; }
        [JsonProperty("reviewRate")]
        public string ReviewRate { get; set; }
        [JsonProperty("reviewCount")]
        public string ReviewCount { get; set; }
        [JsonProperty("versionId")]
        public string VersionId { get; set; }
        [JsonProperty("priceQuoteUrl")]
        public string PriceQuoteUrl { get; set; }
        [JsonProperty("carModelUrl")]
        public string CarModelUrl { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
        [JsonProperty("exShowroomCityId")]
        public int ExShowRoomCityId;
        [JsonProperty("exShowroomCity")]
        public string ExShowRoomCityName;
    }
}