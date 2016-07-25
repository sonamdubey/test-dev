using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 march 2016
    /// Summary : To get Dealers Benifits.
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
