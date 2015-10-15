using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
