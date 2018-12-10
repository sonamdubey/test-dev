using Carwale.Entity.Enum;
using Newtonsoft.Json;

namespace Carwale.DTOs.PriceQuote
{
    public class EmiWidgetCommonDataDto
    {
        [JsonProperty("tenureMinValue")]
        public int TenureMinValue { get; set; }
        [JsonProperty("tenureMaxValue")]
        public int TenureMaxValue { get; set; }
        [JsonProperty("interestMinValue")]
        public int InterestMinValue { get; set; }
        [JsonProperty("interestMaxValue")]
        public int InterestMaxValue { get; set; }
        [JsonProperty("interestTooltipText")]
        public string InterestTooltipText { get; set; }
        [JsonProperty("downPaymentTooltipText")]
        public string DownPaymentTooltipText { get; set; }
    }
}
