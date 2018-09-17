using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.UserReviews;
using Bikewale.Interfaces.UserReviews;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Bikewale.Utility;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// created by Sajal Gupta on 31-08-2017
    /// description : Class to power other details page
    /// </summary>
    public class UserReviewOtherDetailsPage
    {
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId, _modelId, _makeId, _overAllRating, _priceRangeId, _pageSourceID;
        private string _encodedString, _userName, _emailId;
        private ulong _customerId;
        private int _contestSrc;

        public StatusCodes Status;

        public UserReviewOtherDetailsPage(IUserReviews userReviews, string encodedString)
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

        private void ParseQueryString(string encodedQueryString)
        {
            try
            {
                string decodedQueryString = TripleDES.DecryptTripleDES(encodedQueryString);

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
                ErrorClass.LogError(ex, "UserReviewOtherDetailsPage.ParseQueryString()");
            }
        }

        private void BindPageMetas(UserReviewsOtherDetailsPageVM objPageVM)
        {
            try
            {
                if (objPageVM != null && objPageVM.PageMetaTags != null && objPageVM.Make != null && objPageVM.Model != null)
                {
                    objPageVM.PageMetaTags.Title = string.Format("Share more details | {0} {1}  - BikeWale", objPageVM.Make.MakeName, objPageVM.Model.ModelName);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "WriteReviewPageModel.BindPageMetas()");
            }
        }

        public UserReviewsOtherDetailsPageVM GetData()
        {
            UserReviewsOtherDetailsPageVM objPage = null;
            try
            {
                objPage = new UserReviewsOtherDetailsPageVM();

                objPage.UserName = _userName;
                objPage.EmailId = _emailId;

                UserReviewSummary objReviewSummary = _userReviews.GetUserReviewSummary(_reviewId);

                if (objReviewSummary != null)
                {
                    objPage.Make = objReviewSummary.Make;
                    objPage.Model = objReviewSummary.Model;
                    objPage.PreviousPageUrl = string.Format("/write-a-review/?q={1}", objPage.Model.ModelId, _encodedString);
                    objPage.ReturnUrl = objReviewSummary.ReturnUrl;
                }

                objPage.ReviewId = _reviewId;
                objPage.CustomerId = _customerId;
                objPage.PageSourceId = _pageSourceID;
                objPage.ContestSrc = _contestSrc;
                objPage.EncodedWriteUrl = _encodedString;

                BindPageMetas(objPage);

                UserReviewsData objUserReviewData = _userReviews.GetUserReviewsData();

                if (objUserReviewData != null)
                {
                    if (objUserReviewData.OverallRating != null)
                    {
                        objPage.Rating = objUserReviewData.OverallRating.Where(x => x.Id == _overAllRating).First();
                    }

                    if (objUserReviewData.Questions != null)
                    {
                        UserReviewsInputEntity filter = new UserReviewsInputEntity()
                        {
                            Type = UserReviewQuestionType.Review,
                            PriceRangeId = (ushort)_priceRangeId
                        };

                        objPage.QuestionsList = _userReviews.GetUserReviewQuestions(filter, objUserReviewData);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "UserReviewOtherDetailsPage.GetData()");
            }
            return objPage;
        }

    }
}