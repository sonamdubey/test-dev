using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Campaigns
{
    public class PredictionData
    {
        [JsonProperty("score")]
        public double Score { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
