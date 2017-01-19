using BikewaleOpr.Entities;
using Newtonsoft.Json;
using System;

namespace BikewaleOpr.Entity.ContractCampaign
{
    /// <summary>
    /// Created by  :   Sumit Kate on 18 Jan 2017
    /// Description :   Dealer Campaign Details Entity
    /// </summary>
    public class DealerCampaignDetailsEntity : DealerCampaignEntity
    {
        [JsonProperty("PackageName")]
        public string PackageName { get; set; }
        [JsonProperty("ContractId")]
        public uint ContractID { get; set; }
        [JsonProperty("StartDate")]
        public String StrStartDate { get { return ContractStartDate.ToString(); } }
        [JsonIgnore]
        public DateTime ContractStartDate { get; set; }

        [JsonProperty("EndDate")]
        public String StrEndDate { get { return ContractEndDate.ToString(); } }
        [JsonIgnore]
        public DateTime ContractEndDate { get; set; }
        [JsonProperty("ContractStatus")]
        public uint ContractStatus { get; set; }
        [JsonProperty("NoOfRules")]
        public uint RulesCount { get; set; }
        [JsonProperty("Status")]
        public string ContractStatusText { get; set; }
        [JsonProperty("DealerId")]
        public uint DealerId { get; set; }
        [JsonProperty("DailyLeads")]
        public uint DailyLeads { get; set; }
    }
}
