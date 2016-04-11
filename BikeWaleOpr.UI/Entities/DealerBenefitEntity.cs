using BikeWaleOpr.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikewaleOpr.Entities
{
    /// <summary>
    /// Created by: Sangram
    /// Created on: 10 March 2016
    /// Summary   : Entity to hold benefit/ usp data
    /// </summary>
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