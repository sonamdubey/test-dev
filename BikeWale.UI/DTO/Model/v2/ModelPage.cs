using Bikewale.DTO.BikeData;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model.v2
{
    /// <summary>
    /// Created by  :   Sumit Kate on 29 Jan 2016
    /// Description :   This new DTO for Model Page API. 
    /// New model multi-tone colors are replaced with old model colours.
    /// </summary>
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
        public IEnumerable<Bikewale.DTO.CMS.Photos.CMSModelImageBase> Photos { get; set; }

        [JsonProperty("modelColors")]
        public IEnumerable<NewModelColor> ModelColors { get; set; }

        [JsonProperty("upcomingBike")]
        public UpcomingBike UpcomingBike { get; set; }

        [JsonProperty("overview")]
        public Overview objOverview { get; set; }

        [JsonProperty("features")]
        public Features objFeatures { get; set; }

        [JsonProperty("specs")]
        public Bikewale.DTO.Model.Specifications objSpecs { get; set; }
    }
}
