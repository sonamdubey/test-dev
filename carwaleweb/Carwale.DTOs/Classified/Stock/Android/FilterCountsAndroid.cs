using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock
{
    public class FilterCountsAndroid
    {
        [JsonProperty("filtersData")]
        public FiltersCountAndroidBase FiltersData { get; set; }
    }
}
