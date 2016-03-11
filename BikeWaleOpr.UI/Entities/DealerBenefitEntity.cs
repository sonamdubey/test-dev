using BikeWaleOpr.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    public class DealerBenefitEntity
    {
        [JsonProperty("benefitId")]
        public uint BenefitId { get; set; }

        [JsonProperty("catId")]
        public uint CatId { get; set; }

        [JsonProperty("benefitText")]
        public string BenefitText { get; set; }

        [JsonProperty("categoryText")]
        public string CategoryText { get; set; }
    }
}