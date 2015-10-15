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
      private static int m_ModelId;
        public static int ModelId
      {
        get
        {
          return m_ModelId;
        }
        set
        {
          m_ModelId = value;
        }
      }
        public static int PageNo { get; set; }
        public static int PageSize { get; set; }
        public static int VersionId { get; set; }


        public static FilterBy Filter { get; set; }

        private static int m_RecordCount;
      public static int RecordCount
        {
          get
          {
            return m_RecordCount;
          }
          set
          {
            m_RecordCount = value;
          }
        }
        public static int FetchedRecordsCount { get; set; }

        static readonly string _bwHostUrl;
        static readonly string _ApiURL;
        static readonly string _requestType;

        static BindUserReviewControl()
        {
          _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
          _ApiURL = "/api/UserReviewsList?modelId={0}&startIndex={1}&endIndex={2}&versionId={3}&filter={4}&totalRecords={5}";
          _requestType = "application/json";
        }
        public static void BindUserReview(Repeater rptUserReviews)
        {
            List<ReviewEntity> userReviewList = null;
            FetchedRecordsCount = 0;

            try
            {
                int stratIndex=1;
                int endIndex=4;
                Paging.GetStartEndIndex(PageSize, PageNo, out stratIndex, out endIndex);

                string _apiUrl = String.Format(_ApiURL, m_ModelId, stratIndex, endIndex, 0, Filter, m_RecordCount);

                userReviewList = Bikewale.Common.BWHttpClient.GetApiResponseSync<List<ReviewEntity>>(_bwHostUrl, _requestType, _apiUrl, userReviewList);

                if (userReviewList != null && userReviewList.Count > 0)
                {
                  FetchedRecordsCount = userReviewList.Count;
                  rptUserReviews.DataSource = userReviewList;
                  rptUserReviews.DataBind();
                }
                else
                  FetchedRecordsCount = 0;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

        }
    }
}