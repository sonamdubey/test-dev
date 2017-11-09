using Bikewale.Entities.NewBikeSearch;

namespace Bikewale.Models.NewBikeSearch
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 08-Nov-2017
    /// Summary: New bike search view model
    /// 
    /// </summary>
    public class NewBikeSearchVM : ModelBase
    {
        public SearchOutputEntity Bikes { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
    }
}
