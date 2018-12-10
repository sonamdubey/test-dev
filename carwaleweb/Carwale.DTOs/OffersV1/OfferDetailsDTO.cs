using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.OffersV1
{
    public class OfferDetailsDto
    {
        [JsonProperty("heading")]
        public string Heading { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("callOutLine")]
        public string CallOutLine { get; set; }
        [JsonProperty("disclaimer")]
        public string Disclaimer { get; set; }
        [JsonProperty("ctaText")]
        public string CtaText { get; set; }
        [JsonProperty("toolTipText")]
        public List<string> ToolTipText { get; set; }
        [JsonProperty("validityText")]
        public string ValidityText { get; set; }
    }
}
