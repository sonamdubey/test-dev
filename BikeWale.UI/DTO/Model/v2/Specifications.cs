using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model.v2
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 15 Apr 2016
    /// Description : Version 2 of Specification DTO Needed to provide only required Detail.
    /// </summary>
    public class Specifications
    {
        [JsonProperty("specsCategory")]
        public List<DTO.Model.v2.SpecsCategory> SpecsCategory { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
