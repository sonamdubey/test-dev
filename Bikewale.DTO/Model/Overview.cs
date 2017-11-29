using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bikewale.DTO.Model
{
    public class Overview
    {
        [JsonProperty("overviewList")]
        public List<Specs> OverviewList { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
