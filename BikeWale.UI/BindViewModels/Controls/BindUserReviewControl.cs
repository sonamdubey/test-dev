using Bikewale.Common;
using Bikewale.Entities.DTO;
using Bikewale.Entities.UserReviews;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Author:rakesh yadav On 08 Sep 2015
    /// Desc: Call UserReviewsList service and bind review list to repeater
    /// </summary>
    public class BindUserReviewControl
    {
        public static int ModelId { get; set; }
        public static int PageNo { get; set; }
        public static int PageSize { get; set; }
        public static int VersionId { get; set; }
        public static FilterBy Filter { get; set; }
        public static int RecordCount { get; set; }

        public static void BindUserReview(Repeater rptUserReviews)
        {
            List<ReviewEntity> userReviewList = null;
            try
            {
                int stratIndex=1;
                int endIndex=4;
                Paging.GetStartEndIndex(PageSize, PageNo, out stratIndex, out endIndex);

                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
                string _requestType = "application/json";
                string _apiUrl = String.Format("/api/UserReviewsList?modelId={0}&startIndex={1}&endIndex={2}&versionId={3}&filter={4}&totalRecords={5}",
                    ModelId, stratIndex, endIndex, 0, Filter, RecordCount);

                userReviewList = Bikewale.Common.BWHttpClient.GetApiResponseSync<List<ReviewEntity>>(_bwHostUrl, _requestType, _apiUrl, userReviewList);

                if (userReviewList.Count > 0)
                {
                    rptUserReviews.DataSource = userReviewList;
                    rptUserReviews.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
    }
}