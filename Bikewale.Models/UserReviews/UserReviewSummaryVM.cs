using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;

namespace Bikewale.Models
{
    public class UserReviewSummaryVM : ModelBase
    {
        public UserReviewSummary Summary { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
    }

}
