using Newtonsoft.Json;
namespace Carwale.DTOs.PriceQuote
{
    public class VersionPriceDto
    {
        [JsonProperty("priceOverview")]
        public PriceOverviewDtoV3 PriceOverview { get; set; }

        [JsonProperty("emiInformation")]
        public EmiInformationDtoV2 EmiInformation { get; set; }
    }
}
