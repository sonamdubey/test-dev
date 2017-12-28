using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Createed by : Aditi Srivastava on 25 Mar 2017
    /// summary     : View model for bikes by body style
    /// Modified by : Vivek Singh Tomar on 27th Oct 2017
    /// Description: Added city and return url for redirection from popups on amp pages
    /// Modified by : Rajan Chauhan on 28 Dec 2017
    /// Description : Added WidgetLinktext attribute to BestBikesEditorialWidgetVM and PopularBodyStyleVM
    /// </summary>
    public class PopularBodyStyleVM
    {
        public IEnumerable<MostPopularBikesBase> PopularBikes { get; set; }
        public string WidgetHeading { get; set; }
        public string WidgetLinkTitle { get; set; }
        public string WidgetLinkText { get; set; }
        public string WidgetHref { get; set; }
        public string BodyStyleText { get; set; }
        public string BodyStyleLinkTitle { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public bool ShowCheckOnRoadCTA { get; set; }
        public PQSourceEnum PQSourceId { get; set; }
        public uint CityId { get; set; }
        public string ReturnUrlForAmpPages { get; set; }
    }

    public class BestBikesEditorialWidgetVM
    {
        public string WidgetHeading { get; set; }
        public string WidgetLinkTitle { get; set; }
        public string WidgetLinkText { get; set; }
        public string WidgetHref { get; set; }
        public IEnumerable<BestBikeEntityBase> BestBikes { get; set; }
    }
}
