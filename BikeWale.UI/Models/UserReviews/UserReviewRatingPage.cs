﻿
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using System.Linq;
namespace Bikewale.Models
{
    public class UserReviewRatingPage
    {

        private readonly IUserReviews _userReviews = null;
        private IBikeModels<BikeModelEntity, int> _objModel = null;

        private uint _modelId;
        private uint _reviewId;

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>
        public UserReviewRatingPage(uint modelId, IUserReviews userReviews, IBikeModels<BikeModelEntity, int> objModel, uint? reviewId)
        {
            _modelId = modelId;

            _userReviews = userReviews;
            _objModel = objModel;
            _reviewId = reviewId ?? 0;
            ProcessQuery();

        }
        private void ProcessQuery()
        {


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

            BindMetas(objUserVM);

            return objUserVM;
        }

        private void BindMetas(UserReviewRatingVM objUserVM)
        {

            try
            {
                objUserVM.PageMetaTags.Title = "Rate Your Bike| Write a Review - BikeWale";
                objUserVM.PageMetaTags.Description = string.Format("Rate your {0} {1} on BikeWale. Write a detailed review about {0} {1} and help others in making a right buying decision.", objUserVM.objModelEntity.MakeBase.MakeName, objUserVM.objModelEntity.ModelName);

            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        /// <summary>

        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : GetBikeInforamtion for ratings page
        /// </summary>
        /// <param name="objUserVM"></param>
        private void GetBikeData(UserReviewRatingVM objUserVM)
        {
            try
            {
                objUserVM.objModelEntity = _objModel.GetById((int)_modelId);
            }
            catch (System.Exception)
            {

                throw;
            }
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

                if (_reviewId == 0)
                {
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
                                objQuestions.FirstOrDefault(x => x.Id == 2).SubQuestionId = 3;
                                objQuestions.FirstOrDefault(x => x.Id == 3).Visibility = false;
                                objQuestions.FirstOrDefault(x => x.Id == 3).IsRequired = false;
                                objUserVM.RatingQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                            }

                            objUserVM.PriceRangeId = objUserReviewData.PriceRange.First(x => x.MinPrice >= objUserVM.objModelEntity.MinPrice && x.MaxPrice >= objUserVM.objModelEntity.MaxPrice).RangeId;
                        }
                    }


                }
                else
                {
                    UserReviewSummary objUserReviewDataReview = _userReviews.GetUserReviewSummary(_reviewId);

                    objUserVM.OverAllRatingText = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewData.OverallRating);


                    if (objUserReviewDataReview != null)
                    {
                        objUserVM.ReviewsOverAllrating = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewDataReview.OverallRatingId);
                        objUserVM.RatingQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewDataReview.Questions);
                        objUserVM.CustomerEmail = objUserReviewDataReview.CustomerEmail;
                        objUserVM.CustomerName = objUserReviewDataReview.CustomerName;
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