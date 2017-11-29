using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class Specifications
    {
        [JsonProperty("specsCategory")]
        public List<SpecsCategory> SpecsCategory { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
