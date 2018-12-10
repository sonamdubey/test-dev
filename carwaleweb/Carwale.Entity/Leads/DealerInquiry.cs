using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carwale.Entity.Leads
{
    [JsonObject]
    public class DealerInquiry
    {
        public CustomersBasicInfo UserInfo { get; set; }
        public Location UserLocation { get; set; }
        public LeadSource LeadSource { get; set; }
        public List<Inquiry> CarInquiry { get; set; }
        public string EncryptedLeadId { get; set; }
        public Dictionary<string, string> Others { get; set; }
    }
}
