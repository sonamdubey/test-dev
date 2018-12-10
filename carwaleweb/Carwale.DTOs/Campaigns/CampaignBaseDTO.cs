using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Campaigns
{
    public class CampaignBaseDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        [JsonProperty("type")]
        public short Type { get; set; }
        [JsonProperty("contactName")]
        public string ContactName { get; set; }
    }
}
