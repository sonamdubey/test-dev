﻿
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by Sajal Gupta on 10-04-2017
    /// Descrioption : This Model will run write review page.
    /// </summary>
    public class WriteReviewPageModel
    {
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId, _modelId, _makeId, _overAllRating, _priceRangeId, _pageSourceID;
        private string _encodedString, _userName, _emailId;
        private ulong _customerId;
        private int _contestSrc;

        public WriteReviewPageSubmitResponse SubmitResponse { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public BikeModelEntityBase Model { get; set; }
        public ushort Rating { get; set; }
        public bool IsDesktop { get; set; }

        public StatusCodes Status;

        /// <summary>
        /// Created By : Sushil Kumar on 17th April 2017
        /// Description : Added interfaces for bikeinfo and user reviews 
        /// </summary>
        /// <param name="userReviews"></param>
        public WriteReviewPageModel(IUserReviews userReviews, string encodedString)
        {
            _userReviews = userReviews;
            _encodedString = encodedString;

            if (!string.IsNullOrEmpty(encodedString))
            {
                ParseQueryString(encodedString);
                Status = Entities.StatusCodes.ContentFound;
            }
            else
                Status = Entities.StatusCodes.ContentNotFound;
        }

        public void ParseQueryString(string encodedQueryString)
        {
            try
            {
                string decodedQueryString = Utils.Utils.DecryptTripleDES(encodedQueryString);

                NameValueCollection queryCollection = HttpUtility.ParseQueryString(decodedQueryString);

                uint.TryParse(queryCollection["reviewid"], out _reviewId);
                uint.TryParse(queryCollection["modelid"], out _modelId);
                uint.TryParse(queryCollection["makeid"], out _makeId);
                uint.TryParse(queryCollection["overallrating"], out _overAllRating);
                ulong.TryParse(queryCollection["customerid"], out _customerId);
                uint.TryParse(queryCollection["priceRangeId"], out _priceRangeId);
                uint.TryParse(queryCollection["sourceid"], out _pageSourceID);
                int.TryParse(queryCollection["contestsrc"], out _contestSrc);
                _userName = queryCollection["userName"];
                _emailId = queryCollection["emailId"];
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "WriteReviewPageModel.ParseQueryString()");
            }
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

                objPage.UserName = _userName;
                objPage.EmailId = _emailId;

                UserReviewSummary objReviewSummary = _userReviews.GetUserReviewSummary(_reviewId);

                if (objReviewSummary != null)
                {
                    objPage.Make = objReviewSummary.Make;
                    objPage.Model = objReviewSummary.Model;
                    objPage.HostUrl = objReviewSummary.HostUrl;
                    objPage.OriginalImagePath = objReviewSummary.OriginalImagePath;

                    if (IsDesktop)
                        objPage.PreviousPageUrl = string.Format("/user-reviews/rate-bike/{0}/?q={1}", objPage.Model.ModelId, _encodedString);
                    else
                        objPage.PreviousPageUrl = string.Format("/m/user-reviews/rate-bike/{0}/?q={1}", objPage.Model.ModelId, _encodedString);

                    objPage.JsonReviewSummary = Newtonsoft.Json.JsonConvert.SerializeObject(objReviewSummary);
                }

                objPage.ReviewId = _reviewId;
                objPage.CustomerId = _customerId;
                objPage.PageSourceId = _pageSourceID;
                objPage.ContestSrc = _contestSrc;
                objPage.EncodedWriteUrl = _encodedString;

                GetUserRatings(objPage);
                BindPageMetas(objPage);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "WriteReviewPageModel.GetData()");
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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "WriteReviewPageModel.BindPageMetas()");
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
                            objUserVM.JsonQuestionList = Newtonsoft.Json.JsonConvert.SerializeObject(objQuestions);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "WriteReviewPageModel.GetUserRatings()");
            }
        }

    }
}