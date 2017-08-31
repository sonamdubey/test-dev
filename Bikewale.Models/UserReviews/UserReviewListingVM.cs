﻿
using Bikewale.Entities.UserReviews;
namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created By : Sushil Kumar on 7th May 2017
    /// Description : Viewmodel for user reviews listing page
    /// </summary>
    public class UserReviewListingVM : ModelBase
    {
        public uint ModelId { get; set; }
        public string BikeName { get; set; }
        public BikeRatingsReviewsInfo RatingReviewData { get; set; }
        public BikeRatingsInfo RatingsInfo { get { return (RatingReviewData != null ? RatingReviewData.RatingDetails : null); } }
        public BikeReviewsInfo ReviewsInfo { get { return (RatingReviewData != null ? RatingReviewData.ReviewDetails : null); } }
        public bool IsRatingsAvailable { get { return RatingsInfo != null; } }
        public bool IsReviewsAvailable { get { return ReviewsInfo != null && ReviewsInfo.TotalReviews > 0; } }
        public UserReviewsSearchVM UserReviews { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public UserReviewSimilarBikesWidgetVM SimilarBikesWidget { get; set; }
        public string PageUrl { get; set; }
    }
}
