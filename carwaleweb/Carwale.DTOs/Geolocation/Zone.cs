using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Geolocation
{
    //I dont want to do mess but m doing bcz m forced to.
    public class Zone
    {
        public int CityId { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
    }
}
