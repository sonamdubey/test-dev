using Newtonsoft.Json;
using System;

namespace BikeWale.DTO.AutoBiz
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Mar 2016
    /// Description :   Dealer USP's/Benefit Entity
    /// </summary>
    public class DealerBenefitEntityDTO
    {
        [JsonProperty("benefitId")]
        public int BenefitId { get; set; }
        [JsonProperty("dealerId")]
        public int DealerId { get; set; }
        [JsonProperty("catId")]
        public int CatId { get; set; }
        [JsonProperty("categoryText")]
        public string CategoryText { get; set; }
        [JsonProperty("benefitText")]
        public string BenefitText { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("entryDate")]
        public DateTime EntryDate { get; set; }
    }
}