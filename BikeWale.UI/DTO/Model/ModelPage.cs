using Bikewale.DTO.BikeData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Modified by :   Sumit Kate on 29 Jan 2016
    /// Description :   Send the single HexCode for Dual Tone model colours
    /// </summary>
    public class ModelPage
    {
        [JsonProperty("modelDesc")]
        public ModelDescription ModelDesc { get; set; }

        [JsonProperty("modelDetails")]
        public ModelDetails ModelDetails { get; set; }

        [JsonProperty("versionList")]
        public IEnumerable<Bikewale.DTO.Version.VersionMinSpecs> ModelVersions { get; set; }

        [JsonProperty("modelVersionSpecs")]
        public Version.VersionSpecifications ModelVersionSpecs { get; set; }

        [JsonProperty("photos")]
        public IEnumerable<Bikewale.DTO.CMS.Photos.CMSModelImageBase> Photos { get; set; }

        [JsonProperty("modelColors")]
        public IEnumerable<ModelColor> ModelColors { get; set; }

        [JsonProperty("upcomingBike")]
        public UpcomingBike UpcomingBike { get; set; }

        [JsonProperty("overview")]
        public Overview objOverview { get; set; }

        [JsonProperty("features")]
        public Features objFeatures { get; set; }

        [JsonProperty("specs")]
        public Specifications objSpecs { get; set; }
    }
}
