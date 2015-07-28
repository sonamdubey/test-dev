using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using Microsoft.Practices.Unity;
using Bikewale.DAL.UserReviews;


namespace Bikewale.BAL.UserReviews
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 12 June 2014
    /// Summary : Class have business logic for the user reviews
    /// </summary>
    public class UserReviews : IUserReviews
    {
        private readonly IUserReviews userReviewsRepository = null;

        public UserReviews()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IUserReviews, UserReviewsRepository>();
                userReviewsRepository = container.Resolve<IUserReviews>();
            }
        }

        public List<ReviewTaggedBikeEntity> GetMostReviewedBikesList(ushort totalRecords)
        {
            List<ReviewTaggedBikeEntity> objTaggedBikes = null;

            objTaggedBikes = userReviewsRepository.GetMostReviewedBikesList(totalRecords);

            return objTaggedBikes;
        }

        public List<ReviewTaggedBikeEntity> GetReviewedBikesList()
        {
            List<ReviewTaggedBikeEntity> objTaggedBikes = null;

            objTaggedBikes = userReviewsRepository.GetReviewedBikesList();

            return objTaggedBikes;
        }

        public List<ReviewsListEntity> GetMostReadReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostReadReviews(totalRecords);

            return objReviews;
        }

        public List<ReviewsListEntity> GetMostHelpfulReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostHelpfulReviews(totalRecords);

            return objReviews;
        }

        public List<ReviewsListEntity> GetMostRecentReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostRecentReviews(totalRecords);

            return objReviews;
        }

        public List<ReviewsListEntity> GetMostRatedReviews(ushort totalRecords)
        {
            List<ReviewsListEntity> objReviews = null;

            objReviews = userReviewsRepository.GetMostRatedReviews(totalRecords);

            return objReviews;
        }

        public ReviewRatingEntity GetBikeRatings(uint modelId)
        {
            ReviewRatingEntity objRating = null;

            objRating = userReviewsRepository.GetBikeRatings(modelId);

            return objRating;
        }

        public List<ReviewEntity> GetBikeReviewsList(uint startIndex, uint endIndex, uint modelId, uint versionId, FilterBy filter, out uint totalReviews)
        {
            List<ReviewEntity> objReviewList = null;

            objReviewList = userReviewsRepository.GetBikeReviewsList(startIndex, endIndex, modelId, versionId, filter, out totalReviews);

            return objReviewList;
        }

        public ReviewDetailsEntity GetReviewDetails(uint reviewId)
        {
            ReviewDetailsEntity objReview = null;

            objReview = userReviewsRepository.GetReviewDetails(reviewId);
            
            return objReview;
        }
        
        public bool UpdateViews(uint reviewId)
        {
            bool isViesUpdated = false;

            isViesUpdated = userReviewsRepository.UpdateViews(reviewId);

            return isViesUpdated;
        }

        public bool AbuseReview(uint reviewId, string comment, string userId)
        {
            bool isReviewAbused = false;

            isReviewAbused = userReviewsRepository.AbuseReview(reviewId, comment, userId);

            return isReviewAbused;
        }

        public bool UpdateReviewUseful(uint reviewId, bool isHelpful)
        {
            bool isUpdated = false;

            isUpdated = userReviewsRepository.UpdateReviewUseful(reviewId, isHelpful);

            return isUpdated;
        }
    }   // Class
}   // Namespace
