using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    // <summary>
    /// Created by: Snehal Dange on 22ns Sep 2017
    /// Description : Bikes Schema
    /// </summary>
    public class Bikes
    {
        [JsonProperty("@type")]
        public string Type { get { return "Product"; } }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

    }
}
