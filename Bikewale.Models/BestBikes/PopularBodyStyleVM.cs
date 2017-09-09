using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Createed by : Aditi Srivastava on 25 Mar 2017
    /// summary     : View model for bikes by body style
    /// </summary>
    public class PopularBodyStyleVM
    {
        public IEnumerable<MostPopularBikesBase> PopularBikes { get; set; }
        public string WidgetHeading { get; set; }
        public string WidgetLinkTitle { get; set; }
        public string WidgetHref { get; set; }
        public string BodyStyleText { get; set; }
        public string BodyStyleLinkTitle { get; set; }
        public EnumBikeBodyStyles BodyStyle { get; set; }
        public bool ShowCheckOnRoadCTA { get; set; }
        public PQSourceEnum PQSourceId { get; set; }
    }
}
