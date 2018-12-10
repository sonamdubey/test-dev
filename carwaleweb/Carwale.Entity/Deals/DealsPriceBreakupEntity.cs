using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Carwale.Entity.Deals
{
     [Serializable]
    public class DealsPriceBreakupEntity
    {
         [JsonProperty("dealsPriceBreakupList")]
         public List<KeyValuePair<string, string>> BreakUpList { get; set; }
    }
}
