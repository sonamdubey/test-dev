using Newtonsoft.Json;
using System;

namespace Bikewale.DTO.Version
{
    public class ModelVersionList
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }

        [JsonProperty("modelName")]
        public string ModelName { get; set; }

        [JsonProperty("price")]
        public UInt64 Price { get; set; }
    }
}
