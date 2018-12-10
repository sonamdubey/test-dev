using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs
{
    public class CustomerBaseDTO
    {
        [JsonProperty("custName")]
        public string Name { get; set; }

        [JsonProperty("custMobile")]
        public string Mobile { get; set; }

        [JsonProperty("custEmail")]
        public string Email { get; set; }
    }
}
