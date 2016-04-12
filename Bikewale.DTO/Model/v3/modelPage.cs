using Bikewale.DTO.BikeData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model.v3
{
    /// <summary>
    /// Created by  :   Sangram Nandkhile on 29 Jan 2016
    /// Description :   This new DTO for Model Page API v3
    /// New model multi-tone colors are replaced with old model colours.
    /// </summary>
    public class ModelPage
    {

        [JsonProperty("smallDescription")]
        public string SmallDescription { get; set; }

        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("reviewRate")]
        public double ReviewRate { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("isDiscontinued")]
        public bool IsDiscontinued { get; set; }

        [JsonProperty("photos")]
        public List<CMSModelImageBase> Photos { get; set; }

        [JsonProperty("overviewList")]
        public Overview overviewList { get; set; }

        [JsonProperty("versionList")]
        public List<Bikewale.DTO.Version.VersionMinSpecs> ModelVersions { get; set; }

    }
}
