using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppNotification.Entity
{
    public class MobileAppNotificationBase
    {
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("smallPicUrl")]
        public string smallPicUrl { get; set; }
        [JsonProperty("detailUrl")]
        public string detailUrl { get; set; }
        [JsonProperty("alertTypeId")]
        public int alertTypeId { get; set; }
        [JsonProperty("alertId")]
        public int alertId { get; set; }
        [JsonProperty("isFeatured")]
        public bool isFeatured { get; set; }
        [JsonProperty("largePicUrl")]
        public string largePicUrl { get; set; }
        [JsonProperty("publishDate")]
        public string publishDate { get; set; }
    }
}
