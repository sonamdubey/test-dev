
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
        public UserReviewRatingPage(uint modelId, IBikeInfo bikeInfo, IUserReviews userReviews)
        {
            _modelId = modelId;
            _bikeInfo = bikeInfo;
            _userReviews = userReviews;
        }
        public UserReviewRatingVM GetData()
        {
            UserReviewRatingVM objUserVM = new UserReviewRatingVM();

            GetBikeData(objUserVM);
            GetUserRatings(objUserVM);

            return objUserVM;
        }

        private void GetBikeData(UserReviewRatingVM objUserVM)
        {
            objUserVM.BikeInfo = _bikeInfo.GetBikeInfo(_modelId, 0);
        }

        private void GetUserRatings(UserReviewRatingVM objUserVM)
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

    }
}