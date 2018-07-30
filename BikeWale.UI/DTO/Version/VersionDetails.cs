using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Version
{
    /// <summary>
    /// Modified by :   Sumit Kate on 12 Apr 2016
    /// Description :   Changed the access modifier for MakeBase and ModelBase from private to public
    /// </summary>
    public class VersionDetails
    {
        [JsonProperty("new")]
        public bool New { get; set; }

        [JsonProperty("used")]
        public bool Used { get; set; }

        [JsonProperty("futuristic")]
        public bool Futuristic { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("smallPic")]
        public string SmallPicUrl { get; set; }

        [JsonProperty("largePic")]
        public string LargePicUrl { get; set; }

        [JsonProperty("price")]
        public Int64 Price { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
    }
}
