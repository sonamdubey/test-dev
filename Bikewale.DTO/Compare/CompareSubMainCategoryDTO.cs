using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Sub-Main Category DTO
    /// </summary>
    public class CompareSubMainCategoryDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("specCategory")]
        public List<CompareSubCategoryDTO> SpecCategory { get; set; }
    }
}
