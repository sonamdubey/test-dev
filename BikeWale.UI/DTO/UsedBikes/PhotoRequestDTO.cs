using Newtonsoft.Json;

namespace Bikewale.DTO.UsedBikes
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 Sep 2016
    /// Description :   Photo Request DTO
    /// </summary>
    public class PhotoRequestDTO
    {
        [JsonProperty("buyer")]
        public Customer.CustomerBase Buyer { get; set; }
        [JsonProperty("profileId")]
        public string ProfileId { get; set; }
        [JsonProperty("bikeName")]
        public string BikeName { get; set; }
    }
}
