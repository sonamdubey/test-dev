using Carwale.DTOs.Campaigns;
using Carwale.DTOs.Geolocation;

namespace Carwale.DTOs.PriceQuote
{
    public class OptionalPricesDto
    {
        public Entity.Price.PriceQuote PriceQuote { get; set; }
        public bool IsCampaignPresent { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public CityAreaDTO UserLocation { get; set; }
    }
}
