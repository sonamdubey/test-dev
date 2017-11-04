using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class Features
    {
        [JsonProperty("featuresList")]
        public List<Specs> FeaturesList { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
