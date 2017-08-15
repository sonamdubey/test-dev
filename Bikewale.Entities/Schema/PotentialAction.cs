using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class PotentialAction
    {
        [JsonProperty("@type")]
        public string Type { get { return "SearchAction"; } }

        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public string Target { get; set; }

        [JsonProperty("query-input")]
        public string Query_Input { get { return "required name=search_term_string";  } }
    }
}
