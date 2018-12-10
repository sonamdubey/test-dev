using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    public class CampaignInputv2 : CampaignInput
    {
        [JsonProperty(PropertyName = "applicationId")]
        public short ApplicationId { get; set; }        

        [JsonProperty(PropertyName = "makeId")]
        public int MakeId { get; set; }

        [JsonProperty(PropertyName = "pageId")]
        public int PageId { get; set; }
    }
}
