using Bikewale.Interfaces.UserReviews;
using Bikewale.Notifications;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Bikewale.Models.UserReviews
{
    /// <summary>
    /// Created by : Aditi Srivastava on 18 Apr 2017
    /// Summary    : Model for user review summary
    /// </summary>
    public class UserReviewSummaryPage
    {
        #region Variables for dependency injection
        private readonly IUserReviews _userReviews = null;
        private uint _reviewId;
        private string _strEncoded;
        #endregion

        #region Constructor
        public UserReviewSummaryPage(IUserReviews userReviews, uint reviewId,string q)
        {
            _userReviews = userReviews;
            _reviewId = reviewId;
            _strEncoded = q;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 18 Apr 2017
        /// Summary    : Get data for user review summary
        /// </summary>
        public UserReviewSummaryVM GetData()
        {
            UserReviewSummaryVM objData = new UserReviewSummaryVM();
            try
            {
                objData.Summary = _userReviews.GetUserReviewSummary(_reviewId);
                objData.WriteReviewLink = string.Format("/write-a-review/?q={0}", _strEncoded);
                if (objData.Summary != null)
                {
                    objData.PrevPageUrl = Bikewale.Utility.UserReviews.FormatPreviousPageUrl(objData.Summary.PageSource, objData.Summary.Make.MaskingName, objData.Summary.Model.MaskingName);
                }
               
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "UserReviewSummaryPage.GetData");
            }
            return objData;
        }
        #endregion

    }
}