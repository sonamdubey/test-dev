using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeWale.Entities.AutoBiz
{
    /// <summary>
    /// Created by  :   Sumit Kate on 10 Mar 2016
    /// Description :   Dealer USP's/Benefit Entity
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