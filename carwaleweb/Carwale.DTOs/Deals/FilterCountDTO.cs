using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class FilterCountDTO
    {
        [JsonProperty("makes")]
        public List<StockMakeDTO> Makes { get; set; }
    }
}
