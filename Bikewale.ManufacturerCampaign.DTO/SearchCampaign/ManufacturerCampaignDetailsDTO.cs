using Newtonsoft.Json;

namespace Bikewale.ManufacturerCampaign.DTO.SearchCampaign
{
    /// <summary>
    /// Modified by : Ashutosh Sharma on 25 Jan 2017
    /// Description : Added 'DailyStartTime' and 'DailyEndTime'.
    /// Modified By : Rajan Chauhan on 08 Mar 2018
    /// Description : Added CampaignDaysAcronym
    /// </summary>
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
        [JsonProperty("dailyStartTime")]
        public string DailyStartTime { get; set; }
        [JsonProperty("dailyEndTime")]
        public string DailyEndTime { get; set; }
        [JsonProperty("campaignDaysAcronym")]
        public string CampaignDaysAcronym { get; set; }
    }
}
