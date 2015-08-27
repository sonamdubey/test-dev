using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Version
{
    public class VersionBase
    {
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }

        [JsonProperty("versionName")]
        public string VersionName { get; set; }          
    }
}
