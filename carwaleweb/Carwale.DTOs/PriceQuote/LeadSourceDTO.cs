using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    public class LeadSourceDTO
    {
        [JsonProperty("leadClickSourceDesc")]
        public string LeadClickSourceDesc { get; set; }
        [JsonProperty("leadClickSourceId")]
        public int LeadClickSourceId { get; set; }
        [JsonProperty("inquirySourceId")]
        public int InquirySourceId { get; set; }
    }
}
