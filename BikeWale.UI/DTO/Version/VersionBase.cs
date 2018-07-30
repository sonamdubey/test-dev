using Newtonsoft.Json;

namespace Bikewale.DTO.Version
{
    public class VersionBase
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }
    }
}
