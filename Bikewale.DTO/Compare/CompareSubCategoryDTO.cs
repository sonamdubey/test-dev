using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Sub-Category DTO
    /// Modified By :   Rajan Chauhan on 4 Apr 2018
    /// Description :   Changed CompareSpec to IList from List
    /// </summary>
    public class CompareSubCategoryDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("compareSpec")]
        public IList<CompareBikeDataDTO> CompareSpec { get; set; }
    }
}
