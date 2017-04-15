
namespace Bikewale.Entities.UserReviews
{
    public class UserReviewPriceRange
    {
        public uint Id { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public uint QuestionId { get; set; }
    }
}
