using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Bikewale.DTO.App.AppAlert
{
    public class AppIMEIDetailsInput
    {
        [JsonProperty("imei"), Required]
        public string Imei { get; set; }
        [JsonProperty("gcmId"), Required]
        public string GcmId { get; set; }
        [JsonProperty("osType"), Required]
        public string OsType { get; set; }
        [JsonProperty("subsMasterId")]
        public string SubsMasterId { get; set; }
    }
}
