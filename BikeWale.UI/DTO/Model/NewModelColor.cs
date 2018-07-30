using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    /// <summary>
    /// Model Color DTO
    /// Author  : Sushil Kumar  
    /// Date    : 21st Jan 2016 
    /// Modified by :   Sumit Kate on 29 Jan 2016
    /// Description :   Removed the Single tone HexCode property
    /// </summary>
    public class NewModelColor
    {
        [JsonProperty("id")]
        public uint Id { get; set; }
        [JsonProperty("modelId")]
        public uint ModelId { get; set; }
        [JsonProperty("colorName")]
        public string ColorName { get; set; }
        [JsonProperty("hexCodes")]
        public IEnumerable<string> HexCodes { get; set; }
    }

    /// <summary>
    /// Created by  :   Sumit Kate on 05 Oct 2017
    /// Description :   NewModelColor With HasPhoto
    /// </summary>
    public class NewModelColorWithPhoto : NewModelColor
    {
        [JsonIgnore]
        public uint ColorImageId { get; set; }
        [JsonProperty("HasColorPhoto")]
        public bool HasColorPhoto { get; set; }
    }
}
