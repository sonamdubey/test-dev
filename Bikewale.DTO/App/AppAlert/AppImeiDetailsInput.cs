using Newtonsoft.Json;

namespace Bikewale.DTO.App.AppAlert
{
    public class AppIMEIDetailsInput
    {
        [JsonProperty("imei")]
        public string Imei { get; set; }
        [JsonProperty("gcmId")]
        public string GcmId { get; set; }
        [JsonProperty("osType")]
        public string OsType { get; set; }
        [JsonProperty("subsMasterId")]
        public string SubsMasterId { get; set; }
    }
}
