using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
