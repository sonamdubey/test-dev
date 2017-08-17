using Newtonsoft.Json;

namespace BikewaleOpr.DTO.BikeData
{
    public class VersionBase
    {
        [JsonProperty("versionId")]
        public int VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }
    }
}
