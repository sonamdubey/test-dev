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
using Grpc.CMS;
using Bikewale.News.GrpcFiles;

namespace Bikewale.BindViewModels.Controls
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 1 Sept 2015
    /// Summary : Class have functions to bind the expert reviews.
    /// </summary>
    public class BindExpertReviewsControl
    {
        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        string cacheKey = "BW_CMS_";

        static bool _useGrpc = Convert.ToBoolean(ConfigurationManager.AppSettings["UseGrpc"]);

        /// <summary>
        /// Summary : Function to bind the expert reviews control. Function will cache the data from CW api on bikewale
        /// </summary>
        public void BindExpertReviews(Repeater rptr)
        {
            FetchedRecordsCount = 0;

            try
            {
                IEnumerable<ArticleSummary> _objArticleList = null;
                
                string _contentType = (int)EnumCMSContentType.RoadTest + "," + (int)EnumCMSContentType.ComparisonTests;

                cacheKey += _contentType.Replace(",", "_") + "_Cnt_" + TotalRecords;

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        cacheKey += "_Make_" + MakeId + "_Model_" + ModelId;
                    else
                        cacheKey += "_Make_" + MakeId;
                }

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    ICacheManager _cache = container.Resolve<ICacheManager>();

                    _objArticleList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 15, 0), () => GetReviewsFromCW(_contentType));
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

        private IEnumerable<ArticleSummary> GetReviewsFromCW(string contentTypeList)
        {
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(contentTypeList, TotalRecords, MakeId, ModelId);

                    if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.Summary.Count > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                    }
                    else
                    {
                        return GetReviewsFromCWAPIOldWay(contentTypeList);
                    }

                }
                else
                {
                    return GetReviewsFromCWAPIOldWay(contentTypeList);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return GetReviewsFromCWAPIOldWay(contentTypeList);
            }
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 28 Feb 2016
        /// Summary : Function to get the data from the carwale cms api.
        /// </summary>
        /// <param name="contentTypeList">comma separated content ids.</param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetReviewsFromCWAPIOldWay(string contentTypeList)
        {
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {
                string _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + contentTypeList + "&totalrecords=" + TotalRecords;


                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + contentTypeList + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId + "&modelid=" + ModelId;
                    else
                        _apiUrl = "webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + contentTypeList + "&totalrecords=" + TotalRecords + "&makeid=" + MakeId;
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
        }

    }    
}