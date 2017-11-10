using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 18 May 2017
    /// Summary :- Generic Bike Model IndexBestBikesVM;
    /// </summary>
    public class IndexBestBikesVM : ModelBase
    {
        public IEnumerable<BestBikeEntityBase> objBestBikesList { get; set; }
        public string PageMaskingName { get; set; }
        public string PageName { get; set; }
        public string Content { get; set; }
        public string bannerImagePos { get; set; }
        public PQSourceEnum pqSource { get; set; }
        public string bannerImage { get; set; }
        public BestBikeWidgetVM BestBikes { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public RecentNewsVM News { get; set; }
    }
}
