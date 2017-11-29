using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Bike Color Category DTO
    /// </summary>
    public class CompareBikeColorCategoryDTO
    {
        [JsonProperty("bikes")]
        public List<CompareBikeColorDTO> bikes { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
