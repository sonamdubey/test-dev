using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.DTO
{
    public class BikeModelVersionMappingDTO
    {
        [JsonProperty("sponsoredModelId")]
        public uint SponsoredModelId { get; set; }

        [JsonProperty("sponsoredVersionId")]
        public uint SponsoredVersionId { get; set; }

        [JsonProperty("impressionUrl")]
        public string ImpressionUrl { get; set; }
    }
}
