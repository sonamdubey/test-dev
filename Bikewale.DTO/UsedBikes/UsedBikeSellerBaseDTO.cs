
using Newtonsoft.Json;
namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 Sep 2016
    /// Description :   UsedBikeSellerBase DTO
    /// </summary>
    public class UsedBikeSellerBaseDTO
    {
        [JsonProperty("details")]
        public DTO.Customer.CustomerBase Details { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("contactPerson")]
        public string ContactPerson { get; set; }
    }
}
