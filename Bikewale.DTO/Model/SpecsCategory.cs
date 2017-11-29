using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class SpecsCategory
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("specs")]
        public List<Specs> Specs { get; set; }
    }
}
