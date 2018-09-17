using Newtonsoft.Json;

namespace Bikewale.DTO.Dealer
{
    /// <summary>
    /// Created By  : Pratibha Verma on 30 August
    /// DEscription : Dealer's Detail
    /// </summary>
    public class NewBikeDealerDetails
    {
        [JsonProperty("name")]
        public string DealerName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("pincode")]
        public string PinCode { get; set; }

        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
