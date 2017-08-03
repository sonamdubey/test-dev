using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.DTO.Dealers
{
    public class VersionDaysDTO
    {
        [JsonProperty("dealerId")]
        public uint DealerId { get; set; }
        public IEnumerable<uint> BikeVersionIds { get; set; }
        public IEnumerable<uint> NumberOfDays { get; set; }
    }
}
