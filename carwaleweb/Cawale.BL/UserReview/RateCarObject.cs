using Carwale.Entity.UserReview;
using Carwale.Utility;
using System.Collections.Generic;

namespace Carwale.BL.UserReview
{
    public static class RateCarObject
    {
        public static readonly string ResponseMessage = "It seems that you have already written a review for {0} {1} {2}.";
        private static List<UserReviewRatingOptions> _reviewRatingOptions = new List<UserReviewRatingOptions>
        {
            new UserReviewRatingOptions
            {
                Id = 1,
                Value = 1,
                Description = "I regret riding this car.",
                Heading = "Terrible!",
                OriginalImgPath = "/cw/static/icons/m/thumbs-down-red.png"
            },
            new UserReviewRatingOptions
            {
                Id = 2,
                Value = 2,
                Description = "I know better car in the same price range.",
                Heading = "It's bad!",
                OriginalImgPath = "/cw/static/icons/m/thumbs-down-red.png"
            },
            new UserReviewRatingOptions
            {
                Id = 3,
                Value = 3,
                Description = "Could have been much better.",
                Heading = "It's okay!",
                OriginalImgPath = "/cw/static/icons/m/thumbs-up-teal.png"
            },
            new UserReviewRatingOptions
            {
                Id = 4,
                Value = 4,
                Description = "I love the car.",
                Heading = "Superb!",
                OriginalImgPath =  "/cw/static/icons/m/thumbs-up-teal.png"
            },
            new UserReviewRatingOptions
            {
                Id = 5,
                Value =  5,
                Description =  "The car exceeds all the expectations.",
                Heading = "Amazing!",
                OriginalImgPath =  "/cw/static/icons/m/thumbs-up-teal.png"
            }
        };

        private static List<UserReviewOption> _quesOneOptionList = new List<UserReviewOption>
        {
            new UserReviewOption { Id = 1, Value = "New" },
            new UserReviewOption { Id = 2, Value = "Used"},
            new UserReviewOption { Id = 3, Value = "Not Purchased"}
        };
        private static List<UserReviewOption> _quesTwoOptionList = new List<UserReviewOption>
        {
            new UserReviewOption { Id = 4, Value = "Few thousand kilometers" },
            new UserReviewOption { Id = 3, Value = "Few hundred kilometers"},
            new UserReviewOption { Id = 5, Value = "Its my mate since ages"},
            new UserReviewOption { Id = 2, Value = "Did a short drive once"},
            new UserReviewOption { Id = 1, Value = "Haven't driven it"}
        };

        private static List<UserReviewQuestions> _userRatingQuestions = new List<UserReviewQuestions>
           {
                new UserReviewQuestions
                {
                    Id = 1,
                    Heading = "You purchased this car as?",
                    Description = null,
                    UserReviewOption = _quesOneOptionList
                },
                new UserReviewQuestions
                {
                    Id = 2,
                    Heading = "How long have you driven this car?",
                    Description = null,
                    UserReviewOption = _quesTwoOptionList
                }
        };

        private static RateCarDetails _ratingDetails = new RateCarDetails
        {
            Description = "Consider looks, styling performance, servicing, experience,mileage etc. before rating",
            Heading = "How was your experience with this car?",
            HostUrl = CWConfiguration._imgHostUrl,
            UserRatingQuestions = _userRatingQuestions,
            UserReviewRatingOptions = _reviewRatingOptions
        };

        public static RateCarDetails GetRateCarDetails()
        {
            return _ratingDetails;
        }
    }
}
