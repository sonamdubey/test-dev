using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewByMakeVM : ModelBase
    {
        public IEnumerable<PopularBikesWithUserReviews> PopularBikes { get; set; }
        public IEnumerable<PopularBikesWithUserReviews> OtherBikes { get; set; }
        public OtherMakesVM OtherMakes { get; set; }
        public BikeMakeEntityBase Make { get; set; }
    }
}
