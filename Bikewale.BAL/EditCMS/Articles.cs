using Bikewale.BAL.GrpcFiles;
using Bikewale.Cache.Core;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using Grpc.CMS;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string cacheKey = "BW_CMS_";

        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(Articles));
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

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICacheManager, MemcacheManager>();
                    ICacheManager _cache = container.Resolve<ICacheManager>();

                    _objArticleList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 15, 0), () => GetNewsFromCW(contentTypeList));
                }

                //_objArticleList = GetNewsFromCW(contentTypeList);
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

                    if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                    }
                    else
                    {
                        return GetNewsFromCWAPIInOldWay(contentTypeList);
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

                    _objArticleList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 15, 0), () => GetNewsFromCW(_contentType));
                }

                //_objArticleList = GetNewsFromCW(_contentType);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticleDetails GetNewsDetails(uint basicId)
        {
            ArticleDetails _objArticleList = null;
            try
            {
                _objArticleList = GetNewsDetailsViaGrpc(basicId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private ArticleDetails GetNewsDetailsFromApiOldWay(uint basicId)
        {
            ArticleDetails objArticle = null;
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetArticlePhotos {0}", basicId));
                }

                //sets the base URI for HTTP requests
                string _apiUrl = String.Format("webapi/article/contentdetail/?basicid={0}", basicId);

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objArticle = objClient.GetApiResponseSync<ArticleDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objArticle);
                }

            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }


            return objArticle;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private ArticleDetails GetNewsDetailsViaGrpc(uint basicId)
        {
            ArticleDetails objArticle = null;
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcArticle = GrpcMethods.GetContentDetails(Convert.ToUInt64(basicId));

                    if (_objGrpcArticle != null)
                    {
                        objArticle = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);
                    }
                    else
                    {
                        objArticle = GetNewsDetailsFromApiOldWay(basicId);
                    }
                }
                else
                {
                    objArticle = GetNewsDetailsFromApiOldWay(basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                objArticle = GetNewsDetailsFromApiOldWay(basicId);
            }

            return objArticle;
        }


        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 19-7-2016
        /// Desc : Get News From GRPC
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetArticlesViaGrpc(EnumCMSContentType categoryId, uint posts, uint makeId, uint modelId)
        {
            string cacheKey = String.Format("BW_AS_C_{0}_TR_{1}_", categoryId, posts);

            if (makeId > 0 && modelId > 0)
            {

                cacheKey += String.Format("_Make_{0}_Model_{1}" + makeId, modelId);
            }
            else if (makeId > 0 && modelId == 0)
            {
                cacheKey += String.Format("_Make_{0}", makeId);
            }
            else if (makeId == 0 && modelId > 0)
            {
                cacheKey += String.Format("_Model_{0}", modelId);
            }

            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<ICacheManager, MemcacheManager>();
                ICacheManager _cache = container.Resolve<ICacheManager>();
                return _cache.GetFromCache<IEnumerable<ArticleSummary>>(cacheKey, new TimeSpan(0, 30, 0), () => GetNews(categoryId, posts, makeId, modelId));
            }
        }

        /// <summary>
        /// Created By : Vivek Gupta
        /// Date : 19-7-2016
        /// Desc : Get News
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetNews(EnumCMSContentType categoryId, uint posts, uint makeId = 0, uint modelId = 0)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {

                if (_useGrpc)
                {
                    int intMakeId = Convert.ToInt32(makeId);
                    int intModelId = Convert.ToInt32(modelId);
                    string categoryIdList;
                    switch (categoryId)
                    {
                        case EnumCMSContentType.RoadTest:
                            categoryIdList = (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
                            break;

                        case EnumCMSContentType.News:
                            categoryIdList = (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
                            break;

                        default:
                            categoryIdList = ((short)categoryId).ToString();
                            break;
                    }

                    var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)posts, intMakeId, intModelId);

                    if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                    }
                    else
                    {
                        return GetArticlesViaOldWay(categoryId, posts, makeId, modelId);
                    }
                }
                else
                {
                    return GetArticlesViaOldWay(categoryId, posts, makeId, modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }

        private IEnumerable<ArticleSummary> GetArticlesViaOldWay(EnumCMSContentType categoryId, uint posts, uint makeId, uint modelId)
        {
            if (_logGrpcErrors)
            {
                _logger.Error(string.Format("Grpc did not work for GetCMSContentOldWay {0}", categoryId));
            }

            string apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&totalrecords=" + posts;
            IEnumerable<ArticleSummary> objRecentArticles = null;
            if (categoryId == EnumCMSContentType.RoadTest)
            {
                apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
            }
            else if (categoryId == EnumCMSContentType.News)
            {
                apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
            }
            else
            {
                apiUrl += "&contenttypes=" + (short)categoryId;
            }

            if (modelId == 0)
            {
                apiUrl += "&makeid=" + makeId;
            }
            else
            {
                apiUrl += "&makeid=" + makeId + "&modelid=" + modelId;
            }


            using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
            {
                //objRecentArticles = objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
                return objClient.GetApiResponseSync<IEnumerable<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
            }
        }
    }
}
