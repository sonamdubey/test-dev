using Newtonsoft.Json;
namespace Carwale.DTOs.PriceQuote
{
    public class EmiCalculatorModelDataDto
    {
        [JsonProperty("downPaymentMinValue")]
        public int DownPaymentMinValue { get; set; }
        [JsonProperty("downPaymentMaxValue")]
        public long DownPaymentMaxValue { get; set; }
        [JsonProperty("downPaymentDefaultValue")]
        public int DownPaymentDefaultValue { get; set; }
        [JsonProperty("isMetallic")]
        public bool IsMetallic { get; set; }
        [JsonProperty("thirdPartyEmiDetails")]
        public ThirdPartyEmiDetailsDtoV2 ThirdPartyEmiDetails { get; set; }
        [JsonProperty("isEligibleForThirdPartyEmi")]
        public bool IsEligibleForThirdPartyEmi { get; set; }
    }
}
