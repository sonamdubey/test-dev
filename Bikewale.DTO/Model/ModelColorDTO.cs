using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 5th Oct 2017
    /// </summary>
    public class ModelColorDto
    {
        [JsonProperty("modelColorId")]
        public uint ModelColorId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("colorCodes")]
        public IEnumerable<ColorCode> ColorCodes { get; set; }
    }
}
