using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Color DTO
    /// </summary>
    public class BikeColorDTO
    {
        [JsonProperty("colorId")]
        public int ColorId { get; set; }
        [JsonProperty("versionId")]
        public uint VersionId { get; set; }
        [JsonProperty("color")]
        public string Color { get; set; }
        [JsonProperty("hexCodes")]
        public IEnumerable<string> HexCodes { get; set; }
    }

    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Bike Model Color DTO
    /// </summary>
    public class BikeModelColorDTO
    {
        [JsonProperty("modelColorId")]
        public int ModelColorId { get; set; }
        [JsonProperty("hexCode")]
        public string HexCode { get; set; }
    }
}
