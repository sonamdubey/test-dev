///Created By Vivek Gupta on 20-05-2016
///This entity is used to carry data for top city prices
namespace Bikewale.Entities.PriceQuote
{
    public class PriceQuoteOfTopCities
    {
        public string CityName { get; set; }

        public string CityMaskingName { get; set; }

        public uint OnRoadPrice { get; set; }
    }
}
