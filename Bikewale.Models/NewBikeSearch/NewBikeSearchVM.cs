using Bikewale.Entities.BikeData;
using Bikewale.Entities.NewBikeSearch;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using System.Collections.Generic;


namespace Bikewale.Models.NewBikeSearch
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 08-Nov-2017
    /// Summary: New bike search view model
    /// 
    /// </summary>
    public class NewBikeSearchVM : ModelBase
    {
        public IEnumerable<BikeMakeEntityBase> PopularBrands { get; set; }
        public SearchOutputEntity BikeSearch { get; set; }
        public IEnumerable<BikeMakeEntityBase> OtherBrands { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public PagerEntity Pager { get; set; }
        public uint TabCount { get; set; }
        public bool IsNewsActive { get; set; }
        public bool IsExpertReviewActive { get; set; }
        public bool IsVideoActive { get; set; }
        public PQSourceEnum PqSource { get; set; }
    }
}
