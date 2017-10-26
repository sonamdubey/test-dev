using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Campaign
{
    public class DetailsDto
    {
        [JsonProperty("dealer", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public DealerCampaignBase Dealer { get; set; }
        [JsonProperty("esCamapign", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public PreRenderCampaignBase EsCamapign { get; set; }
    }
}
