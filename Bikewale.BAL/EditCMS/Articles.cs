using Bikewale.BAL.GrpcFiles;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using Grpc.CMS;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.BAL.EditCMS
{
    /// <summary>
    /// Author : Vivek Gupta 
    /// Date : 5/5/2016
    /// Desc : news/expert news articles from cw
    /// </summary>
    public class Articles : IArticles
    {

        public int TotalRecords { get; set; }
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }

        string cacheKey = "BW_CMS_";
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);


        /// <summary>
        /// Written By : Ashish G. Kamble on 28 Feb 2016
        /// Summary : Function to get the data from the carwale apis. This data is cached on the bikewale.
        /// </summary>
        /// <param name="rptr"></param>
        public IEnumerable<ArticleSummary> GetRecentNews(int makeId, int modelId, int totalRecords)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            MakeId = makeId;
            ModelId = modelId;
            TotalRecords = totalRecords;

            try
            {

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.News);
                categorList.Add(EnumCMSContentType.AutoExpo2016);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                //cacheKey += contentTypeList.Replace(",", "_") + "_Cnt_" + TotalRecords;

                //if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                //{
                //    if (ModelId.HasValue && ModelId.Value > 0)
                //    {
                //        cacheKey += "_Make_" + MakeId + "_Model_" + ModelId;
                //    }
                //    else
                //    {
                //        cacheKey += "_Make_" + MakeId;
                //    }
                //}

                //using (IUnityContainer container = new UnityContainer())
                //{
                //    container.RegisterType<ICacheManager, MemcacheManager>();
                //    ICacheManager _cache = container.Resolve<ICacheManager>();

                //    _objArticleList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 15, 0), () => GetNewsFromCW(contentTypeList));
                //}

                _objArticleList = GetNewsFromCW(contentTypeList);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }

        /// <summary>
        /// Author : Vivek Gupta 
        /// Date : 05-may-2016
        /// Desc : Get Recent news articles from cw
        /// </summary>
        /// <param name="contentTypeList"></param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetNewsFromCW(string contentTypeList)
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
                        return null;
                    }
                }
                else
                {
                    return GetNewsFromCWAPIInOldWay(contentTypeList);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
                return GetNewsFromCWAPIInOldWay(contentTypeList);
            }
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 28 Feb 2016
        /// Summary : Function to get the data from the carwale cms api.
        /// </summary>
        /// <param name="contentTypeList">comma separated content ids.</param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetNewsFromCWAPIInOldWay(string contentTypeList)
        {
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {
                string _apiUrl = String.Format("webapi/article/mostrecentlist/?applicationid=2&contenttypes={0}&totalrecords={1}", contentTypeList, TotalRecords);

                if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                {
                    if (ModelId.HasValue && ModelId.Value > 0)
                    {
                        _apiUrl = String.Format("webapi/article/mostrecentlist/?applicationid=2&contenttypes={0}&totalrecords={1}&makeid={2}&modelid={3}", contentTypeList, TotalRecords, MakeId, ModelId);
                    }
                    else
                    {
                        _apiUrl = String.Format("webapi/article/mostrecentlist/?applicationid=2&contenttypes={0}&totalrecords={1}&makeid={2}", contentTypeList, TotalRecords, MakeId);
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


        /// <summary>
        /// Author : Vivek Gupta 
        /// Date : 5-5-2016
        /// Summary : Function to bind the expert reviews control. Function will cache the data from CW api on bikewale
        /// </summary>
        public IEnumerable<ArticleSummary> GetRecentExpertReviews(int makeId, int modelId, int totalRecords)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            MakeId = makeId;
            ModelId = modelId;
            TotalRecords = totalRecords;

            try
            {

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);

                //cacheKey += _contentType.Replace(",", "_") + "_Cnt_" + TotalRecords;

                //if (MakeId.HasValue && MakeId.Value > 0 || ModelId.HasValue && ModelId.Value > 0)
                //{
                //    if (ModelId.HasValue && ModelId.Value > 0)
                //        cacheKey += "_Make_" + MakeId + "_Model_" + ModelId;
                //    else
                //        cacheKey += "_Make_" + MakeId;
                //}

                //using (IUnityContainer container = new UnityContainer())
                //{
                //    container.RegisterType<ICacheManager, MemcacheManager>();
                //    ICacheManager _cache = container.Resolve<ICacheManager>();

                //    _objArticleList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 15, 0), () => GetNewsFromCW(_contentType));
                //}

                _objArticleList = GetNewsFromCW(_contentType);
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
