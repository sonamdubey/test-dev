
namespace Bikewale.Entities.UserReviews
{
    public class UserReviewOverallRating
    {
        public uint Id { get; set; }
        public ushort Value { get; set; }
        public string Description { get; set; }
        public string Heading { get; set; }
    }

    public enum UserReviewQuestionType
    {
        Rating = 1,
        Review = 2
    }
}
