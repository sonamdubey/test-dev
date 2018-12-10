using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.DTOs.PriceQuote
{
    public class EMIInformationDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("toolTipMessage")]
        public string TooltipMessage { get; set; }
        [JsonProperty("linkText")]
        public string LinkText { get; set; }
        [JsonProperty("rupeeSymbol")]
        public string RupeeSymbol { get; set; }
    }

    public class EmiInformationDto_V1
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("toolTipMessage")]
        public string TooltipMessage { get; set; }
        [JsonProperty("rupeeSymbol")]
        public string RupeeSymbol { get; set; }
    }

    public class EmiInformationDtoV2 : EmiInformationDto_V1
    {
        [JsonProperty("suffix")]
        public string Suffix { get; set; }
    }
}
