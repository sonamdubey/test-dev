using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Service.Utilities;
using EditCMSWindowsService.Messages;
using Grpc.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

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

                IEnumerable<ArticleSummary> objRecentArticles = _objCMSContent.GetMostRecentArticlesByIdList(Convert.ToString((int)categoryId), posts, 0, 0);

                if (objRecentArticles != null && objRecentArticles.Any())
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

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
                IEnumerable<ArticleSummary> objRecentArticles = _objCMSContent.GetMostRecentArticlesByIdList(Convert.ToString((int)categoryId), posts, Convert.ToUInt32(makeId), Convert.ToUInt32(modelId));

                if (objRecentArticles != null && objRecentArticles.Any())
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

                return InternalServerError();
            }
            return NotFound();
        }  //get 

        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/V2/{categoryId}/posts/{posts}/make/{makeId}/")]
        public IHttpActionResult GetV2(EnumCMSContentType categoryId, uint posts, string makeId, string modelId = null)
        {
            try
            {
                IEnumerable<ArticleSummary> objRecentArticles = _objCMSContent.GetMostRecentArticlesByIdList(Convert.ToString((int)categoryId), posts, Convert.ToUInt32(makeId), Convert.ToUInt32(modelId));

                if (objRecentArticles != null && objRecentArticles.Any())
                {
                    List<CMSArticleSummaryMin> objCMSRArticles;
                    objCMSRArticles = CMSMapper.ConvertV2(objRecentArticles);
                    return Ok(objCMSRArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController.GetV2");

                return InternalServerError();
            }
            return NotFound();
        }

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

                objFeaturedArticles = _objCMSContent.GetArticlesByCategoryList(Convert.ToString((int)categoryId), startIndex, endIndex, 0, 0);

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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

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

                objFeaturedArticles = _objCMSContent.GetArticlesByCategoryList(Convert.ToString((int)categoryId), startIndex, endIndex, intMakeId, intModelId);

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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.CMS.CMSController");

                return InternalServerError();
            }
            return NotFound();
        }  //get 
        #endregion

        #region Clear Memcache Keys EditCMS
        /// <summary>
        /// Created By : Sajal Gupta on 22/09/2016
        /// Description: Clear memcached buckets of Grpc editCms of respective category Ids.
        /// </summary>
        [HttpGet, Route("api/cms/category/{catId}/refreshcache/")]
        public IHttpActionResult ClearEditCMSCacheKeys(int catId)
        {
            try
            {
                switch (catId)
                {
                    case 6:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, 0, 0);
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.Features, 0, 0);
                        break;
                    case 8:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, 0, 0);
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.ExpertReviews, 0, 0);
                        break;
                    case 2:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, 0, 0);
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.ExpertReviews, 0, 0);
                        break;
                    case 19:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, 0, 0);
                        break;
                    case 1:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, 0, 0);
                        break;
                    case 18:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.Features, 0, 0);
                        break;
                    case 11:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.Videos, 0, 0);
                        break;
                    default:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.All, 0, 0);
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception at Bikewale.Service.CMS.CMSController.ClearEditCMSCacheKeys for parameter catId : {0}", catId));

                return InternalServerError();
            }
        }
        #endregion

        [HttpGet, Route("api/cms/category/{catId}/refreshcache/{makeId}/{modelId}")]
        public IHttpActionResult ClearEditCMSCacheKeys(int catId, int makeId, int modelId)
        {
            try
            {
                switch (catId)
                {
                    case 6:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, makeId, modelId);
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.Features, makeId, modelId);
                        break;
                    case 8:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, makeId, modelId);
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.ExpertReviews, makeId, modelId);
                        break;
                    case 2:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, makeId, modelId);
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.ExpertReviews, makeId, modelId);
                        break;
                    case 19:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, makeId, modelId);
                        break;
                    case 1:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.News, makeId, modelId);
                        break;
                    case 18:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.Features, makeId, modelId);
                        break;
                    case 11:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.Videos, makeId, modelId);
                        break;
                    default:
                        GrpcMethods.ClearMemCachedKEys(EditCMSCategoryEnum.All, makeId, modelId);
                        break;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception at Bikewale.Service.CMS.CMSController.ClearEditCMSCacheKeys for parameter catId : {0},{1},{2}", catId, makeId, modelId));

                return InternalServerError();
            }
        }
    }   // class
}   // namespace


