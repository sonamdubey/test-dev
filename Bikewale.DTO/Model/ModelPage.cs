using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.DTO.BikeData;

namespace Bikewale.DTO.Model
{
    public class ModelPage 
    {
        [JsonProperty("modelDesc")]
        public ModelDescription ModelDesc { get; set; }

        [JsonProperty("modelDetails")]
        public ModelDetails ModelDetails { get; set; }

        [JsonProperty("versionList")]
        public List<Bikewale.DTO.Version.VersionMinSpecs> ModelVersions { get; set; }

        [JsonProperty("modelVersionSpecs")]
        public Version.VersionSpecifications ModelVersionSpecs { get; set; }

        [JsonProperty("photos")]
        public List<Bikewale.DTO.CMS.Photos.CMSModelImageBase> Photos { get; set; }
        
        [JsonProperty("modelColors")]
        public IEnumerable<ModelColor> ModelColors { get; set; }

        [JsonProperty("upcomingBike")]
        public UpcomingBike UpcomingBike { get; set; }
    }
}
