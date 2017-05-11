
using System;
namespace Bikewale.Entities.UserReviews
{
    [Serializable]
    public class UserReviewPriceRange
    {
        public uint Id { get; set; }
        public uint RangeId { get; set; }
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; }
        public uint QuestionId { get; set; }
    }
}
