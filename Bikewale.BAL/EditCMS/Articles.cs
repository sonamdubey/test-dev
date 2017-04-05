using Bikewale.BAL.GrpcFiles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
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

        //public int TotalRecords { get; set; }
        //public int? MakeId { get; set; }
        //public int? ModelId { get; set; }
        public int FetchedRecordsCount { get; set; }


        static bool _logGrpcErrors = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.LogGrpcErrors);
        static readonly ILog _logger = LogManager.GetLogger(typeof(Articles));
        static bool _useGrpc = Convert.ToBoolean(Bikewale.Utility.BWConfiguration.Instance.UseGrpc);

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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
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
                switch (categoryIdList)
                {
                    case "8": //EnumCMSContentType.RoadTest
                        categoryIdList = Convert.ToString((int)EnumCMSContentType.RoadTest) + "," + (short)EnumCMSContentType.ComparisonTests;
                        break;

                    case "1": //EnumCMSContentType.News
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
                switch (categoryIdList)
                {
                    case "8": //EnumCMSContentType.RoadTest
                        categoryIdList = Convert.ToString((int)EnumCMSContentType.RoadTest) + "," + (short)EnumCMSContentType.ComparisonTests;
                        break;

                    case "1": //EnumCMSContentType.News
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
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles by list according to pagination using carwale api call
        /// </summary>
        /// <param name="categoryIds"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
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
                    nvc.Add("ContentId", basicId.ToString());
                    SyncBWData.PushToQueue("editcms.UpdateContentViewCount", DataBaseName.CW, nvc);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
        }
        #endregion

    }
}
