using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.Deals
{
    public class DealsEmiDTO
    {        
            [JsonProperty("emiValue")]
            public string EmiValue { get; set; }
            [JsonProperty("emiMessage")]
            public string EmiMessage { get; set; }        
    }
}
