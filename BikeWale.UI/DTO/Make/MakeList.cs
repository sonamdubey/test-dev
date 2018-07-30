using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Make
{
    public class MakeList
    {
        [JsonProperty("makes")]
        public IEnumerable<MakeBase> Makes { get; set; }
    }
}
