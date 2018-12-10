using Carwale.Entity.CarData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Advertizings.Apps
{
    public class ESCampaign
    {
        [JsonProperty("htmlUrl")]
        public string HtmlUrl { get; set; }

        [JsonProperty("showAd")]
        public string ShowAd { get; set; }

        [JsonProperty("modelDetailUrl")]
        public string ModelDetailUrl { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("image")]
        public CarImageBase Image { get; set; }
    }
}
