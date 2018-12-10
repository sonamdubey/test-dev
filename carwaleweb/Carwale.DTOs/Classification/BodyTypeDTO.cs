using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classification
{
    public class BodyTypeDTO
    {
        [JsonProperty("id")]
        public ushort Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("newCarSearchUrl")]
        public string LandingUrl { get; set; }
    }
}
