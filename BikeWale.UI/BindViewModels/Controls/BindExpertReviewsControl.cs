using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Notifications;
using Bikewale.Utility;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// Summary : Class have functions to bind the expert reviews.
    /// </summary>
    public static class BindExpertReviewsControl
    {
        public static int TotalRecords { get; set; }
        public static int? MakeId { get; set; }
        public static int? ModelId { get; set; }
        public static int FetchedRecordsCount { get; set; }

        /// <summary>
        /// Summary : Function to bind the expert reviews control.
        /// </summary>
        public static void BindExpertReviews(Repeater rptr)
        {
            try
            {
                List<ArticleSummary> _objArticleList = null;

                string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
                string _requestType = "application/json";
                string _contentType = (int)EnumCMSContentType.RoadTest + "," + (int)EnumCMSContentType.ComparisonTests;
                string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + TotalRecords;


                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId + "&modelid=" + ModelId;
                    else
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId;
                }

                _objArticleList = BWHttpClient.GetApiResponseSync<List<ArticleSummary>>(_cwHostUrl, _requestType, _apiUrl, _objArticleList);

                if (_objArticleList != null && _objArticleList.Count > 0)
                {
                    FetchedRecordsCount = _objArticleList.Count;

                    rptr.DataSource = _objArticleList;
                    rptr.DataBind();
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