using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model
{
    public class ModelPage 
    {
        [JsonProperty("modelDesc")]
        public ModelDescription ModelDesc { get; set; }

        [JsonProperty("modelDetails")]
        public ModelDetails ModelDetails { get; set; }

        [JsonProperty("versionList")]
        public List<Bikewale.DTO.Version.ModelVersionList> ModelVersions { get; set; }

        [JsonProperty("modelVersionSpecs")]
        public Version.VersionSpecifications ModelVersionSpecs { get; set; }
    }
}
