﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Sub-Category DTO
    /// </summary>
    public class CompareSubCategoryDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("compareSpec")]
        public List<CompareBikeDataDTO> CompareSpec { get; set; }
    }
}
