using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.Entities.AutoComplete
{
    public class Payload
    {
        [JsonProperty("makeId")]
        public int MakeId { get; set; }

        [JsonProperty("modelId")]
        public int ModelId { get; set; }

        [JsonProperty("makeMaskingName")]
        public string MakeMaskingName { get; set; }

        [JsonProperty("modelMaskingName")]
        public string ModelMaskingName { get; set; }
    }
}
