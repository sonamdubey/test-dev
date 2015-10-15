using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bikewale.DTO.Model
{
    public class SpecsCategory
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("specs")]
        public List<Specs> Specs { get; set; }
    }
}
