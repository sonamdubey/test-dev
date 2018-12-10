using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace AppWebApi.Models
{
    public class Photo
    {
        [JsonProperty("smallPicUrl")]
        public string SmallPicUrl { get; set; }
        [JsonProperty("largePicUrl")]
        public string LargePicUrl { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("captionName")]
        public string CaptionName { get; set; }
        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }
        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }
    }
}