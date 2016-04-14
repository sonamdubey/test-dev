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
    /// Created by  :   Sangram Nandkhile on 12 Apr Jan 2016
    /// Description :   This new DTO for Model Page API v3
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

        [JsonProperty("isExShowroomPrice")]
        public bool IsExShowroomPrice { get; set; }

        [JsonIgnore]
        [JsonProperty("isCityExists")]
        public bool IsCityExists { get; set; }

        [JsonIgnore]
        [JsonProperty("isAreaExists")]
        public bool IsAreaExists { get; set; }

        [JsonProperty("photos")]
        public List<CMSModelImageBase> Photos { get; set; }

        [JsonProperty("overviewList")]
        public List<Specs> OverviewList { get; set; }

        [JsonProperty("versionList")]
        public List<VersionDetail> ModelVersions { get; set; }

    }
}
