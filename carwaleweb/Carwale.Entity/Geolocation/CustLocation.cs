using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Geolocation
{
    [JsonObject]
    [Serializable]
    public class CustLocation
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }
}
