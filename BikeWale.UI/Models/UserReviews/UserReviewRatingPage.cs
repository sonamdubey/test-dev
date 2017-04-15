
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using System.Collections.Generic;
using System.Linq;
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
            objUserVM.BikeInfo = _bikeInfo.GetBikeInfo(_modelId, 0);
            IEnumerable<UserReviewOverallRating> obj = null;

            var objUserReviews = _userReviews.GetUserReviewsData();
            obj = objUserReviews.OverallRating;

            objUserVM.OverAllRatingText = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            foreach (var question in objUserReviews.Questions)
            {
                question.Rating = objUserReviews.Ratings.Where(x => x.QuestionId == question.Id && question.Type == UserReviewQuestionType.Rating);
            }

            objUserVM.RatingQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviews.Questions);
            return objUserVM;
        }

    }
}