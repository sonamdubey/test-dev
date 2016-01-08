using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.UrlShortner
{
    public class UrlShortnerResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("longUrl")]
        public string LongUrl { get; set; }
    }
}
