using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [JsonProperty("sponsoredVersionsMapping")]
        public IEnumerable<BikeModelVersionMappingDTO> SponsoredVersionsMapping { get; set; }
    }
}
