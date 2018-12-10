using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class VersionItem
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }
        [JsonProperty("carName")]
        public string CarName { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("makeName")]
        public string MakeName { get; set; }
        [JsonProperty("modelName")]
        public string ModelName { get; set; }
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }
        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}