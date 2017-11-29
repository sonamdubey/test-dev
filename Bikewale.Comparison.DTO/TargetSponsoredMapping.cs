using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.Comparison.DTO
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 02-Aug-2017
    /// Summary: Too Target and Sponsored models
    /// 
    /// </summary>
    public class TargetSponsoredMappingDTO
    {
        [JsonProperty("sponsoredModelVersion")]
        public IEnumerable<BikeModelDTO> SponsoredModelVersion { get; set; }
        [JsonProperty("targetVersionsMapping")]
        public IEnumerable<BikeModelVersionMappingDTO> TargetVersionsMapping { get; set; }
    }
}
