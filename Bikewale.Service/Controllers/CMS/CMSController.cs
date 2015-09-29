using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Bikewale.Entities.BikeData;
using System.Web.Http.Description;
using Bikewale.Entities.CMS.Photos;
using Bikewale.DTO.CMS;
using Bikewale.Utility;
using System.Net.Http.Formatting;
using Bikewale.Interfaces.Pager;
using Microsoft.Practices.Unity;
using Bikewale.BAL.Pager;
using Bikewale.Entities.CMS;
using Bikewale.DTO.CMS.Photos;
using AutoMapper;
using Bikewale.DTO.Make;
using Bikewale.DTO.Model;
using Newtonsoft.Json;
using Bikewale.Entities.CMS.Articles;
using Bikewale.DTO.CMS.Articles;
using Bikewale.DTO.Version;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.CMS
{
    /// <summary>
    /// Edit CMS Controller :  All Edit CMS related Operations 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class CMSController : ApiController
    {
        string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        string _requestType = "application/json";


        private readonly IPager _pager = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pager"></param>
        public CMSController(IPager pager)
        {
            _pager = pager;
        }


        #region ModelImages List Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get list of photos for the specified model.
        /// </summary>
        /// <param name="modelId">Mandatory field. Value should be greater than 0.</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>)), Route("api/cms/photos/model/{modelId}")]
        public IHttpActionResult Get(int modelId)
        {
            List<EnumCMSContentType> categorList = null;
            List<ModelImage> objImageList = null;

            try
            {
                categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);
                string _apiUrl = String.Format("/webapi/image/modelphotolist/?applicationid={0}&modelid={1}&categoryidlist={2}", _applicationid, modelId, contentTypeList);


                objImageList = BWHttpClient.GetApiResponseSync<List<ModelImage>>(_cwHostUrl, _requestType, _apiUrl, objImageList);
                if (objImageList != null && objImageList.Count > 0)
                {
                    // Auto map the properties
                    List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                    objCMSModels = CMSMapper.Convert(objImageList);

                    return Ok(objCMSModels);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();

                return InternalServerError();

            }
            return NotFound();
        }  //get  ModelImages 
        #endregion


        #region Other Model Images List Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get list of models with main image of each model for the same make as of specified model.
        /// </summary>
        /// <param name="modelId">Mandatory.</param>
        /// <param name="posts">Mandatory. No of records on each page.</param>
        /// <param name="pageNumber">Page number for which records are required.</param>        
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<CMSImageList>)), Route("api/cms/photos/othermodels/modelId/{modelId}/posts/{posts}/pn/{pageNumber}/")]
        public IHttpActionResult GetOtherModelsPhotos(int modelId, int posts, int pageNumber)
        {
            int startIndex = 0, endIndex = 0;
            CMSImage objPhotos = null;
            List<EnumCMSContentType> categorList = null;

            try
            {
                categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.RoadTest);
                categorList.Add(EnumCMSContentType.PhotoGalleries);
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                _pager.GetStartEndIndex(Convert.ToInt32(posts), Convert.ToInt32(pageNumber), out startIndex, out endIndex);

                string _apiUrl = "/webapi/image/othermodelphotolist/?applicationid=2&startindex=" + startIndex + "&endindex=" + endIndex + "&modelid=" + modelId + "&categoryidlist=" + contentTypeList;
                objPhotos = BWHttpClient.GetApiResponseSync<CMSImage>(_cwHostUrl, _requestType, _apiUrl, objPhotos);

                if (objPhotos != null)
                {
                    CMSImageList objCMSModelImageList = new CMSImageList();
                    objCMSModelImageList = CMSMapper.Convert(objPhotos);
                    return Ok(objCMSModelImageList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }  //othermodelist api
        #endregion

        #region Article Content Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble.
        /// Summary : API to get details of article. This is api is used for the articles having multiple pages. e.g. Road Tests, Expert Reviews, Features.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>Article Details</returns>
        [ResponseType(typeof(CMSArticlePageDetails)), Route("api/cms/id/{basicId}/pages/")]
        public IHttpActionResult Get(string basicId)
        {
            try
            {
                ArticlePageDetails objFeaturedArticles = null;
                
                string _apiUrl = "/webapi/article/contentpagedetail/?basicid=" + basicId;

                objFeaturedArticles = BWHttpClient.GetApiResponseSync<ArticlePageDetails>(_cwHostUrl, _requestType, _apiUrl, objFeaturedArticles);

                if (objFeaturedArticles != null)
                {
                    CMSArticlePageDetails objCMSFArticles = new CMSArticlePageDetails();
                    objCMSFArticles = CMSMapper.Convert(objFeaturedArticles);

                    objCMSFArticles.FormattedDisplayDate = Bikewale.Utility.FormatDate.GetDaysAgo(objFeaturedArticles.DisplayDate);

                    // If android, IOS client sanitize the article content 
                    string platformId = string.Empty;

                    if(Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    {
                        foreach (var page in objCMSFArticles.PageList)
                        {
                            Bikewale.Entities.CMS.Articles.HtmlContent objContent = Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent(page.Content);

                            if (objContent.HtmlItems != null && objContent.HtmlItems.Count > 0)
                            {
                                DTO.CMS.Articles.HtmlContent htmlContent = new DTO.CMS.Articles.HtmlContent();
                                htmlContent.HtmlItems = objContent.HtmlItems.Select(item => new DTO.CMS.Articles.HtmlItem() { Content = item.Content, ContentList = item.ContentList, SetMargin = item.SetMargin, Type = item.Type }).ToList();

                                page.htmlContent = htmlContent;
                                page.Content = "";
                            }
                        }
                    }

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
        }  //get article content
        #endregion


        #region News Details Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get details of article. This is api is used for the articles single page. e.g. News.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns>News Details</returns>
        [ResponseType(typeof(CMSArticleDetails)), Route("api/cms/id/{basicId}/page/")]
        public IHttpActionResult GetArticleDetailsPage(string basicId)
        {
            DTO.CMS.Articles.HtmlContent htmlContent = null;
            CMSArticleDetails objCMSFArticles = null;
            try
            {
                ArticleDetails objNews = null;

                string _apiUrl = "/webapi/article/contentdetail/?basicid=" + basicId;
                objNews = BWHttpClient.GetApiResponseSync<ArticleDetails>(_cwHostUrl, _requestType, _apiUrl, objNews);

                if (objNews != null)
                {                    
                    objCMSFArticles = new CMSArticleDetails();
                    objCMSFArticles = CMSMapper.Convert(objNews);

                    objCMSFArticles.FormattedDisplayDate = Bikewale.Utility.FormatDate.GetDaysAgo(objNews.DisplayDate);
                    
                    // If android, IOS client execute this code
                    string platformId = string.Empty;

                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    {
                        Bikewale.Entities.CMS.Articles.HtmlContent objContent = Bikewale.Utility.SanitizeHtmlContent.GetFormattedContent(objNews.Content);

                        if (objContent.HtmlItems != null && objContent.HtmlItems.Count > 0)
                        {
                            htmlContent = new DTO.CMS.Articles.HtmlContent();
                            htmlContent.HtmlItems = objContent.HtmlItems.Select(item => new DTO.CMS.Articles.HtmlItem() { Content = item.Content, ContentList = item.ContentList, SetMargin = item.SetMargin, Type = item.Type }).ToList();

                            objCMSFArticles.htmlContent = htmlContent;
                            objCMSFArticles.Content = "";
                        }
                    }
                    
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
        }  //get News Details
        #endregion


        #region Article Photos Api
        /// <summary>
        /// Modified By : Ashish G. Kamble
        /// Summary : API to get list of photos for the specified article.
        /// </summary>
        /// <param name="basicId">Mandatory field.</param>
        /// <returns>Returns list of photos.</returns>
        [ResponseType(typeof(IEnumerable<CMSModelImageBase>)), Route("api/cms/id/{basicId}/photos/")]
        public IHttpActionResult GetArticlePhotos(string basicId)
        {
            try
            {
                string _apiUrl = "/webapi/image/GetArticlePhotos/?basicid=" + basicId;
                List<ModelImage> objImg = null;

                objImg = BWHttpClient.GetApiResponseSync<List<ModelImage>>(_cwHostUrl, _requestType, _apiUrl, objImg);

                if (objImg != null && objImg.Count > 0)
                {
                    // Auto map the properties
                    List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                    objCMSModels = CMSMapper.Convert(objImg);
                    return Ok(objCMSModels);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.CMS.CMSController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }  //get Articles Photos       
        #endregion

    }   // class
}   // namespace
