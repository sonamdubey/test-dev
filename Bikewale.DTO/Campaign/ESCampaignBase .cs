using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Campaign
{
    public class ESCampaignBase 
    {
        [JsonProperty("floatingBtnText", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FloatingBtnText { get; set; }

        [JsonProperty("leadSourceId", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ushort LeadSourceId { get; set; }

        [JsonProperty("captionText", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string CaptionText { get; set; }

   

       
    }
}
