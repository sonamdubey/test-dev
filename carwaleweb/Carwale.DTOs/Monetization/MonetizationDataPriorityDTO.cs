using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Monetization
{
    public class MonetizationDataPriorityDTO
    {
        [JsonProperty("priority")]
        public int Priority { get; set; }
    }
}
