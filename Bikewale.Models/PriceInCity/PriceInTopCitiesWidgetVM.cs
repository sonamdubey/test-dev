using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models.PriceInCity
{
    public class PriceInTopCitiesWidgetVM
    {
        public IEnumerable<PriceQuoteOfTopCities> PriceQuoteList { get; set; }
        public string BikeName { get; set; }
    }
}
