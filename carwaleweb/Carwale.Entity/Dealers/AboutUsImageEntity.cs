using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Dealers
{
    [Serializable]
    //[JsonObject]
    public class AboutUsImageEntity
    {
        [JsonProperty("imgThumbUrl")]
        public string ImgThumbUrl { get; set; }

        [JsonProperty("imgLargeUrl")]
        public string ImgLargeUrl { get; set; }

        [JsonProperty("hostUrl")]
        public string HostUrl { get; set; }

        [JsonProperty("originalImgPath")]
        public string OriginalImgPath { get; set; }

        [JsonProperty("showroomImage")]
        public string ShowroomImage { get; set; }

        [JsonProperty("isMainBanner")]
        public bool IsMainBanner { get; set; }
    }
}
