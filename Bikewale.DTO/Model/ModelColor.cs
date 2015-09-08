using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Model Color DTO
    /// Author  :   Sumit Kate
    /// Date    :   08 Sept 2015
    /// </summary>
    public class ModelColor
    {
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("colorName")]
        public string ColorName { get; set; }
        [JsonProperty("hexCode")]
        public string HexCode { get; set; }
    }
}
