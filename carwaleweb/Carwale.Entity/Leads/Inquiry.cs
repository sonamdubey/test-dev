using Carwale.Entity.Campaigns;
using Newtonsoft.Json;
namespace Carwale.Entity.Leads
{
    [JsonObject]
    public class Inquiry
    {
        public CarIdEntity CarDetail { get; set; }
        public Seller Seller { get; set; }
    }
}
