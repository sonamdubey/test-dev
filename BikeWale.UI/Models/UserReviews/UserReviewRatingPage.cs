
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
namespace Bikewale.Models
{
    public class UserReviewRatingPage
    {
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IUserReviews _userReviews = null;
        private uint _modelId;

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>
        public UserReviewRatingPage(uint modelId, IBikeInfo bikeInfo, IUserReviews userReviews)
        {
            _modelId = modelId;
            _bikeInfo = bikeInfo;
            _userReviews = userReviews;
        }
        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get data for the ratings page
        /// </summary>
        /// <returns></returns>
        public UserReviewRatingVM GetData()
        {
            UserReviewRatingVM objUserVM = new UserReviewRatingVM();

            GetBikeData(objUserVM);
            GetUserRatings(objUserVM);

            return objUserVM;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : GetBikeInforamtion for ratings page
        /// </summary>
        /// <param name="objUserVM"></param>
        private void GetBikeData(UserReviewRatingVM objUserVM)
        {
            objUserVM.BikeInfo = _bikeInfo.GetBikeInfo(_modelId, 0);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Function to get user ratings and overall ratings for ratings page
        /// </summary>
        /// <param name="objUserVM"></param>
        private void GetUserRatings(UserReviewRatingVM objUserVM)
        {
            try
            {
                UserReviewsData objUserReviewData = _userReviews.GetUserReviewsData();
                if (objUserReviewData != null)
                {
                    if (objUserReviewData.OverallRating != null)
                    {
                        objUserVM.OverAllRatingText = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewData.OverallRating);
                    }

                    if (objUserReviewData.Questions != null)
                    {
                        UserReviewsInputEntity filter = new UserReviewsInputEntity()
                        {
                            Type = UserReviewQuestionType.Rating
                        };
                        var objQuestions = _userReviews.GetUserReviewQuestions(filter, objUserReviewData);
                        if (objQuestions != null)
                        {
                            objUserVM.RatingQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                        }
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

    }
}