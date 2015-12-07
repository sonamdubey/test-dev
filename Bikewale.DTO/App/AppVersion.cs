using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.App
{
    /// <summary>
    /// Author      :   Sumit Kate
    /// Description :   APP Version DTO
    /// Created On  :   07 Dec 2015
    /// </summary>
    public class AppVersion
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("isSupported")]
        public bool IsSupported { get; set; }
        [JsonProperty("isLatest")]
        public bool IsLatest { get; set; }
    }
}
