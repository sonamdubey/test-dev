using Newtonsoft.Json;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd Aug 2017
    /// Description : Address schema for local bussiness and places 
    /// </summary>
    public class Address
    {
        [JsonProperty("@type")]
        public string Type { get { return "PostalAddress"; } }

        [JsonProperty("addressLocality", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("addressRegion", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("addressCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get { return "IN"; } }

        [JsonProperty("postalCode", NullValueHandling = NullValueHandling.Ignore)]
        public string PinCode { get; set; }

        [JsonProperty("streetAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string StreetAddress { get; set; }

    }
}
