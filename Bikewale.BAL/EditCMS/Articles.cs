using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public int FetchedRecordsCount { get; set; }
        static readonly ILog _logger = LogManager.GetLogger(typeof(Articles));

        #region Get News Details
        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for News Details based on basic id 
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for News Details based on basic id using grpc
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private ArticleDetails GetNewsDetailsViaGrpc(uint basicId)
        {
            ArticleDetails objArticle = null;
            try
            {

                var _objGrpcArticle = GrpcMethods.GetContentDetails(Convert.ToUInt64(basicId));

                if (_objGrpcArticle != null)
                {
                    objArticle = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return objArticle;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for News Details based on basic id using grpc
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        private PwaArticleDetails GetNewsDetailsViaGrpcForPwa(uint basicId)
        {
            PwaArticleDetails objArticle = null;
            try
            {

                var _objGrpcArticle = GrpcMethods.GetContentDetails(Convert.ToUInt64(basicId));

                if (_objGrpcArticle != null)
                {
                    objArticle = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWalePwa(_objGrpcArticle);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return objArticle;
        }

        #endregion


        #region MostRecentArticles List

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Most recent Articles Details based on based on contentslistIds and make,model
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
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetMostRecentArticlesViaGrpc(categoryIdList, totalRecords, makeId, modelId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="totalRecords"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, string modelIds)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetMostRecentArticlesViaGrpc(categoryIdList, totalRecords, makeId, modelIds);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 14 June 2017
        /// Summary    : To get recent articles based on a body style
        /// </summary>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, string bodyStyleId, uint makeId, uint modelId)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetMostRecentArticlesViaGrpc(categoryIdList, totalRecords, bodyStyleId, makeId, modelId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="totalRecords"></param>
        /// <param name="bodyStyleId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, string bodyStyleId, uint makeId, string modelIds)
        {
            FetchedRecordsCount = 0;
            IEnumerable<ArticleSummary> _objArticleList = null;

            try
            {
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetMostRecentArticlesViaGrpc(categoryIdList, totalRecords, bodyStyleId, makeId, modelIds);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Most recent Articles Details based on based on contentslistIds and make,model using grpc
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="totalRecords"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetMostRecentArticlesViaGrpc(string categoryIdList, uint totalRecords, uint makeId, uint modelId)
        {
            try
            {

                int intMakeId = Convert.ToInt32(makeId);
                int intModelId = Convert.ToInt32(modelId);

                var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)totalRecords, intMakeId, intModelId);

                if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="totalRecords"></param>
        /// <param name="makeId"></param>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetMostRecentArticlesViaGrpc(string categoryIdList, uint totalRecords, uint makeId, string modelIds)
        {
            try
            {

                int intMakeId = Convert.ToInt32(makeId);

                var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)totalRecords, intMakeId, modelIds);

                if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        private IEnumerable<ArticleSummary> GetMostRecentArticlesViaGrpc(string categoryIdList, uint totalRecords, string bodyStyleId, uint makeId, uint modelId)
        {
            try
            {
                int intMakeId = Convert.ToInt32(makeId);
                int intModelId = Convert.ToInt32(modelId);

                var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)totalRecords, bodyStyleId, intMakeId, intModelId);

                if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="totalRecords"></param>
        /// <param name="bodyStyleId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelIds"></param>
        /// <returns></returns>
        private IEnumerable<ArticleSummary> GetMostRecentArticlesViaGrpc(string categoryIdList, uint totalRecords, string bodyStyleId, uint makeId, string modelIds)
        {
            try
            {
                int intMakeId = Convert.ToInt32(makeId);

                var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)totalRecords, bodyStyleId, intMakeId, modelIds);

                if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        #endregion


        #region ArticlesByCategory

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles by list according to pagination
        /// Modified By: Subodh Jain on 10 Nov 2016
        /// Description : Added TipsAndAdvices case
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
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetArticlesByCategoryViaGrpc(categoryIdList, startIndex, endIndex, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles by list according to pagination
        /// Modified By: Subodh Jain on 10 Nov 2016
        /// Description : Added TipsAndAdvices case
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, string modelIds)
        {
            CMSContent _objArticleList = null;
            try
            {
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetArticlesByCategoryViaGrpc(categoryIdList, startIndex, endIndex, makeId, modelIds);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles by list according to pagination using grpc
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private CMSContent GetArticlesByCategoryViaGrpc(string categoryIds, int startIndex, int endIndex, int makeId, int modelId)
        {
            try
            {

                var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categoryIds, (uint)startIndex, (uint)endIndex, makeId, modelId);

                if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);

                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 22nd Sep 2017
        /// Description : Addded new overload method to fetch data according to categorylist with multiple model ids 
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private CMSContent GetArticlesByCategoryViaGrpc(string categoryIds, int startIndex, int endIndex, int makeId, string modelIds)
        {
            try
            {

                var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categoryIds, (uint)startIndex, (uint)endIndex, makeId, modelIds);

                if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);

                }
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return null;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 16th Aug 2017
        /// Summary: Get articles for given category and body style using grpc
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="bodyStyleId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, string bodyStyleId, int makeId)
        {
            CMSContent _objArticleList = null;
            try
            {
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetArticlesByCategoryViaGrpc(categoryIdList, startIndex, endIndex, bodyStyleId, makeId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.BAL.EditCMS.Articles.GetArticlesByCategoryList");

            }

            return _objArticleList;
        }

        public PwaContentBase GetArticlesByCategoryListPwa(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId)
        {
            PwaContentBase _objArticleList = null;
            try
            {
                categoryIdList = ReturnCategoryIds(categoryIdList);
                _objArticleList = GetArticlesByCategoryViaGrpcPwa(categoryIdList, startIndex, endIndex, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }

            return _objArticleList;
        }

        /// <summary>
        /// Created by: Prasad Gawde 25th Sept 2017
        /// Summary: Get articles for given category and body style using grpc
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="bikeBodyType"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        private PwaContentBase GetArticlesByCategoryViaGrpcPwa(string categoryIds, int startIndex, int endIndex, int makeId, int modelId)
        {
            try
            {
                var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categoryIds, (uint)startIndex, (uint)endIndex, makeId, modelId);

                if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWalePwa(_objGrpcArticle);

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                ErrorClass.LogError(ex, "Bikewale.BAL.EditCMS.Articles.GetArticlesByCategoryViaGrpc");
            }
            return null;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 16th Aug 2017
        /// Summary: Get articles for given category and body style using grpc
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="bikeBodyType"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        private CMSContent GetArticlesByCategoryViaGrpc(string categoryIds, int startIndex, int endIndex, string bodyStyleId, int makeId)
        {
            try
            {
                var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categoryIds, (uint)startIndex, (uint)endIndex, bodyStyleId, makeId);

                if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                ErrorClass.LogError(ex, "Bikewale.BAL.EditCMS.Articles.GetArticlesByCategoryViaGrpc");
            }
            return null;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 20-Sep-2017
        /// Description : Method to get articles by category list with parameters 'categoryIdList, startIndex, endIndex'.
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex)
        {
            try
            {
                var _objGrpcArticle = GrpcMethods.GetArticleListByCategory(categoryIdList, (uint)startIndex, (uint)endIndex);

                if (_objGrpcArticle != null && _objGrpcArticle.RecordCount > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticle);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            return null;
        }


        #endregion


        #region Article Details

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles Details based on basic id
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetArticleDetails(uint basicId)
        {
            ArticlePageDetails objFeature = null;
            try
            {

                var _objGrpcFeature = GrpcMethods.GetContentPages((ulong)basicId);

                if (_objGrpcFeature != null)
                {
                    objFeature = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcFeature);
                }

            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }
            return objFeature;
        }


        #endregion


        #region Article Photos
        /// <summary>
        /// Created By : Vivek Gupta on 18th July 2016
        /// Description : Caching for Articles Photos based on basic id
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetArticlePhotos(int basicId)
        {
            IEnumerable<ModelImage> objImages = null;
            try
            {
                var _objGrpcArticlePhotos = GrpcMethods.GetArticlePhotos((ulong)basicId);

                if (_objGrpcArticlePhotos != null && _objGrpcArticlePhotos.LstGrpcModelImage.Count > 0)
                {
                    //following needs to be optimized
                    objImages = GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticlePhotos);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }

            return objImages;
        }

        #endregion

        #region ContentListBySubCategory
        /// <summary>
        /// Created by : Ashutosh Sharma on 13 Dec 2017
        /// Description : Method to get content list by category and subcategory id.
        /// </summary>
        public CMSContent GetContentListBySubCategoryId(uint startIndex, uint endIndex, string categoryIdList, string subCategoryIdList, int makeId = 0, int modelId = 0)
        {
            try
            {
                var _objGrpcContent = GrpcMethods.GetContentListBySubCategoryId(startIndex, endIndex, categoryIdList, subCategoryIdList, makeId, modelId);

                if (_objGrpcContent != null && _objGrpcContent.RecordCount > 0)
                {
                    return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcContent);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
            return null;
        }
        #endregion
        #region Update the View Count
        /// <summary>
        /// Created by  :   Sumit Kate on 25 July 2016
        /// Description :   Updates the View count
        /// </summary>
        /// <param name="basicId"></param>
        public void UpdateViewCount(uint basicId)
        {
            try
            {
                if (basicId > 0)
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("v_ContentId", basicId.ToString());
                    SyncBWData.PushToQueue("editcms.UpdateContentViewCount", DataBaseName.CW, nvc);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);

            }
        }
        #endregion

        #region common business logic
        /// <summary>
        /// Returns the category ids.
        /// Created by : Sangram Nandkhile on 28 Nov 2017
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        private string ReturnCategoryIds(string categoryId)
        {
            switch (categoryId)
            {
                case "8": //EnumCMSContentType.RoadTest
                    categoryId = Convert.ToString((int)EnumCMSContentType.RoadTest) + "," + (short)EnumCMSContentType.ComparisonTests;
                    break;

                case "1": //EnumCMSContentType.News
                    categoryId = Convert.ToString((int)EnumCMSContentType.News) + "," + (short)EnumCMSContentType.AutoExpo2016 + "," + (short)EnumCMSContentType.AutoExpo2018;
                    break;

                case "6": //EnumCMSContentType.Features
                    categoryId = Convert.ToString((int)EnumCMSContentType.Features) + "," + (short)EnumCMSContentType.SpecialFeature;
                    break;
                default:
                    break;
            }

            return categoryId;
        }
        #endregion


    }
}
