using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
