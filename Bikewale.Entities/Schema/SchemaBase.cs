using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.Schema
{
    public class SchemaBase
    {
        [JsonProperty("@context")]
        public string Context { get { return "http://schema.org"; } }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
