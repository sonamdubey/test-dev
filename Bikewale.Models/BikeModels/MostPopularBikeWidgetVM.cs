using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 24 Mar 2017
    /// Description :   Most Popular Bike Widget VM
    /// Modified by Sajal Gupta on 25-04-2017
    /// Decription : Added CtaText
    /// </summary>
    public class MostPopularBikeWidgetVM
    {
        public uint PageCatId { get; set; }
        public PQSourceEnum PQSourceId { get; set; }
        public bool ShowCheckOnRoadCTA { get; set; }
        public bool ShowPriceInCityCTA { get; set; }
        public IEnumerable<MostPopularBikesBase> Bikes { get; set; }
        public string WidgetHeading { get; set; }
        public string WidgetHref { get; set; }
        public string WidgetLinkTitle { get; set; }
        public string CtaText { get; set; }
        public uint CityId { get; set; }
        public string ReturnUrlForAmpPages { get; set; }
    }
}
