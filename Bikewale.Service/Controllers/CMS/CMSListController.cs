using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.Entities.CMS.Articles;

namespace Bikewale.Service.Controllers.CMS
{

    /// <summary>
    /// Edit CMS List Controller :  Operations related to list of content 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class CMSListController : ApiController
    {
        string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        string _requestType = "application/json"; 
        
 
        private readonly IPager _pager = null;
        public CMSListController(IPager pager)
        {
            _pager = pager;
        }

        #region List Recent Categories Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of a specified category.
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>      
        /// <param name="posts">No of records needed. should be greater than 0.</param>
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/{categoryId}/posts/{posts}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts)
        {
            List<ArticleSummary> objRecentArticles = null;
            try
            {
                string apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&totalrecords=" + posts;

                if (categoryId == EnumCMSContentType.RoadTest)
                {
                    apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
                }
                else
                {
                    apiUrl += "&contenttypes=" + (short)categoryId;
                }
                
                objRecentArticles = BWHttpClient.GetApiResponseSync<List<ArticleSummary>>(_cwHostUrl, _requestType, apiUrl, objRecentArticles);

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {

                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);
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
        /// </summary>
        /// <param name="categoryId">Id of the category whose data is required.</param>
        /// <param name="posts">No of records needed. should be greater than 0.</param>
        /// <param name="makeId">Mandetory field.</param>
        /// <param name="modelId">Optional.</param>        
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>)), Route("api/cms/cat/{categoryId}/posts/{posts}/make/{makeId}/")]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts, string makeId, string modelId = null)
        {
            List<ArticleSummary> objRecentArticles = null;
            try
            {
                string apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&totalrecords=" + posts;

                if (categoryId == EnumCMSContentType.RoadTest)
                {
                    apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
                }
                else
                {
                    apiUrl += "&contenttypes=" + (short)categoryId;
                }

                if (String.IsNullOrEmpty(modelId))
                {
                    apiUrl += "&makeid=" + makeId;
                }
                else
                {                    
                    apiUrl += "&makeid=" + makeId + "&modelid=" + modelId;
                }

                objRecentArticles = BWHttpClient.GetApiResponseSync<List<ArticleSummary>>(_cwHostUrl, _requestType, apiUrl, objRecentArticles);

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {

                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);
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


        #region List Category Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of specified category. This api returns data for given page number.
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
                int startIndex=0, endIndex=0;
                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                string apiUrl = "/webapi/article/listbycategory/?applicationid=2";

                if (categoryId == EnumCMSContentType.RoadTest)
                {
                    apiUrl += "&categoryidlist=" + (int)categoryId + "," + (int)EnumCMSContentType.ComparisonTests;
                }
                else
                {
                    apiUrl += "&categoryidlist=" + (int)categoryId;
                }

                apiUrl += "&startindex=" + startIndex + "&endindex=" + endIndex;

                objFeaturedArticles = BWHttpClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(_cwHostUrl, _requestType, apiUrl, objFeaturedArticles);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);
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


        #region List Category Content
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get recent content of specified category for the given make or model. This api return data for given page number.
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

                string apiUrl = "/webapi/article/listbycategory/?applicationid=2";

                if (categoryId == EnumCMSContentType.RoadTest)
                {
                    apiUrl += "&categoryidlist=" + (int)categoryId + "," + (int)EnumCMSContentType.ComparisonTests;
                }
                else
                {
                    apiUrl += "&categoryidlist=" + (int)categoryId;
                }
                
                if (String.IsNullOrEmpty(modelId))
                {
                    apiUrl += "&makeid=" + makeId;
                }
                else
                {
                    apiUrl += "&makeid=" + makeId + "&modelid=" + modelId;

                }

                apiUrl += "&startindex=" + startIndex + "&endindex=" + endIndex;

                objFeaturedArticles = BWHttpClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(_cwHostUrl, _requestType, apiUrl, objFeaturedArticles);

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);
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
