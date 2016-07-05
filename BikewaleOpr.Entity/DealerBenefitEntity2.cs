using Newtonsoft.Json;
using System;

namespace BikewaleOpr.Entity
{
    /// <summary>
    /// Created by: Sangram
    /// Created on: 10 March 2016
    /// Summary   : Entity to hold benefit/ usp data
    /// </summary>
    public class DealerBenefitEntity
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
