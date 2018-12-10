using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.Sponsored
{
    [Serializable, JsonObject]
    public class SponsoredHistoryModels
    {
        [JsonProperty]
        public int TargetModelId { get; set; }

        [JsonProperty]
        public int FeaturedModelId { get; set; }

        [JsonProperty]
        public int Priority { get; set; }

        [JsonProperty]
        public int AdPosition { get; set; }

        [JsonProperty]
        public DateTime StartDate { get; set; }

        [JsonProperty]
        public DateTime EndDate { get; set; }        
    }
}
