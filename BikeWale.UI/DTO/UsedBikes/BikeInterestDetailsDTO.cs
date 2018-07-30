
using Newtonsoft.Json;
namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 Sep 2016
    /// Description :   BikeInterestDetails DTO
    /// </summary>
    public class BikeInterestDetailsDTO
    {
        [JsonProperty("seller")]
        public UsedBikeSellerBaseDTO Seller { get; set; }
        [JsonProperty("buyer")]
        public DTO.Customer.CustomerBase Buyer { get; set; }
        [JsonProperty("shownInterest")]
        public bool ShownInterest { get; set; }
    }
}
