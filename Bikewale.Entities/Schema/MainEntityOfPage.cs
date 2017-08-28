using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class MainEntityOfPage
    {
        [JsonProperty("@type")]
        public string Type { get { return "WebPage"; } }

        [JsonProperty("@id", NullValueHandling = NullValueHandling.Ignore)]
        public string PageUrlId { get; set; }

    }
}
