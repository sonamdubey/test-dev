using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.Model
{
    public class ModelDescription
    {
        [JsonProperty("smallDescription")]
        public string SmallDescription { get; set; }

        [JsonProperty("fullDescription")]
        public string FullDescription { get; set; }
    }
}
