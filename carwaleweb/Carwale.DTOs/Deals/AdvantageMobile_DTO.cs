using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class AdvantageMobile_DTO
    {
        [JsonProperty("carsWithBestSavings")]
        public List<DealsSummaryMobile_DTO> CarsWithBestSavings { get; set; }
        [JsonProperty("cityDetails")]
        public List<City> CityDetails { get; set; }
    }
}
