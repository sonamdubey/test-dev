using Carwale.Entity.CarData;
using Carwale.Entity.CompareCars;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.CarData
{
    public class HotCarComparisons
    {
        [JsonProperty("hotComparisons")]
        public List<HotCarComparison> HotComparisons;
    }
}
