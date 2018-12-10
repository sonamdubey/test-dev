using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class CarMakeModelAdDTO : CarMakeModelDTO
    {
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("adPosition")]
        public int AdPosition { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
    }
}
