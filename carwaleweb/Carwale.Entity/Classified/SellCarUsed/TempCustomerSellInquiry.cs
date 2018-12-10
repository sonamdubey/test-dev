using Carwale.Entity.Enum;
using Newtonsoft.Json;

namespace Carwale.Entity.Classified.SellCarUsed
{
    public class TempCustomerSellInquiry
    {
        [JsonProperty("customerdetails")]
        public SellCarCustomer sellCarCustomer { get; set; }
        [JsonProperty("cardetails")]
        public SellCarInfo sellCarInfo { get; set; }
        public Platform Platform { get; set; }
    }
}
