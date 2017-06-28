using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ManufacturerCampaign.DTO.SearchCampaign
{
   public class ManufacturerCampaignDetailsDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("maskingNumber")]
        public string MaskingNumber { get; set; }
        [JsonProperty("campaignStartDate")]
        public string CampaignStartDate { get; set; }
        [JsonProperty("campaignEndDate")]
        public string CampaignEndDate { get; set; }
        [JsonProperty("dailyLeadLimit")]
        public int DailyLeadLimit { get; set; }
        [JsonProperty("totalLeadLimit")]
        public int TotalLeadLimit { get; set; }
        [JsonProperty("dailyLeadsDelivered")]
        public int DailyLeadsDelivered { get; set; }
        [JsonProperty("totalLeadsDelivered")]
        public int TotalLeadsDelivered { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("showCampaignOnExshowroom")]
        public bool ShowCampaignOnExshowroom { get; set; }
    }
}
