using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class ListItem
    {
        [JsonProperty("@type")]
        public string Type { get { return "ListItem"; } }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public uint? Position { get; set; }
    }

}

