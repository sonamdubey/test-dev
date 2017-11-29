using Newtonsoft.Json;

namespace Bikewale.DTO.PriceQuote.BikeBooking
{
    public class DealerDetailsDTO
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("emailId")]
        public string EmailId { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("address1")]
        public string Address1 { get; set; }
        [JsonProperty("address2")]
        public string Address2 { get; set; }
        [JsonProperty("pincode")]
        public string Pincode { get; set; }
        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }
        [JsonProperty("faxNo")]
        public string FaxNo { get; set; }
        [JsonProperty("mobileNo")]
        public string MobileNo { get; set; }
        [JsonProperty("websiteUrl")]
        public string WebsiteUrl { get; set; }
        [JsonProperty("contactHours")]
        public string ContactHours { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("area")]
        public string Area { get; set; }
        [JsonProperty("lattitude")]
        public string Lattitude { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}
