﻿using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Grpc.CMS;
using log4net;
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

        //public int TotalRecords { get; set; }
        //public int? MakeId { get; set; }
        //public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }


        static bool _logGrpcErrors = Convert.ToBoolean(ConfigurationManager.AppSettings["LogGrpcErrors"]);
        static readonly ILog _logger = LogManager.GetLogger(typeof(Articles));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

        #region Get News Details
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
        #endregion


        #region MostRecentArticles List

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="totalRecords"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {
                switch (categoryIdList)
                {
                    case "8":
                        categoryIdList = Convert.ToString((int)EnumCMSContentType.RoadTest) + "," + (short)EnumCMSContentType.ComparisonTests;
                        break;

                    case "1":
                        categoryIdList = Convert.ToString((int)EnumCMSContentType.News) + "," + (short)EnumCMSContentType.AutoExpo2016;
                        break;

                    default:
                        break;
                }

                _objArticleList = GetMostRecentArticlesViaGrpc(categoryIdList, totalRecords, makeId, modelId);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }

        private IEnumerable<ArticleSummary> GetMostRecentArticlesViaGrpc(string categoryIdList, uint totalRecords, uint makeId, uint modelId)
        {
            try
            {
                if (_useGrpc)
                {
                    int intMakeId = Convert.ToInt32(makeId);
                    int intModelId = Convert.ToInt32(modelId);

                    var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)totalRecords, intMakeId, intModelId);

                    if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                    }
                    else
                    {
                        return GetArticlesViaOldWay(categoryIdList, totalRecords, makeId, modelId);
                    }
                }
                else
                {
                    return GetArticlesViaOldWay(categoryIdList, totalRecords, makeId, modelId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetArticlesViaOldWay(categoryIdList, totalRecords, makeId, modelId);
            }
        }

        private IEnumerable<ArticleSummary> GetArticlesViaOldWay(string contentTypeList, uint totalRecords, uint makeId, uint modelId)
        {
            if (_logGrpcErrors)
            {
                _logger.Error(string.Format("Grpc did not work for GetCMSContentOldWay {0}", contentTypeList));
            }

            IEnumerable<ArticleSummary> objRecentArticles = null;

            try
            {
                string _apiUrl = String.Format("webapi/article/mostrecentlist/?applicationid=2&contenttypes={0}&totalrecords={1}", contentTypeList, totalRecords);

                if (makeId > 0 || modelId > 0)
                {
                    if (modelId > 0)
                    {
                        _apiUrl = String.Format("webapi/article/mostrecentlist/?applicationid=2&contenttypes={0}&totalrecords={1}&makeid={2}&modelid={3}", contentTypeList, totalRecords, makeId, modelId);
                    }
                    else
                    {
                        _apiUrl = String.Format("webapi/article/mostrecentlist/?applicationid=2&contenttypes={0}&totalrecords={1}&makeid={2}", contentTypeList, totalRecords, makeId);
                    }
                }


                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    return objClient.GetApiResponseSync<IEnumerable<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objRecentArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objRecentArticles;
        }
        #endregion


        #region ArticlesByCategory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId)
        {
            CMSContent _objArticleList = null;
            try
            {
                switch (categoryIdList)
                {
                    case "8":
                        categoryIdList = Convert.ToString((int)EnumCMSContentType.RoadTest) + "," + (short)EnumCMSContentType.ComparisonTests;
                        break;

                    case "1":
                        categoryIdList = Convert.ToString((int)EnumCMSContentType.News) + "," + (short)EnumCMSContentType.AutoExpo2016;
                        break;

                    default:
                        break;
                }

                _objArticleList = GetArticlesByCategoryViaGrpc(categoryIdList, startIndex, endIndex, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }

        private CMSContent GetArticlesByCategoryViaGrpc(string categoryIds, int startIndex, int endIndex, int makeId, int modelId)
        {
            try
            {
                if (_useGrpc)
                {

                    var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categoryIds, (uint)startIndex, (uint)endIndex, makeId, modelId);

                    if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);

                    }
                    else
                    {
                        return GetCMSContentOldWay(categoryIds, startIndex, endIndex, makeId, modelId);
                    }

                }
                else
                {
                    return GetCMSContentOldWay(categoryIds, startIndex, endIndex, makeId, modelId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetCMSContentOldWay(categoryIds, startIndex, endIndex, makeId, modelId);
            }
        }

        private CMSContent GetCMSContentOldWay(string categoryIds, int startIndex, int endIndex, int makeId, int modelId)
        {
            CMSContent objFeaturedArticles = null;
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetCMSContentOldWay {0}", categoryIds));
                }

                string apiUrl = string.Format("/webapi/article/listbycategory/?applicationid=2&categoryidlist={0}&startindex={1}&endindex={2}", categoryIds, startIndex, endIndex);
                if (makeId > 0 || modelId > 0)
                {
                    if (makeId > 0)
                    {
                        apiUrl = string.Format("{0}&makeid={1}", apiUrl, makeId);
                    }
                    else
                    {
                        apiUrl = string.Format("{0}&modelid={1}", apiUrl, modelId);
                    }
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    return objClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objFeaturedArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objFeaturedArticles;
        }
        #endregion


        #region Article Details

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching and used for feature details
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetArticleDetails(uint basicId)
        {
            ArticlePageDetails objFeature = null;
            try
            {
                if (_useGrpc)
                {
                    var _objGrpcFeature = GrpcMethods.GetContentPages((ulong)basicId);

                    if (_objGrpcFeature != null)
                    {
                        objFeature = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcFeature);
                    }
                    else
                    {
                        objFeature = GetArticleDetailsOldWay(basicId);
                    }
                }
                else
                {
                    objFeature = GetArticleDetailsOldWay(basicId);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                objFeature = GetArticleDetailsOldWay(basicId);
            }

            return objFeature;

        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private ArticlePageDetails GetArticleDetailsOldWay(uint basicId)
        {
            ArticlePageDetails objFeature = null;

            try
            {

                string _apiUrl = "webapi/article/contentpagedetail/?basicid=" + basicId;

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objFeature = objClient.GetApiResponseSync<ArticlePageDetails>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objFeature);
                }


                if (_logGrpcErrors && objFeature != null)
                {
                    _logger.Error(string.Format("Grpc did not work for GetFeatureDetailsOldWay {0}", basicId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objFeature;
        }

        #endregion


        #region Article Photos
        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetArticlePhotos(int basicId)
        {
            IEnumerable<ModelImage> objImages = null;
            try
            {
                if (_useGrpc)
                {

                    var _objGrpcArticlePhotos = GrpcMethods.GetArticlePhotos((ulong)basicId);

                    if (_objGrpcArticlePhotos != null && _objGrpcArticlePhotos.LstGrpcModelImage.Count > 0)
                    {
                        //following needs to be optimized
                        objImages = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticlePhotos);
                    }
                    else
                    {
                        objImages = GetArticlePhotosOldWay(basicId);
                    }

                }
                else
                {
                    objImages = GetArticlePhotosOldWay(basicId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : in Photos(int basicId)");
                objErr.SendMail();
                objImages = GetArticlePhotosOldWay(basicId);
            }

            return objImages;
        }

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/features/view.aspx.cs for caching
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private IEnumerable<ModelImage> GetArticlePhotosOldWay(int basicId)
        {

            IEnumerable<ModelImage> objImages = null;
            try
            {
                string _apiUrl = "webapi/image/GetArticlePhotos/?basicid=" + basicId;

                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetArticlePhotos {0}", basicId));
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    objImages = objClient.GetApiResponseSync<IEnumerable<ModelImage>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, objImages);
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return objImages;
        }
        #endregion

    }
}
