using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class TrendingCarDTO
    {
        [JsonProperty("trendingCars")]
        public List<CarMakeModelDTO> TrendingModels { get; set; }

        [JsonProperty("sponsoredModel")]
        public CarMakeModelDTO SponsoredModel { get; set; }
    }
}
