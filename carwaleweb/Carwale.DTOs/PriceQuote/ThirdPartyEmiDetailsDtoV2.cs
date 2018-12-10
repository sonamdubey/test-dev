using Newtonsoft.Json;
namespace Carwale.DTOs.PriceQuote
{
    public class ThirdPartyEmiDetailsDtoV2 : ThirdPartyEmiDetailsDto
    {
        [JsonProperty("visitWebsiteText")]
        public string VisitWebsiteText { get; set; }
        [JsonProperty("interestToolTipText")]
        public string InterestToolTipText { get; set; }
        [JsonProperty("amountToolTipText")]
        public string AmountToolTipText { get; set; }
        [JsonProperty("totalDownPaymentTooltipText")]
        public string TotalDownPaymentTooltipText { get; set; }
    }
}
