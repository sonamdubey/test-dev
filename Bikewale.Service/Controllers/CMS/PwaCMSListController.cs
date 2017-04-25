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
        [ResponseType(typeof(IEnumerable<PwaContentBase>)), Route("api/pwa/cat/{categoryIds}/posts/{posts}/pn/{pageNumber}/")]
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Pwa.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get 
        
        #endregion


    }   // class
}   // namespace


