
using Bikewale.Common;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.UserReviews
{
    public class WriteReviewPageModel
    {
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId, _modelId, _makeId, _overAllRating, _priceRangeId;
        private string _decodedString;
        private string _encodedString, _userName, _emailId;
        private ulong _customerId;

        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public ushort Rating { get; set; }


        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="userReviews"></param>
        public WriteReviewPageModel(IUserReviews userReviews, string encodedString)
        {
            _userReviews = userReviews;
            _decodedString = Utils.Utils.DecryptTripleDES(encodedString);
            _encodedString = encodedString;
            ParseQueryString(_decodedString);
        }

        public void ParseQueryString(string decodedQueryString)
        {
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedQueryString);

            uint.TryParse(queryCollection["reviewid"], out _reviewId);
            uint.TryParse(queryCollection["modelid"], out _modelId);
            uint.TryParse(queryCollection["makeid"], out _makeId);
            uint.TryParse(queryCollection["overallrating"], out _overAllRating);
            ulong.TryParse(queryCollection["customerid"], out _customerId);
            uint.TryParse(queryCollection["priceRangeId"], out _priceRangeId);
            _userName = queryCollection["userName"];
            _emailId = queryCollection["emailId"];
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

                BikeModelEntity objModelEntity = null;

                if (_modelId > 0)
                    objModelEntity = new ModelHelper().GetModelDataById(_modelId);

                objPage.UserName = _userName;
                objPage.EmailId = _emailId;

                if (objModelEntity != null)
                {
                    objPage.Make = objModelEntity.MakeBase;
                    objPage.Model = new BikeModelEntityBase();
                    objPage.Model.ModelId = objModelEntity.ModelId;
                    objPage.Model.ModelName = objModelEntity.ModelName;
                    objPage.Model.MaskingName = objModelEntity.MaskingName;
                    objPage.HostUrl = objModelEntity.HostUrl;
                    objPage.OriginalImagePath = objModelEntity.OriginalImagePath;
                    objPage.PreviousPageUrl = string.Format("/m/user-reviews/rate-bike/{0}/?reviewId={1}", objPage.Model.ModelId, _encodedString);
                }

                objPage.ReviewId = _reviewId;
                objPage.CustomerId = _customerId;

                GetUserRatings(objPage);
                BindPageMetas(objPage);

            }
            catch (Exception ex)
            {

            }
            return objPage;
        }

        private void BindPageMetas(WriteReviewPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.Model != null)
                {
                    objPageVM.PageMetaTags.Title = string.Format("Write a Review | {0} {1}  - BikeWale", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                    objPageVM.PageMetaTags.Description = string.Format("Write a detailed review about {0} {1}. Tell us what do you think about {0} {1}. Share your experience of {0} {1} with others.", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ServiceCenterIndiaPage.BindPageMetas()");
            }
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
                        objUserVM.Rating = objUserReviewData.OverallRating.Where(x => x.Id == _overAllRating).First();
                    }

                    if (objUserReviewData.Questions != null)
                    {
                        UserReviewsInputEntity filter = new UserReviewsInputEntity()
                        {
                            Type = UserReviewQuestionType.Review,
                            PriceRangeId = (ushort)_priceRangeId
                        };
                        var objQuestions = _userReviews.GetUserReviewQuestions(filter, objUserReviewData);
                        if (objQuestions != null)
                        {
                            objQuestions.LastOrDefault().DisplayType = UserReviewQuestionDisplayType.Text;
                            objUserVM.JsonQuestionList = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

    }
}