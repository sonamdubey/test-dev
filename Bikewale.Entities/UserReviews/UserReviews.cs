using System.Collections.Generic;
namespace Bikewale.Entities.UserReviews
{
    public class UserReviewQuestion
    {
        public uint Id { get; set; }
        public UserReviewQuestionType Type { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public uint SelectedRatingId { get; set; }
        public UserReviewQuestionDisplayType DisplayType { get; set; }
        public IEnumerable<UserReviewRating> Rating { get; set; }
        public ushort Order { get; set; }
    }

    public enum UserReviewQuestionDisplayType
    {
        Star = 1,
        Text = 2
    }

    public class UserReviewRating
    {
        public uint Id { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }
        public uint QuestionId { get; set; }
    }


}
