
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
namespace Bikewale.Models
{
    public class UserReviewRatingPage
    {

        private readonly IUserReviews _userReviews = null;
        private IBikeMaskingCacheRepository<BikeModelEntity, int> _objModel = null;
        private readonly IUserReviewsRepository _userReviewsRepo = null;

        private uint _modelId;
        private uint _reviewId;
        private string _Querystring;
        private ulong _customerId;
        private bool _isVerified;
        private uint _pagesourceId;
        public StatusCodes status;
        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>
        public UserReviewRatingPage(uint modelId, uint? pagesourceId, IUserReviews userReviews, IBikeMaskingCacheRepository<BikeModelEntity, int> objModel, string Querystring, IUserReviewsRepository userReviewsRepo)
        {
            _modelId = modelId;
            _userReviews = userReviews;
            _objModel = objModel;
            _Querystring = Querystring;
            _pagesourceId = pagesourceId ?? 0;
            _userReviewsRepo = userReviewsRepo;
            if (!string.IsNullOrEmpty(_Querystring))
                ProcessQuery(_Querystring);
            else
                status = Entities.StatusCodes.ContentFound;

        }
        private void ProcessQuery(string Querystring)
        {
            string _decodedString = Utils.Utils.DecryptTripleDES(Querystring);
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(_decodedString);
            uint.TryParse(queryCollection["reviewid"], out _reviewId);
            ulong.TryParse(queryCollection["customerid"], out _customerId);
            uint.TryParse(queryCollection["pagesourceid"], out _pagesourceId);
            _isVerified = _userReviewsRepo.IsUserVerified(_reviewId, _customerId);
            if (_isVerified)
                status = Entities.StatusCodes.ContentFound;
            else
                status = Entities.StatusCodes.ContentNotFound;


        }
        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get data for the ratings page
        /// </summary>
        /// <returns></returns>
        public UserReviewRatingVM GetData()
        {
            UserReviewRatingVM objUserVM = new UserReviewRatingVM();

            try
            {
                GetBikeData(objUserVM);

                GetUserRatings(objUserVM);

                if (objUserVM != null && objUserVM.objModelEntity != null)
                {
                    BindMetas(objUserVM);
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewRatingPage.GetData() - ModelId :{0}", _modelId));
            }

            return objUserVM;
        }
        /// <summary>
        /// Created By :- Subodh Jain 19 April 2017
        /// Summary :- Bind Metas for the page
        /// </summary>
        /// <param name="objUserVM"></param>
        private void BindMetas(UserReviewRatingVM objUserVM)
        {

            try
            {
                if (objUserVM != null && objUserVM.PageMetaTags != null)
                {
                    objUserVM.PageMetaTags.Title = "Rate Your Bike | Write a Review - BikeWale";

                    objUserVM.PageMetaTags.Description = string.Format("Rate your {0} {1} on BikeWale. Write a detailed review about {0} {1} and help others in making a right buying decision.", objUserVM.objModelEntity.MakeBase.MakeName, objUserVM.objModelEntity.ModelName);

                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewRatingPage.BindMetas() - ModelId :{0}", _modelId));
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
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewRatingPage.GetBikeData() - ModelId :{0}", _modelId));
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
                objUserVM.ReviewId = _reviewId;
                objUserVM.pagesourceId = _pagesourceId;

            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("UserReviewRatingPage.GetUserRatings() - ModelId :{0}", _modelId));
            }
        }

    }
}