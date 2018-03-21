using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Modified by : Rajan Chauhan on 21 Mar 2017
    /// Description : Changed Specs List to IEnumerable
    /// </summary>
    public class SpecsCategory
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("specs")]
        public IEnumerable<Specs> Specs { get; set; }
    }
}
