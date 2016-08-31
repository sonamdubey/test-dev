using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.AutoComplete.V2
{
    /// <summary>
    /// Created By : Sajal Gupta
    /// Created On : 01/08/2016
    /// Description : Dto for extracting makeId and modelId only.
    /// </summary>
    public class Payload
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }
    }
}
