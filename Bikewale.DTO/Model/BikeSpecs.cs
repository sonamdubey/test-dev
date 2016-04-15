using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created By : Lucky Rathore on 14 Apr 2016
    /// Description  : DTO for specification, color, models version and Feature of bike version.
    /// </summary>
    public class BikeSpecs
    {
        private Bikewale.DTO.Model.v2.SpecsCategory _specs;
        
        [JsonProperty("versionList")]
        public List<Bikewale.DTO.Version.VersionMinSpecs> ModelVersions { get; set; }

        [JsonProperty("modelColors")]
        public IEnumerable<NewModelColor> ModelColors { get; set; }
        
        [JsonIgnore,JsonProperty("features")]
        public Features objFeatures { get; set; }

        [JsonProperty("featuresList")]
        public IEnumerable<Specs> FeaturesList { get { return objFeatures!=null ? objFeatures.FeaturesList : null; } }

        [JsonIgnore, JsonProperty("specs")]
        public Bikewale.DTO.Model.v2.Specifications objSpecs { get; set; }

        [JsonProperty("specsCategory")]
        public List<Bikewale.DTO.Model.v2.SpecsCategory> SpecsCategory { get { return objSpecs != null ? objSpecs.SpecsCategory : null; } }

        [JsonProperty("isExShowroomPrice")]
        public bool IsExShowroomPrice { get; set; }


        [JsonProperty("isAreaExists")]
        public bool IsAreaExists { get; set; }
        }
}
