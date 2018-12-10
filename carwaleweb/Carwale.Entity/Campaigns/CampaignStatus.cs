using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Campaigns
{
    public class CampaignStatus
    {
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
