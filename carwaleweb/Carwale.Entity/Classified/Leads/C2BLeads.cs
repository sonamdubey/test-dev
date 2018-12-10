using System;
using Newtonsoft.Json;

namespace Carwale.Entity.Classified.Leads
{
    [Serializable]
    public class C2BLead
    {
        [JsonProperty("company")]
        public string CustomerName { get; set; }
        [JsonProperty("contacted_date")]
        public DateTime RequestDateTime { get; set; }
        [JsonProperty("virtual_number")]
        public string CustomerMobile { get; set; }
        [JsonIgnore]
        public string SourceIdentifier { get { return "c2b"; } }
    }
}
