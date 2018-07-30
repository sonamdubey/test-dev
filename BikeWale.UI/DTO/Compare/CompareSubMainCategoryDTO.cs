using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Sub-Main Category DTO
    /// Modified By :   Rajan Chauhan on 04 Apr 2018
    /// Description :   Changed SpecCategory to IList from List
    /// </summary>
    public class CompareSubMainCategoryDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("specCategory")]
        public IList<CompareSubCategoryDTO> SpecCategory { get; set; }
    }
}
