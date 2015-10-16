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
        public int ModelId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int VersionId { get; set; }
        public FilterBy Filter { get; set; }
        public int RecordCount { get; set; }
        public int FetchedRecordsCount { get; set; }

        static readonly string _bwHostUrl;
        static readonly string _ApiURL;
        static readonly string _requestType;

        static BindUserReviewControl()
        {
            _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
            _ApiURL = "/api/UserReviewsList?modelId={0}&startIndex={1}&endIndex={2}&versionId={3}&filter={4}&totalRecords={5}";
            _requestType = "application/json";
        }

        public void BindUserReview(Repeater rptUserReviews)
        {
            IEnumerable<ReviewEntity> userReviewList = null;
            FetchedRecordsCount = 0;

            try
            {
                int stratIndex = 1;
                int endIndex = 4;
                Paging.GetStartEndIndex(PageSize, PageNo, out stratIndex, out endIndex);

                string _apiUrl = String.Format(_ApiURL, ModelId, stratIndex, endIndex, 0, Filter, RecordCount);

                userReviewList = Bikewale.Common.BWHttpClient.GetApiResponseSync<IEnumerable<ReviewEntity>>(_bwHostUrl, _requestType, _apiUrl, userReviewList);

                if (userReviewList != null)
                {
                    FetchedRecordsCount = userReviewList.Count();

                    if (FetchedRecordsCount > 0)
                    {
                        rptUserReviews.DataSource = userReviewList;
                        rptUserReviews.DataBind();
                    }
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