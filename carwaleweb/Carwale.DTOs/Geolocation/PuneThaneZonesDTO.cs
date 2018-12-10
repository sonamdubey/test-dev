using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class PuneThaneZonesDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
