using System;
using System.Collections.Generic;
using Carwale.Entity.CMS;
using Carwale.Entity.UserReview;

namespace Carwale.BL.UserReview
{
    public static class WriteReviewObject
    {
        public static readonly string ResponseMessage = "Your rating and review has been recorded. The review will be moderated by our experts. We will inform you once the review is published. Thank you for your patience. If you do not receive any update within 4 days please contact us on {0}";

        private static Dictionary<int, string> _ratingResponseHeading = new Dictionary<int, string>
        {
            { 1, "Had a poor experience?" },
            { 2 , "Could things have been better?" },
            { 3 , "Could things have been better?" },
            { 4 , "Could things have been better?" },
            { 5 , "Had an amazing experience?" }
        };

        private static List<UserReviewRatingOptions> _ratingOptions = new List<UserReviewRatingOptions>
        {
                new UserReviewRatingOptions
                {
                    Id = 1,
                    Value = 1,
                    Heading = "Poor"
                },
                new UserReviewRatingOptions
                {
                    Id = 2,
                    Value = 2,
                    Heading = "Fair"
                },
                new UserReviewRatingOptions
                {
                    Id = 3,
                    Value = 3,
                    Heading = "Good"
                },
                new UserReviewRatingOptions
                {
                    Id = 4,
                    Value = 4,
                    Heading = "Very Good"
                },
                new UserReviewRatingOptions
                {
                    Id = 5,
                    Value = 5,
                    Heading = "Excellent"
                }
        };

        private static List<string> _parameters = new List<string>
        {
            "Buying experience",
            "Riding experience",
            "Details about looks, performance etc",
            "Servicing and maintenance",
            "Pros and Cons"
        };
        public static UserReviewPageDetails GetWriteReviewPageDetails(UserReviewDetail userReviewDetails)
        {

            UserReviewGuideLines review = new UserReviewGuideLines
            {
                Heading = _ratingResponseHeading[userReviewDetails.ReviewRate != null ? Convert.ToInt16(userReviewDetails.ReviewRate) : 2],
                Description = "Tell us more about it. You may consider writing about:",
                ReviewHintText = "The more detailed your review, the more useful it will be. Write your review.",
                UserResponse = userReviewDetails.Comments,
                Parameters = _parameters
            };
            UserReviewTitle title = new UserReviewTitle
            {
                Heading = "Give a title to your review",
                Description = "A catchy title will grab eyeballs, and more people will read your reviews.",
                UserResponse = userReviewDetails.Title
            };

            List<UserReviewQuestions> questions = new List<UserReviewQuestions>{
                new UserReviewQuestions{
                    Id = 1,
                    Heading = "Exterior/Styles",
                    Description = null,
                    UserResponse = userReviewDetails.StyleR.ToString()
                },
                new UserReviewQuestions{
                    Id = 2,
                    Heading = "Comfort & Space",
                    Description = null,
                    UserResponse = userReviewDetails.ComfortR.ToString()
                },
                new UserReviewQuestions{
                    Id = 3,
                    Heading = "Performance (Engine/Gear/Overall)",
                    Description = null,
                    UserResponse = userReviewDetails.PerformanceR.ToString()
                },
                new UserReviewQuestions{
                    Id = 4,
                    Heading = "Fuel Economy",
                    Description = null,
                    UserResponse = userReviewDetails.FuelEconomyR.ToString()
                },
                new UserReviewQuestions{
                    Id = 5,
                    Heading = "Value for Money/Features",
                    Description = null,
                    UserResponse = userReviewDetails.ValueR.ToString()
                }
            };

            UserReviewMoreRatings moreRatings = new UserReviewMoreRatings
            {
                Heading = "We’d love your opinion on few more things about the car.",
                Questions = questions,
                RatingOptions = _ratingOptions
            };
            UserReviewUserDetails userDetails = new UserReviewUserDetails
            {
                Name = userReviewDetails.CustomerName,
                Email = userReviewDetails.CustomerEmail
            };
            NameIdBase make = new NameIdBase
            {
                Name = userReviewDetails.Make,
                Id = userReviewDetails.MakeId,
                MaskingName = userReviewDetails.MakeMaskingName
            };
            NameIdBase model = new NameIdBase
            {
                Name = userReviewDetails.Model,
                Id = String.IsNullOrEmpty(userReviewDetails.ModelId) ? 0 : Convert.ToInt16(userReviewDetails.ModelId),
                MaskingName = userReviewDetails.MaskingName
            };
            NameIdBase version = new NameIdBase
            {
                Name = userReviewDetails.Version,
                Id = String.IsNullOrEmpty(userReviewDetails.VersionId) ? 0 : Convert.ToInt16(userReviewDetails.VersionId),
                MaskingName = userReviewDetails.VersionMaskingName
            };
            UserReviewCarDetails carDetails = new UserReviewCarDetails
            {
                Make = make,
                Model = model,
                Version = version,
                HostUrl = userReviewDetails.HostUrl,
                OriginalImgPath = userReviewDetails.OriginalImgPath
            };
            UserReviewPageDetails reviewDetails = new UserReviewPageDetails
            {
                CarDetails = carDetails,
                Review = review,
                Title = title,
                MoreRatings = moreRatings,
                UserRating = userReviewDetails.ReviewRate,
                UserDetails = userDetails
            };
            return reviewDetails;
        }
    }
}
