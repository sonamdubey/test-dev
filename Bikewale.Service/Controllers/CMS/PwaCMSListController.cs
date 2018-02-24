using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PWA.CMS
{

    /// <summary>
    /// Edit CMS List Controller :  Operations related to list of content 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PwaCMSListController : CompressionApiController//ApiController
    {
        ICMSCacheContent _objCMSContent = null;
        private readonly IPager _pager = null;
        public PwaCMSListController(IPager pager, ICMSCacheContent objCMSContent)
        {
            _pager = pager;
            _objCMSContent = objCMSContent;
        }

        #region List Category Content PWA
        /// <summary>
        /// Modified By : Prasad Gawde
        /// Summary : API to get recent content of specified category for the given make or model. This api return data for given page number.
        /// Modified By : Sangram Nandkhile on 04 Mar 2016
        /// Summary : Utility function to fetch shareurl is used
        /// </summary>
        /// <param name="categoryIds">Id of the categories whose data is required.comma separated list</param> 
        /// <param name="posts">No of records per page. Should be greater than 0.</param>
        /// <param name="pageNumber">page number for which data is required.</param>
        /// <returns>Category Content List</returns>
        [ResponseType(typeof(IEnumerable<PwaContentBase>)), Route("api/pwa/cms/cat/{categoryIds}/posts/{posts}/pn/{pageNumber}/")]
        public IHttpActionResult Get(string categoryIds, int posts, int pageNumber)
        {
            Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles = null;
            try
            {
                int startIndex = 0, endIndex = 0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                objFeaturedArticles = _objCMSContent.GetArticlesByCategoryList(categoryIds, startIndex, endIndex, 0, 0);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    PwaContentBase objPWAArticles = new PwaContentBase();
                    objPWAArticles.Articles = ConverterUtility.MapArticleSummaryListToPwaArticleSummaryList(objFeaturedArticles.Articles);
                    objPWAArticles.RecordCount = objFeaturedArticles.RecordCount;
                    objPWAArticles.StartIndex = (uint)startIndex;
                    objPWAArticles.EndIndex = (uint)endIndex;
                    return Ok(objPWAArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Pwa.CMS.CMSController");

                return InternalServerError();
            }
            return NotFound();
        }  //get

        /// <summary>
        /// Created By : Pratibha Verma on 24 February, 2018
        /// Summary : Common method to get recent content for news and expert reviews based on category IDs,articlesPerPage and Pageno.
        /// </summary>
        private PwaContentBase GetArticleList(string categoryIds, int posts, int pageNumber)
        {
            Bikewale.Entities.CMS.Articles.CMSContent objFeaturedArticles = null;
            PwaContentBase objPWAArticles = null;
            try
            {
                int startIndex = 0, endIndex = 0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                objFeaturedArticles = _objCMSContent.GetArticlesByCategoryList(categoryIds, startIndex, endIndex, 0, 0);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    objPWAArticles = new PwaContentBase();
                    objPWAArticles.Articles = ConverterUtility.MapArticleSummaryListToPwaArticleSummaryList(objFeaturedArticles.Articles);
                    objPWAArticles.RecordCount = objFeaturedArticles.RecordCount;
                    objPWAArticles.StartIndex = (uint)startIndex;
                    objPWAArticles.EndIndex = (uint)endIndex;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Pwa.CMS.CMSController");
            }
            return objPWAArticles;
        }
        /// <summary>
        /// Created By : Pratibha Verma on 24 February, 2018
        /// Summary : API to get recent content for news.
        /// </summary>
        [ResponseType(typeof(IEnumerable<PwaContentBase>)), Route("api/pwa/cms/news/posts/{articlePerPage}/pn/{pageNumber}/")]
        public IHttpActionResult GetNews(int articlePerPage, int pageNumber) {            
            try
            {
                PwaContentBase objPWAArticles = GetArticleList("1,19,6,8,2,18,5,26", articlePerPage, pageNumber);
                if (objPWAArticles != null)
                {
                    objPWAArticles.PageTitle = "Bike News";
                }
                return Ok(objPWAArticles);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Pwa.CMS.CMSController.GetNews");

                return InternalServerError();
            }
        }
        /// <summary>
        /// Created By : Pratibha Verma on 24 February, 2018
        /// Summary : API to get recent content for expert-reviews.
        /// </summary>
        [ResponseType(typeof(IEnumerable<PwaContentBase>)), Route("api/pwa/cms/expertreviews/posts/{articlePerPage}/pn/{pageNumber}/")]
        public IHttpActionResult GetExpertReviews(int articlePerPage, int pageNumber)
        {
            try
            {
                PwaContentBase objPWAArticles = GetArticleList("8", articlePerPage, pageNumber);
                if (objPWAArticles != null)
                {
                    objPWAArticles.PageTitle = "Expert Reviews";
                }
                return Ok(objPWAArticles);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Pwa.CMS.CMSController.GetExpertReviews");

                return InternalServerError();
            }
        }
        #endregion


    }   // class
}   // namespace


