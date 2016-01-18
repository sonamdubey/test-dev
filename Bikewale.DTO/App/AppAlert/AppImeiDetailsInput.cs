using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.App.AppAlert
{
    public class AppImeiDetailsInput
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
