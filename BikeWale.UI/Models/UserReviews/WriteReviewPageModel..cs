
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using System;

namespace Bikewale.Models.UserReviews
{
    public class WriteReviewPageModel
    {
        private readonly IUserReviews _userReviews = null;
        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="userReviews"></param>
        public WriteReviewPageModel(IUserReviews userReviews)
        {
            _userReviews = userReviews;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get data for the write reviews page
        /// </summary>
        /// <returns></returns>
        public WriteReviewPageVM GetData()
        {
            WriteReviewPageVM objPage = null;
            try
            {
                objPage = new WriteReviewPageVM();

                GetUserRatings(objPage);

            }
            catch (Exception ex)
            {

            }
            return objPage;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Function to get user ratings and overall ratings for write review page
        /// </summary>
        /// <param name="objUserVM"></param>
        private void GetUserRatings(WriteReviewPageVM objUserVM)
        {
            try
            {
                UserReviewsData objUserReviewData = _userReviews.GetUserReviewsData();
                if (objUserReviewData != null)
                {
                    if (objUserReviewData.OverallRating != null)
                    {
                        //objUserVM.Rating = objUserReviewData.OverallRating;
                    }

                    if (objUserReviewData.Questions != null)
                    {
                        UserReviewsInputEntity filter = new UserReviewsInputEntity()
                        {
                            Type = UserReviewQuestionType.Review,
                            PriceRangeId = 3
                        };
                        var objQuestions = _userReviews.GetUserReviewQuestions(filter, objUserReviewData);
                        if (objQuestions != null)
                        {
                            objUserVM.JsonQuestionList = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}