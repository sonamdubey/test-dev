using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class WebSite
    {
        [JsonProperty("@type")]
        public string Type { get { return "WebSite"; } }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("potentialAction", NullValueHandling = NullValueHandling.Ignore)]
        public PotentialAction PotentialAction { get; set; }

    }
}
