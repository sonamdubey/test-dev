using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CarData
{
    [Serializable]
    public class CarReviewBase
    {
        [JsonProperty("overallRating")]
        public float OverallRating { get; set; }
        [JsonProperty("reviewCount")]
        public ushort ReviewCount { get; set; }
        [JsonIgnore]
        public float ReviewRate { get; set; }
    }
}
