using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    public class CityZonesDTOV2 : CityDTO
    {
        [JsonProperty("zones",Order=3)]
        public List<ZoneDTO> Zones { get; set; }
    }
}
