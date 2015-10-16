using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    public class Specs
    {
        [JsonProperty("displayText")]
        public string DisplayText { get; set; }

        [JsonProperty("displayValue")]
        public string DisplayValue { get; set; }
    }
}
