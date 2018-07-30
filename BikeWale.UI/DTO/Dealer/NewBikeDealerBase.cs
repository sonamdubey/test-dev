using Newtonsoft.Json;

namespace Bikewale.DTO.Dealer
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On : 7th Oct 2015
    /// Summary : NewBike Dealers Base properties 
    /// </summary>
    public class NewBikeDealerBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("contactNo")]
        public string ContactNo { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("workingHours")]
        public string WorkingHours { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("bikeMake")]
        public string BikeMake { get; set; }

        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }

    }
}
