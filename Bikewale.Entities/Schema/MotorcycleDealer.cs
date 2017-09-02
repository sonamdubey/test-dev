using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 23rd Aug 2017
    /// Description : MotorcycleDealer schema for dealer locator's dealer details page 
    /// </summary>
    public class MotorcycleDealer : SchemaBase
    {
        [JsonProperty("@type")]
        public string Type { get { return "MotorcycleDealer"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string PageUrl { get; set; }

        [JsonProperty("sameAs", NullValueHandling = NullValueHandling.Ignore)]
        public string DealerWebsiteUrl { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("openingHours", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> OpeningHours { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty("hasMap", NullValueHandling = NullValueHandling.Ignore)]
        public string GoogleMapUrl { get; set; }

        [JsonProperty("logo", NullValueHandling = NullValueHandling.Ignore)]
        public string Logo { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string ImageUrl { get; set; }

        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public Address Address { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        public GeoCoordinates Location { get; set; }

        [JsonProperty("contactPoint", NullValueHandling = NullValueHandling.Ignore)]
        public ContactPoint ContactPoint { get; set; }

        [JsonProperty("areaServed", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaServed { get; set; }

        [JsonProperty("telephone", NullValueHandling = NullValueHandling.Ignore)]
        public string Telephone { get; set; }

        [JsonProperty("priceRange", NullValueHandling = NullValueHandling.Ignore)]
        public string PriceRange { get; set; }

        [JsonProperty("paymentAccepted", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentAccepted { get { return "Cash, Debit Card, Credit Card, Cheque"; } }

        [JsonProperty("currenciesAccepted", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrenciesAccepted { get { return "INR"; } }

    }
}
