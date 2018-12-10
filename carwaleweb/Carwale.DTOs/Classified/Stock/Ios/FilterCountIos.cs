using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Classified.Stock.Ios
{
    /// <summary>
    /// Created By : Sadhana Upadhyay on 28 MAy 2015
    /// </summary>
    public class FilterCountIos
    {
        [JsonProperty("filtersData")]
        public FilterCountIosBase FiltersData { get; set; }
    }
}
