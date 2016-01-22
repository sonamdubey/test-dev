using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Compare
{
    /// <summary>
    /// Created By  :   Sumit Kate on 22 Jan 2016
    /// Description :   Compare Main Category DTO
    /// </summary>
    public class CompareMainCategoryDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("spec")]
        public List<CompareSubMainCategoryDTO> Spec { get; set; }
    }
}
