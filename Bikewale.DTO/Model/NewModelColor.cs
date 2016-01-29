using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Model Color DTO
    /// Author  : Sushil Kumar  
    /// Date    : 21st Jan 2016 
    /// </summary>
    public class NewModelColor
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("colorName")]        
        public string ColorName { get; set; }
        [JsonProperty("hexCode")]
        public string HexCode { get; set; }
        [JsonProperty("hexCodes")]
        public IEnumerable<string> HexCodes { get; set; }
    }
}
