using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsPriceBreakupDTO
    {
        [JsonProperty("dealsPriceBreakupList")]
        public List<KeyValuePair<string, string>> BreakUpList { get; set; }
    }
}
