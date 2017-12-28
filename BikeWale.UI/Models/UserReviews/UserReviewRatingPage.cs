
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
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
        private ushort _sourceId;
        private uint _selectedRating;
        private bool _isFake;
        public StatusCodes status;
        private string _returnUrl;
        private int _contestsrc;
        public ushort PlatFormId { get; set; }
        public bool IsMobile { get; internal set; }

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="userReviews"></param>       
        public UserReviewRatingPage(uint modelId, IUserReviews userReviews, IBikeMaskingCacheRepository<BikeModelEntity, int> objModel, string Querystring, IUserReviewsRepository userReviewsRepo)
        {
            _modelId = modelId;
            _userReviews = userReviews;
            _objModel = objModel;
            _Querystring = Querystring;
            _userReviewsRepo = userReviewsRepo;
            if (!string.IsNullOrEmpty(_Querystring))
                ProcessQuery(_Querystring);
            else
                status = Entities.StatusCodes.ContentFound;

        }


        private void ProcessQuery(string Querystring)
        {
            try
            {
                string _decodedString = Utils.Utils.DecryptTripleDES(Querystring);

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(_decodedString);
                uint.TryParse(queryCollection["reviewid"], out _reviewId);
                ulong.TryParse(queryCollection["customerid"], out _customerId);
                bool.TryParse(queryCollection["isFake"], out _isFake);
                _returnUrl = Convert.ToString(queryCollection["returnUrl"]);
                uint.TryParse(queryCollection["selectedRating"], out _selectedRating);
                ushort.TryParse(queryCollection["sourceid"], out _sourceId);
                int.TryParse(queryCollection["contestsrc"], out _contestsrc);
                if (_reviewId > 0 && !_isFake)
                {

                    if (_userReviewsRepo.IsUserVerified(_reviewId, _customerId))
                        status = Entities.StatusCodes.ContentFound;
                    else
                        status = Entities.StatusCodes.ContentNotFound;
                }
                else
                    status = Entities.StatusCodes.ContentFound;

            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("UserReviewRatingPage.ProcessQuery() - ModelId :{0}", _modelId));
            }


        }
        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Get data for the ratings page
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        /// <returns></returns>
        public UserReviewRatingVM GetData()
        {
            UserReviewRatingVM objUserVM = new UserReviewRatingVM();

            try
            {
                objUserVM.SelectedRating = _selectedRating;

                GetBikeData(objUserVM);

                GetUserRatings(objUserVM);
                if (objUserVM != null && objUserVM.objModelEntity != null)
                {
                    objUserVM.SourceId = _sourceId;
                    objUserVM.ReturnUrl = _returnUrl;
                    objUserVM.ContestSrc = _contestsrc;
                    BindMetas(objUserVM);
                    objUserVM.Page = Entities.Pages.GAPages.Rate_Bike;
                }

                objUserVM.UtmzCookieValue = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : String.Empty;
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("UserReviewRatingPage.GetData() - ModelId :{0}", _modelId));
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
                    objUserVM.PageMetaTags.Title = string.Format("Rate Your Bike | {0} {1} - BikeWale", objUserVM.objModelEntity.MakeBase.MakeName, objUserVM.objModelEntity.ModelName);
                    objUserVM.PageMetaTags.Description = string.Format("Rate {0} {1} on BikeWale. Tell us what do you think about {0} {1}. Share your experience of {0} {1} with others.", objUserVM.objModelEntity.MakeBase.MakeName, objUserVM.objModelEntity.ModelName);
                    objUserVM.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/rate-your-bike/{0}/", _modelId);
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("UserReviewRatingPage.BindMetas() - ModelId :{0}", _modelId));
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

                ErrorClass.LogError(ex, string.Format("UserReviewRatingPage.GetBikeData() - ModelId :{0}", _modelId));
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

                        }
                    }


                }
                else
                {
                    UserReviewSummary objUserReviewDataReview = _userReviews.GetUserReviewSummary(_reviewId);

                    if (objUserReviewData.OverallRating != null)
                    {
                        objUserVM.OverAllRatingText = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewData.OverallRating);
                    }
                    if (objUserReviewDataReview != null)
                    {
                        objUserVM.ReviewsOverAllrating = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewDataReview.OverallRatingId);

                        objUserVM.RatingQuestion = Newtonsoft.Json.JsonConvert.SerializeObject(objUserReviewDataReview.Questions);
                        objUserVM.CustomerEmail = objUserReviewDataReview.CustomerEmail;
                        objUserVM.CustomerName = objUserReviewDataReview.CustomerName;
                    }



                }

                var objLastPrice = objUserReviewData.PriceRange.Last();
                if (objUserVM.objModelEntity != null && objUserVM.objModelEntity.MinPrice >= objLastPrice.MaxPrice)
                {
                    objUserVM.PriceRangeId = objLastPrice.RangeId;
                }
                else
                {
                    objUserVM.PriceRangeId = objUserReviewData.PriceRange.First(x => x.MinPrice <= objUserVM.objModelEntity.MinPrice && x.MaxPrice >= objUserVM.objModelEntity.MinPrice).RangeId;
                }
                objUserVM.IsFake = _isFake;
                objUserVM.ReviewId = _reviewId;

            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("UserReviewRatingPage.GetUserRatings() - ModelId :{0}", _modelId));
            }
        }

    }
}