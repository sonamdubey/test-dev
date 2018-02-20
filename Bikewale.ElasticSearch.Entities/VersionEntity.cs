using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.ElasticSearch.Entities
{
    /// <summary>
    /// Created By : Deepak Israni on 19 Feb 2018
    /// Description : Entity to store version information.
    /// Modified by: Dhruv Joshi
    /// Dated: 20th Feb 2018
    /// Description: Added flag for version status
    /// </summary>
    public class VersionEntity
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("versionName")]
        public string VersionName { get; set; }
        [JsonProperty("specs")]
        public IEnumerable<SpecsEntity> Specs { get; set; }
        [JsonProperty("priceList")]
        public IEnumerable<PriceEntity> PriceList { get; set; }
        [JsonProperty("versionStatus")]
        public BikeStatus VersionStatus { get; set; }

    }
}
