
namespace Bikewale.Entities.UserReviews
{
    public class UserReviewsInputEntity
    {
        public UserReviewQuestionDisplayType DisplayType { get; set; }
        public UserReviewQuestionType Type { get; set; }
        public ushort PriceRangeId { get; set; }
        public bool IsRequired { get; set; }
    }
}
