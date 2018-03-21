using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created By : Lucky Rathore on 14 Apr 2016
    /// Description  : DTO for specification, color, models version and Feature of bike version.
    /// Modified by : Pratibha Verma on 21 Mar 2018
    /// Description : Removed Specification and Features Property
    /// </summary>
    public class BikeSpecs
    {
        [JsonProperty("versionList")]
        public IEnumerable<Bikewale.DTO.Model.v3.VersionDetail> ModelVersions { get; set; }

        [JsonProperty("modelColors")]
        public IEnumerable<NewModelColor> ModelColors { get; set; }
      
        [JsonProperty("featuresList")]
        public IEnumerable<Specs> FeaturesList { get; set; }

        [JsonProperty("specsCategory")]
        public IEnumerable<Bikewale.DTO.Model.v2.SpecsCategory> SpecsCategory { get; set; }

        [JsonProperty("isExShowroomPrice")]
        public bool IsExShowroomPrice { get; set; }

        [JsonProperty("isAreaExists")]
        public bool IsAreaExists { get; set; }
    }
}
