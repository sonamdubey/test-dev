using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model.v2
{
    /// <summary>
    /// Created By : Lucky Rathore
    /// Created On : 15 Apr 2016.
    /// Description : Version 2 of SpecsCategory DTO Needed to provide only required Detail.
    /// </summary>
    public class SpecsCategory
        {
            [JsonIgnore, JsonProperty("categoryName")]
            public string CategoryName { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("specs")]
            public IEnumerable<Specs> Specs { get; set; }
        }
}
