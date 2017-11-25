using Bikewale.DTO.BikeData;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Widgets
{
    public class MostPopularBikes
    {
        public MakeBase objMake { get; set; }
        public ModelBase objModel { get; set; }
        public VersionBase objVersion { get; set; }
        public string BikeName { get; set; }
        public string HostURL { get; set; }
        public string OriginalImagePath { get; set; }
        public int ReviewCount { get; set; }
        public double ModelRating { get; set; }
        public Int64 VersionPrice { get; set; }
        public MinSpecs Specs { get; set; } 
        [JsonProperty("popularityIndex")]
        public ushort BikePopularityIndex { get; set; }
        public string CityName { get; set; }
        public string CityMaskingName { get; set; }
    }
}
