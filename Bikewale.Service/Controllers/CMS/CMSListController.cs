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
using Bikewale.Service.Utilities;

namespace Bikewale.Service.Controllers.CMS
{

    /// <summary>
    /// Edit CMS List Controller :  Operations related to list of content 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class CMSListController : ApiController
    {
        string _applicationid = Utility.BWConfiguration.Instance.ApplicationId;                
 
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
                else if (categoryId == EnumCMSContentType.News)
                {
                    apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
                }
                else
                {
                    apiUrl += "&contenttypes=" + (short)categoryId;
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objRecentArticles = objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
                    objRecentArticles = objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
                }

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {

                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);

                    objRecentArticles.Clear();
                    objRecentArticles = null;

                    objCMSRArticles = new CMSShareUrl().GetShareUrl(objCMSRArticles);
                    //foreach (var article in objCMSRArticles)
                    //{
                    //    EnumCMSContentType contentType = (EnumCMSContentType)article.CategoryId;
                    //    switch (contentType)
                    //    {
                    //        case EnumCMSContentType.News:
                    //        case EnumCMSContentType.AutoExpo2016:
                    //            article.ShareUrl = _bwHostUrl + "/news/" + article.BasicId + "-" + article.ArticleUrl + ".html";
                    //            break;
                    //        case EnumCMSContentType.Features:
                    //            article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                    //            break;
                    //        case EnumCMSContentType.RoadTest:
                    //            article.ShareUrl = _bwHostUrl + "/road-tests/" + article.ArticleUrl + "-" + article.BasicId + ".html";
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
                    //}

                   // objCMSRArticles.ForEach(s => s.FormattedDisplayDate = s.DisplayDate.ToString("MMM dd, yyyy"));

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
                string _bwHostUrl = ConfigurationManager.AppSettings["bwHostUrl"];

                if (categoryId == EnumCMSContentType.RoadTest)
                {
                    apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.ComparisonTests;
                }
                else if (categoryId == EnumCMSContentType.News)
                {
                    apiUrl += "&contenttypes=" + (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
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

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objRecentArticles = objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
                    objRecentArticles = objClient.GetApiResponseSync<List<ArticleSummary>>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objRecentArticles);
                }

                if (objRecentArticles != null && objRecentArticles.Count > 0)
                {
                    List<CMSArticleSummary> objCMSRArticles = new List<CMSArticleSummary>();
                    objCMSRArticles = CMSMapper.Convert(objRecentArticles);

                    objRecentArticles.Clear();
                    objRecentArticles = null;
                    objCMSRArticles = new CMSShareUrl().GetShareUrl(objCMSRArticles);
                    //foreach (var article in objCMSRArticles)
                    //{
                    //    EnumCMSContentType contentType = (EnumCMSContentType)article.CategoryId;
                    //    switch (contentType)
                    //    {
                    //        case EnumCMSContentType.News:
                    //        case EnumCMSContentType.AutoExpo2016:
                    //            article.ShareUrl = _bwHostUrl + "/news/" + article.BasicId + "-" + article.ArticleUrl + ".html";
                    //            break;
                    //        case EnumCMSContentType.Features:
                    //            article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                    //            break;
                    //        case EnumCMSContentType.RoadTest:
                    //            article.ShareUrl = _bwHostUrl + "/road-tests/" + article.ArticleUrl + "-" + article.BasicId + ".html";
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
                    //}

                    //objCMSRArticles.ForEach(s => s.FormattedDisplayDate = s.DisplayDate.ToString("MMM dd, yyyy"));

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
                    //objFeaturedArticles = objClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objFeaturedArticles);
                    objFeaturedArticles = objClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objFeaturedArticles);
                }

                if (objFeaturedArticles != null && objFeaturedArticles.Articles.Count > 0)
                {
                    Bikewale.DTO.CMS.Articles.CMSContent objCMSFArticles = new Bikewale.DTO.CMS.Articles.CMSContent();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    objFeaturedArticles.Articles.Clear();
                    objFeaturedArticles.Articles = null;
                    objCMSFArticles = new CMSShareUrl().GetShareUrl(objCMSFArticles);
                    //foreach (var article in objCMSFArticles.Articles)
                    //{
                    //    EnumCMSContentType contentType =  (EnumCMSContentType)article.CategoryId;
                    //    switch (contentType)
                    //    {
                    //        case EnumCMSContentType.News:
                    //        case EnumCMSContentType.AutoExpo2016:
                    //            article.ShareUrl =  _bwHostUrl +"/news/" + article.BasicId + "-" + article.ArticleUrl + ".html";
                    //            break;
                    //        case EnumCMSContentType.Features:
                    //            article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId;;
                    //            break;
                    //        case EnumCMSContentType.RoadTest:
                    //            article.ShareUrl = _bwHostUrl+ "/road-tests/" + article.ArticleUrl + "-" + article.BasicId + ".html";
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
                    //}
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
                else if (categoryId == EnumCMSContentType.News)
                {
                    apiUrl += "&categoryidlist=" + (short)categoryId + "," + (short)EnumCMSContentType.AutoExpo2016;
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

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    //objFeaturedArticles = objClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(Utility.BWConfiguration.Instance.CwApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objFeaturedArticles);
                    objFeaturedArticles = objClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objFeaturedArticles);
                }

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

                    //foreach (var article in objCMSFArticles.Articles)
                    //{
                    //    EnumCMSContentType contentType = (EnumCMSContentType)article.CategoryId;
                    //    switch (contentType)
                    //    {
                    //        case EnumCMSContentType.News:
                    //        case EnumCMSContentType.AutoExpo2016:
                    //            article.ShareUrl = _bwHostUrl + "/news/" + article.BasicId + "-" + article.ArticleUrl + ".html";
                    //            break;
                    //        case EnumCMSContentType.Features:
                    //            article.ShareUrl = _bwHostUrl + "/features/" + article.ArticleUrl + "-" + article.BasicId; ;
                    //            break;
                    //        case EnumCMSContentType.RoadTest:
                    //            article.ShareUrl = _bwHostUrl + "/road-tests/" + article.ArticleUrl + "-" + article.BasicId + ".html";
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    article.FormattedDisplayDate = article.DisplayDate.ToString("MMM dd, yyyy");
                    //}

                   // objCMSFArticles.Articles.ToList().ForEach(s => s.FormattedDisplayDate = s.DisplayDate.ToString("MMM dd, yyyy"));

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
