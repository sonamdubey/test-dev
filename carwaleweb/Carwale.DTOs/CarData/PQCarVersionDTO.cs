using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class PQCarVersionDTO
    {
        [JsonProperty("versionId")]
        public int Id { get; set; }

        [JsonProperty("versionName")]
        public string Name { get; set; }

        [JsonProperty("specSummary")]
        public string SpecsSummary;

        [JsonProperty("reviewRate")]
        public string ReviewRate;
    }
}
