using Bikewale.DTO.CMS.Articles;
using Bikewale.Entities.CMS;
using Bikewale.Entity.CMS.Articles;
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
        /// To get Recent Categories Content List
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="posts"></param>
        /// <returns>Recent Articles List Summary</returns>
        [ResponseType(typeof(IEnumerable<CMSArticleSummary>))]
        public IHttpActionResult Get(EnumCMSContentType categoryId, uint posts, string makeId = null, string modelId = null)
        {
            List<ArticleSummary> objRecentArticles = null;
            try
            {
                string apiUrl = "";
                if (!String.IsNullOrEmpty(makeId) || !String.IsNullOrEmpty(modelId))
                {
                    if (!String.IsNullOrEmpty(modelId))
                        apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + (int)categoryId + "&totalrecords=" + posts + "&makeid=" + makeId + "&modelid=" + modelId;
                    else
                        apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + (int)categoryId + "&totalrecords=" + posts + "&makeid=" + makeId;
                }
                else
                {
                    apiUrl = "/webapi/article/mostrecentlist/?applicationid=2&contenttypes=" + categoryId + "&totalrecords=" + posts;
                }

                objRecentArticles = BWHttpClient.GetApiResponseSync<List<ArticleSummary>>(_cwHostUrl, _requestType, apiUrl, objRecentArticles);

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {

                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);
                    return Ok(objRecentArticles);
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
        ///  To get content of Specified category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>Category Content List</returns>
        [ResponseType(typeof(IEnumerable<Bikewale.DTO.CMS.Articles.CMSContent>))]
        public IHttpActionResult Get(EnumCMSContentType CategoryId, string makeId, string modelId,int pageSize, int pageNumber)
        {
            List<Bikewale.Entities.CMS.Articles.CMSContent> objFeaturedArticles = null;
            try
            {
                int startIndex=0, endIndex=0;
                _pager.GetStartEndIndex(Convert.ToInt32(pageSize), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                string apiUrl = "webapi/article/listbycategory/?applicationid=2&categoryidlist=";
                if (CategoryId != EnumCMSContentType.RoadTest)
                {
                    apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex;
                }
                else
                {
                    if (String.IsNullOrEmpty(makeId))
                    {
                        apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(modelId))
                        {
                            apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex + "&makeid=" + makeId;
                        }
                        else
                        {
                            apiUrl += (int)CategoryId + "&startindex=" + startIndex + "&endindex=" + endIndex + "&makeid=" + makeId + "&modelid=" + modelId;

                        }
                    }
                }

                objFeaturedArticles = BWHttpClient.GetApiResponseSync<List<Bikewale.Entities.CMS.Articles.CMSContent>>(_cwHostUrl, _requestType, apiUrl, objFeaturedArticles);

                if (objFeaturedArticles != null && objFeaturedArticles.Count > 0)
                {
                    List<Bikewale.DTO.CMS.Articles.CMSContent> objCMSFArticles = new List<Bikewale.DTO.CMS.Articles.CMSContent>();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);
                    return Ok(objFeaturedArticles);

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



    }
}
