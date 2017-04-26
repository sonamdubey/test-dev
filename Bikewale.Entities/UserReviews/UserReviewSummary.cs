using Bikewale.Entities.BikeData;
using System.Collections.Generic;

namespace Bikewale.Entities.UserReviews
{
    public class UserReviewSummary
    {
        public UserReviewOverallRating OverallRating { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public string OriginalImgPath { get; set; }
        public string HostUrl { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string TipsDescriptionSmall { get; set; }
        public string Tips { get; set; }
        public ushort OverallRatingId { get; set; }
        public IEnumerable<UserReviewQuestion> Questions { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public UserReviewPageSourceEnum PageSource { get; set; }
    }
}
