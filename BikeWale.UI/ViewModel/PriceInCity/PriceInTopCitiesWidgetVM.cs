using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models.PriceInCity
{
    /// <summary>
    /// Created by Sajal Gupta on 24-03-2017
    /// Wrapper class for widget priceInNearByCities 
    /// </summary>
    public class PriceInTopCitiesWidgetVM
    {
        public IEnumerable<PriceQuoteOfTopCities> PriceQuoteList { get; set; }
        public string BikeName { get; set; }
        public Bikewale.Entities.Pages.BikewalePages Page { get; set; }
    }
}
