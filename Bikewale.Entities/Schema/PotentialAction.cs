using Newtonsoft.Json;
using System;

namespace Bikewale.Entities.Schema
{
    /// <summary>
    /// Created By : Sushil Kumar on 15th August 2017
    /// Description : To show potential action to search witin websites and search linkages
    /// </summary>
    public class PotentialAction
    {
        [JsonProperty("@type")]
        public string Type { get { return "SearchAction"; } }

        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetUrl { get; set; }

        [JsonProperty("query-input", NullValueHandling = NullValueHandling.Ignore)]
        public string Query_Input { get { return (!String.IsNullOrEmpty(this.TargetUrl) ? "required name=search_term_string" : null);  } }

    }
}
