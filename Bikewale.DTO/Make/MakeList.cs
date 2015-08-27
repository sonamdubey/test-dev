using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Make
{
    public class MakeList
    {
        [JsonProperty("makes")]
        public IEnumerable<MakeBase> Makes { get; set; }
    }
}
