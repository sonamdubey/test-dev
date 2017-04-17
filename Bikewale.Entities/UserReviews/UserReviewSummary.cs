using System.Collections.Generic;

namespace Bikewale.Entities.UserReviews
{
    public class UserReviewSummary
    {
        public UserReviewOverallRating OverallRating { get; set; }
        public uint ModelId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Tips { get; set; }
        public ushort OverallRatingId { get; set; }
        public IEnumerable<UserReviewQuestion> Questions { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
