using Bikewale.DTO.BikeData.v2;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Widgets.v2
{
    /// <summary>
    /// Created By : Sushil Kumar
    /// Created On :20th March 2016
    /// Summary : Created second version to pass json data into proper format  and namingConventions
    /// </summary>
    public class MostPopularBikes
    {
        [JsonProperty("make")]
        public MakeBase objMake { get; set; }

        [JsonProperty("model")]
        public ModelBase objModel { get; set; }

        [JsonProperty("version")]
        public VersionBase objVersion { get; set; }

        [JsonProperty("bike")]
        public string BikeName { get; set; }

        [JsonProperty("hostUrl")]
        public string HostURL { get; set; }

        [JsonProperty("imagePath")]
        public string OriginalImagePath { get; set; }

        [JsonProperty("reviewCount")]
        public int ReviewCount { get; set; }

        [JsonProperty("modelRating")]
        public double ModelRating { get; set; }

        [JsonProperty("versionPrice")]
        public Int64 VersionPrice { get; set; }

        [JsonProperty("specs")]
        public MinSpecs Specs { get; set; }

        [JsonProperty("popularityIndex")]
        public ushort BikePopularityIndex { get; set; }
    }
}
