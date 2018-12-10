using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    public class CampaignInput
    {
        [JsonProperty(PropertyName = "platformid")]
        public short PlatformId { get; set; }

        [JsonProperty(PropertyName = "campaignid")]
        public int CampaignId { get; set; }

        [JsonProperty(PropertyName = "cityid")]
        public int CityId { get; set; }

        [JsonProperty(PropertyName = "modelid")]
        public int ModelId { get; set; }

        [JsonProperty(PropertyName = "zoneid")]
        public int ZoneId { get; set; }

        [JsonProperty(PropertyName = "areaId")]
        public int AreaId { get; set; }
    }
}
