using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created on : 15 march 2016
    /// Summary : To get Dealers Benifits.
    /// </summary>
    [Serializable, DataContract]
    public class DealerBenefitEntity
    {
        [JsonProperty("benefitId"), DataMember]
        public int BenefitId { get; set; }
        [JsonProperty("dealerId"), DataMember]
        public int DealerId { get; set; }
        [JsonProperty("catId"), DataMember]
        public int CatId { get; set; }
        [JsonProperty("categoryText"), DataMember]
        public string CategoryText { get; set; }
        [JsonProperty("benefitText"), DataMember]
        public string BenefitText { get; set; }
        [JsonProperty("city"), DataMember]
        public string City { get; set; }
        [JsonProperty("entryDate"), DataMember]
        public DateTime EntryDate { get; set; }

    }
}
