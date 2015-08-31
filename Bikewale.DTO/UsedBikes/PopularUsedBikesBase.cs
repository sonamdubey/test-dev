using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.UsedBikes
{
    public class PopularUsedBikesBase
    {
        [JsonProperty("makeName")]
        public string MakeName { get; set; }

        [JsonProperty("totalBikes")]
        public uint TotalBikes { get; set; }

        [JsonProperty("avgPrice")]
        public double AvgPrice { get; set; }

        [JsonProperty("hostURL")]
        public string HostURL { get; set; }

        [JsonProperty("originalImagePath")]
        public string OriginalImagePath { get; set; }
    }
}
