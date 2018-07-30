using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class Features
    {
        /// <summary>
        /// Modified By : Rajan Chauhan on 21 Mar 2018
        /// Description : Changed FeaturesList from List to IEnumerable
        /// </summary>
        [JsonProperty("featuresList")]
        public IEnumerable<Specs> FeaturesList { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
