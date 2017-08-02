using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Comparison.DTO
{
    public class BikeModelDTO
    {
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }

        [JsonProperty("bikeName")]
        public string BikeName { get; set; }

        [JsonProperty("versionId")]
        public string VersionId { get; set; }

        [JsonProperty("price")]
        public uint Price { get; set; }
    }
}
