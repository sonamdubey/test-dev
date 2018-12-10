using System.Collections.Generic;

namespace Carwale.Entity.UserReviews
{
    public class RatingQuestions
    {
        public int QuestionId { get; set; }
        public int Answer { get; set; }
    }
    public class UserDetails
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
    public class CarDetails
    {
        public int ModelId { get; set; }
        public int MakeId { get; set; }
        public int VersionId { get; set; }
    }
    public class Rating
    {
        public int UserRating { get; set; }
        public List<RatingQuestions> RatingQuestions { get; set; }
        public bool IsOwned { get; set; }
        public bool IsNewlyPurchased { get; set; }
        public int Familiarity { get; set; }
    }
    public class RatingDetails
    {
        public UserDetails UserDetails { get; set; }
        public CarDetails CarDetails { get; set; }
        public Rating Rating { get; set; }
        public int CustomerId { get; set; }
        public int PlatformId { get; set; }
        public int ReviewId { get; set; }
    }
}
