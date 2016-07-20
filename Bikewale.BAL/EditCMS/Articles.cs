using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
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
                _objArticleList = GetMostRecentArticlesViaGrpc(categoryIdList, totalRecords, makeId, modelId);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
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
        public IEnumerable<ArticleSummary> GetMostRecentArticlesById(EnumCMSContentType contentType, uint totalRecords, uint makeId, uint modelId)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;
            try
            {
                string categoryIdList = string.Empty;
                switch (contentType)
                {
                    case EnumCMSContentType.RoadTest:
                        categoryIdList = (short)contentType + "," + (short)EnumCMSContentType.ComparisonTests;
                        break;

                    case EnumCMSContentType.News:
                        categoryIdList = (short)contentType + "," + (short)EnumCMSContentType.AutoExpo2016;
                        break;

                    default:
                        categoryIdList = ((short)contentType).ToString();
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
        /// <param name="categoryId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategory(EnumCMSContentType categoryId, int startIndex, int endIndex, int makeId, int modelId)
        {
            CMSContent _objArticleList = null;
            try
            {
                _objArticleList = GetArticlesByCategoryViaGrpc(categoryId, startIndex, endIndex, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return _objArticleList;
        }

        private CMSContent GetArticlesByCategoryViaGrpc(EnumCMSContentType categoryId, int startIndex, int endIndex, int makeId, int modelId)
        {
            try
            {
                if (_useGrpc)
                {
                    string categotyListId;
                    switch (categoryId)
                    {
                        case EnumCMSContentType.RoadTest:
                            categotyListId = (int)categoryId + "," + (int)EnumCMSContentType.ComparisonTests;
                            break;

                        case EnumCMSContentType.News:
                            categotyListId = (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
                            break;
                        default:
                            categotyListId = ((int)categoryId).ToString();
                            break;

                    }

                    var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categotyListId, (uint)startIndex, (uint)endIndex, makeId, modelId);

                    if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                    {
                        return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);

                    }
                    else
                    {
                        return GetCMSContentOldWay(categoryId, startIndex, endIndex);
                    }

                }
                else
                {
                    return GetCMSContentOldWay(categoryId, startIndex, endIndex);
                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
                return GetCMSContentOldWay(categoryId, startIndex, endIndex);
            }
        }

        private CMSContent GetCMSContentOldWay(EnumCMSContentType categoryId, int startIndex, int endIndex)
        {
            CMSContent objFeaturedArticles = null;
            try
            {
                if (_logGrpcErrors)
                {
                    _logger.Error(string.Format("Grpc did not work for GetCMSContentOldWay {0}", categoryId));
                }

                string apiUrl = "/webapi/article/listbycategory/?applicationid=2";

                if (categoryId == EnumCMSContentType.RoadTest)
                {
                    apiUrl += "&categoryidlist=" + (int)categoryId + "," + (int)EnumCMSContentType.ComparisonTests;
                }
                else if (categoryId == EnumCMSContentType.News)
                {
                    apiUrl += "&categoryidlist=" + (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
                }
                else
                {
                    apiUrl += "&categoryidlist=" + (int)categoryId;
                }
                apiUrl += "&startindex=" + startIndex + "&endindex=" + endIndex;

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

    }
}
