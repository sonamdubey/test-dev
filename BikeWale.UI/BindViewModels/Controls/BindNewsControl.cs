using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Bikewale.Utility;
using Microsoft.Practices.Unity;

namespace Bikewale.BindViewModels.Controls
{
    public class BindNewsControl
    {        
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        string cacheKey = "BW_CMS_";


        /// <summary>
        /// Written By : Ashish G. Kamble on 28 Feb 2016
        /// Summary : Function to get the data from the carwale apis. This data is cached on the bikewale.
        /// </summary>
        /// <param name="rptr"></param>
        public void BindNews(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            try
            {
                IEnumerable<ArticleSummary> _objArticleList = null;

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.News);
                categorList.Add(EnumCMSContentType.AutoExpo2016);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                cacheKey += contentTypeList.Replace(",", "_") + "_Cnt_" + TotalRecords;

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                    {                        
                        cacheKey += "_Make_" + MakeId + "_Model_" + ModelId;
                    }
                    else
                    {
                        cacheKey += "_Make_" + MakeId;
                    }
                }

                using(IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    ICacheManager _cache = container.Resolve<ICacheManager>();

                    _objArticleList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 15, 0), () => GetNewsFromCWAPI(contentTypeList));
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


        /// <summary>
        /// Written By : Ashish G. Kamble on 28 Feb 2016
        /// Summary : Function to get the data from the carwale cms api.
        /// </summary>
        /// <param name="contentTypeList">comma separated content ids.</param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetNewsFromCWAPI(string contentTypeList)
        {
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {                
                
                string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + contentTypeList + "&totalrecords=" + TotalRecords;

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                    {
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + contentTypeList + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId + "&modelid=" + ModelId;                        
                    }
                    else
                    {
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + contentTypeList + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId;
                    }
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {                   
                    _objArticleList = objClient.GetApiResponseSync<IEnumerable<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, _objArticleList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;

        }   // end of GetNewsFromCWAPI

    }
}