using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class ContactPoint
    {
        [JsonProperty("@type")]
        public string Type { get { return "ContactPoint"; } }

        [JsonProperty("telephone", NullValueHandling = NullValueHandling.Ignore)]
        public string Telephone { get; set; }

        [JsonProperty("contactType", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactType { get; set; }

        [JsonProperty("contactOption", NullValueHandling = NullValueHandling.Ignore)]
        public string ContactOption { get; set; }

        [JsonProperty("areaServed", NullValueHandling = NullValueHandling.Ignore)]
        public string AreaServed { get; set; }

        [JsonProperty("availableLanguage", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<string> AvailableLanguage { get; set; }
    }
}
