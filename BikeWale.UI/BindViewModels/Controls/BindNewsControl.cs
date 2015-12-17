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
    public class BindNewsControl
    {        
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        public void BindNews(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            try
            {
                IEnumerable<ArticleSummary> _objArticleList = null;
                                
                int _contentType = (int)EnumCMSContentType.News;
                string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + TotalRecords;


                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId + "&modelid=" + ModelId;
                    else
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + _contentType + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId;
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //_objArticleList = objClient.GetApiResponseSync<IEnumerable<ArticleSummary>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objArticleList);
                    _objArticleList = objClient.GetApiResponseSync<IEnumerable<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objArticleList);
                }

                if (_objArticleList != null && _objArticleList.Count() > 0)
                {
                    FetchedRecordsCount = _objArticleList.Count();

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