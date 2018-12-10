using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    [Serializable]
    public class ZoneDTO
    {
        [JsonProperty("id",Order=1)]
        public int Id { get; set; }

        [JsonProperty("name",Order=2)]
        public string Name { get; set; }
    }
}
