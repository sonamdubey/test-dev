using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Created by : Vivek Singh Tomar on 5th Oct 2017
    /// </summary>
    public class ColorCode
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("modelColorId")]
        public uint ModelColorId { get; set; }
        [JsonProperty("hexCode")]
        public string HexCode { get; set; }
        [JsonProperty("isActive"), JsonIgnore]
        public bool IsActive { get; set; }
    }
}
