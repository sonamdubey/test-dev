using Bikewale.BAL.GrpcFiles;
using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Service.Utilities;
using Grpc.CMS;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.Interfaces.News;

namespace Bikewale.Service.Controllers.CMS
{

    /// <summary>
    /// Edit CMS List Controller :  Operations related to list of content 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class CMSListController : CompressionApiController//ApiController
    {
        ICMSCacheContent _objCMSContent = null;
        private readonly IPager _pager = null;
        public CMSListController(IPager pager, ICMSCacheContent objCMSContent)
        {
            _pager = pager;
            _objCMSContent = objCMSContent;
        }

        #region List Recent Categories Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of a specified category.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>      
        /// <param name="posts">No of records needed. should be greater than 0.</param>
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/{categoryId}/posts/{posts}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts)
        {
            try
            {
                IEnumerable<ArticleSummary> objRecentArticles = _objCMSContent.GetMostRecentArticlesById(categoryId, posts, 0, 0);

                if (objRecentArticles != null && objRecentArticles.Count() > 0)
                {

                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);

                    objRecentArticles = null;
                    objCMSRArticles = new CMSShareUrl().GetShareUrl(objCMSRArticles);
                    return Ok(objCMSRArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 
        #endregion

        #region List Recent Categories Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of a specified category for a particular make or model
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>
        /// <param name="posts">No of records needed. should be greater than 0.</param>
        /// <param name="makeId">Mandetory field.</param>
        /// <param name="modelId">Optional.</param>        
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/{categoryId}/posts/{posts}/make/{makeId}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts, string makeId, string modelId = null)
        {
            try
            {
                IEnumerable<ArticleSummary> objRecentArticles = _objCMSContent.GetMostRecentArticlesById(categoryId, posts, Convert.ToUInt32(makeId), Convert.ToUInt32(modelId));

                if (objRecentArticles != null && objRecentArticles.Count() > 0)
                {
                    List<CMSArticleSummary> objCMSRArticles;
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);

                    objRecentArticles = null;
                    objCMSRArticles = new CMSShareUrl().GetShareUrl(objCMSRArticles);
                    return Ok(objCMSRArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 


        //private List<ArticleSummary> GetArticlesViaGrpc(EnumCMSContentType categoryId, uint posts, string makeId, string modelId)
        //{
        //    try
        //    {
        //        if (_useGrpc)
        //        {
        //            int intMakeId = string.IsNullOrEmpty(makeId) ? 0 : Convert.ToInt32(makeId);
        //            int intModelId = string.IsNullOrEmpty(modelId) ? 0 : Convert.ToInt32(modelId);
        //            string categoryIdList;
        //            switch (categoryId)
        //            {
        //                case EnumCMSContentType.RoadTest:
        //                    categoryIdList = (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
        //                    break;

        //                case EnumCMSContentType.News:
        //                    categoryIdList = (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
        //                    break;

        //                default:
        //                    categoryIdList = ((short)categoryId).ToString();
        //                    break;
        //            }

        //            var _objGrpcArticleSummaryList = GrpcMethods.MostRecentList(categoryIdList, (int)posts, intMakeId, intModelId);

        //            if (_objGrpcArticleSummaryList != null && _objGrpcArticleSummaryList.LstGrpcArticleSummary.Count > 0)
        //            {
        //                return GrpcToBikeWaleConvert.ConvertFromGrpcToBikeWale(_objGrpcArticleSummaryList);
        //            }
        //            else
        //            {
        //                return GetArticlesViaOldWay(categoryId, posts, makeId, modelId);
        //            }
        //        }
        //        else
        //        {
        //            return GetArticlesViaOldWay(categoryId, posts, makeId, modelId);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        _logger.Error(err.Message, err);
        //        return GetArticlesViaOldWay(categoryId, posts, makeId, modelId);
        //    }
        //}

        //private List<ArticleSummary> GetArticlesViaOldWay(EnumCMSContentType categoryId, uint posts, string makeId, string modelId)
        //{
        //    if (_logGrpcErrors)
        //    {
        //        _logger.Error(string.Format("Grpc did not work for GetCMSContentOldWay {0}", categoryId));
        //    }

        //    string apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&totalrecords=" + posts;
        //    string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];
        //    List<ArticleSummary> objRecentArticles = null;
        //    if (categoryId == EnumCMSContentType.RoadTest)
        //    {
        //        apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
        //    }
        //    else if (categoryId == EnumCMSContentType.News)
        //    {
        //        apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
        //    }
        //    else
        //    {
        //        apiUrl += "&contenttypes=" + (short)categoryId;
        //    }
        //    if (String.IsNullOrEmpty(modelId))
        //    {
        //        apiUrl += "&makeid=" + makeId;
        //    }
        //    else
        //    {
        //        apiUrl += "&makeid=" + makeId + "&modelid=" + modelId;
        //    }
        //    using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
        //    {
        //        //objRecentArticles = objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
        //        return objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
        //    }
        //}

        #endregion


        #region List Category Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of specified category. This api returns data for given page number.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>        
        /// <param name="posts">No of records per page. Should be greater than 0.</param>
        /// <param name="pageNumber">page number for which data is required.</param>
        /// <returns>Category Content List</returns>
        [ResponseType(typeof(IEnumerable<Bikewale.DTO.CMS.Articles.CMSContent>)), Route("api/cms/cat/{categoryId}/posts/{posts}/pn/{pageNumber}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, int posts, int pageNumber)
        {
            Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles = null;
            try
            {
                int startIndex = 0, endIndex = 0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                objFeaturedArticles = _objCMSContent.GetArticlesByCategory(categoryId, startIndex, endIndex, 0, 0);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    objFeaturedArticles.Articles.Clear();
                    objFeaturedArticles.Articles = null;
                    objCMSFArticles = new CMSShareUrl().GetShareUrl(objCMSFArticles);
                    return Ok(objCMSFArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }


        //get 
        #endregion


        #region List Category Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of specified category for the given make or model. This api return data for given page number.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>
        /// <param name="makeId">Mandatory parameter.</param>
        /// <param name="modelId">Optional parameter.</param>        
        /// <param name="posts">No of records per page. Should be greater than 0.</param>
        /// <param name="pageNumber">page number for which data is required.</param>
        /// <returns>Category Content List</returns>
        [ResponseType(typeof(IEnumerable<Bikewale.DTO.CMS.Articles.CMSContent>)), Route("api/cms/cat/{categoryId}/posts/{posts}/pn/{pageNumber}/make/{makeId}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, int posts, int pageNumber, string makeId, string modelId = null)
        {
            Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles = null;
            try
            {
                int startIndex = 0, endIndex = 0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                int intMakeId = string.IsNullOrEmpty(makeId) ? 0 : Convert.ToInt32(makeId);
                int intModelId = string.IsNullOrEmpty(modelId) ? 0 : Convert.ToInt32(modelId);

                objFeaturedArticles = _objCMSContent.GetArticlesByCategory(categoryId, startIndex, endIndex, intMakeId, intModelId);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    if (objFeaturedArticles.Articles != null)
                    {
                        objFeaturedArticles.Articles.Clear();
                        objFeaturedArticles.Articles = null;
                    }

                    objCMSFArticles = new CMSShareUrl().GetShareUrl(objCMSFArticles);
                    return Ok(objCMSFArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 
        #endregion
    }   // class
}   // namespace
