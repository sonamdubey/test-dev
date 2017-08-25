using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
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
