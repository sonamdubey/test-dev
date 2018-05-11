using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class Specifications
    {
        /// <summary>
        /// Modified by : Rajan Chauhan on 21 Mar 2018
        /// Description : Changed SpecsCategory from List to IEnumerable
        /// </summary>
        [JsonProperty("specsCategory")]
        public IEnumerable<SpecsCategory> SpecsCategory { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
